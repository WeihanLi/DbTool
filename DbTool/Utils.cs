using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using WeihanLi.Common;
using WeihanLi.Common.Models;
using WeihanLi.Extensions;

namespace DbTool
{
    internal static class Utils
    {
        /// <summary>
        /// 从 源代码 中获取表信息
        /// </summary>
        /// <param name="sourceFilePaths">sourceCodeFiles</param>
        /// <param name="dbProvider">dbProvider</param>
        /// <returns></returns>
        public static List<TableEntity> GetTableEntityFromSourceCode(this IDbProvider dbProvider, params string[] sourceFilePaths)
        {
            if (sourceFilePaths == null || sourceFilePaths.Length <= 0)
            {
                return null;
            }
            var usingList = new List<string>();

            var sourceCodeTextBuilder = new StringBuilder();

            foreach (var path in sourceFilePaths)
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    if (line.StartsWith("using ") && line.EndsWith(";"))
                    {
                        //
                        usingList.AddIfNotContains(line);
                    }
                    else
                    {
                        sourceCodeTextBuilder.AppendLine(line);
                    }
                }
            }

            var sourceCodeText =
                $"{usingList.StringJoin(Environment.NewLine)}{Environment.NewLine}{sourceCodeTextBuilder}";
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCodeText, new CSharpParseOptions(LanguageVersion.Latest));

            // https://github.com/dotnet/roslyn/issues/34111
            var references =
                new[]
                    {
                        typeof(object).Assembly,
                        typeof(TableAttribute).Assembly,
                        typeof(ResultModel).Assembly,
                        typeof(DescriptionAttribute).Assembly,
                        Assembly.Load("netstandard"),
                        Assembly.Load("System.Runtime"),
                    }
                    .Select(assembly => assembly.Location)
                    .Distinct()
                    .Select(l => MetadataReference.CreateFromFile(l))
                    .Cast<MetadataReference>()
                    .ToArray();

            // A single, immutable invocation to the compiler
            // to produce a library
            var assemblyName = $"DbTool.DynamicGenerated.{GuidIdGenerator.Instance.NewId()}";
            var compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release, allowUnsafe: true))
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTree);
            using (var ms = new MemoryStream())
            {
                var compilationResult = compilation.Emit(ms);
                if (compilationResult.Success)
                {
                    var assemblyBytes = ms.ToArray();
                    return GetTableEntityFromAssembly(Assembly.Load(assemblyBytes), dbProvider);
                }

                var error = new StringBuilder(compilationResult.Diagnostics.Length * 1024);
                foreach (var t in compilationResult.Diagnostics)
                {
                    error.AppendLine($"{t.GetMessage()}");
                }
                throw new ArgumentException($"所选文件编译有错误{Environment.NewLine}{error}");
            }
        }

        /// <summary>
        /// 从 Assembly 中提取 Model 信息
        /// </summary>
        /// <param name="assembly">assembly</param>
        /// <param name="dbProvider">currentDbType</param>
        /// <returns></returns>
        private static List<TableEntity> GetTableEntityFromAssembly(Assembly assembly, IDbProvider dbProvider)
        {
            var tables = new List<TableEntity>(4);
            foreach (var type in assembly.GetExportedTypes().Where(x => x.IsClass && !x.IsAbstract))
            {
                var table = new TableEntity
                {
                    TableName = type.Name.TrimModelName(),
                    TableDescription = type.GetCustomAttribute<DescriptionAttribute>()?.Description
                };
                var tableAttr = type.GetCustomAttribute<TableAttribute>();
                if (tableAttr != null)
                {
                    table.TableName = tableAttr.Name;
                    table.TableSchema = tableAttr.Schema;
                }
                var defaultVal = Activator.CreateInstance(type);
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.IsDefined(typeof(NotMappedAttribute)))
                    {
                        continue; // not mapped or is navigationProperty
                    }
                    if (property.GetGetMethod().IsVirtual)
                    {
                        continue; // virtual navigationProperty
                    }
                    if (!property.PropertyType.IsBasicType() && !property.PropertyType.IsEnum)
                    {
                        continue; // none basic type
                    }

                    var columnInfo = new ColumnEntity
                    {
                        ColumnName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name,
                        ColumnDescription = property.GetCustomAttribute<DescriptionAttribute>()?.Description,
                    };
                    var defaultPropertyValue = property.PropertyType.GetDefaultValue();
                    if (null == defaultPropertyValue)
                    {
                        // ReferenceType
                        columnInfo.IsNullable = !property.IsDefined(typeof(RequiredAttribute));
                    }
                    else
                    {
                        // ValueType
                        columnInfo.IsNullable = false;
                    }

                    var val = property.GetValue(defaultVal);
                    columnInfo.DefaultValue =
                        (columnInfo.IsNullable
                         || null == val
                         || val.Equals(defaultPropertyValue)
                         )
                        ? null : val;
                    columnInfo.IsPrimaryKey = property.IsDefined(typeof(KeyAttribute))
                                              || property.Name == "Id"
                                              || columnInfo.ColumnDescription?.Contains("主键") == true;
                    columnInfo.DataType = dbProvider.ClrType2DbType(
                        property.PropertyType.IsEnum
                            ? Enum.GetUnderlyingType(property.PropertyType)
                            : property.PropertyType);

                    // use VARCHAR for MySql
                    if (!dbProvider.DbType.EqualsIgnoreCase("SqlServer") && columnInfo.DataType.Equals("NVARCHAR"))
                    {
                        columnInfo.DataType = "VARCHAR";
                    }

                    columnInfo.Size = (uint?)property.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength
                                      ?? dbProvider.GetDefaultSizeForDbType(columnInfo.DataType);
                    table.Columns.Add(columnInfo);
                }
                tables.Add(table);
            }

            return tables;
        }
    }
}
