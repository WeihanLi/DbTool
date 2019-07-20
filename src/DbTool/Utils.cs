using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace DbTool
{
    public static class Utils
    {
        /// <summary>
        /// 从 源代码 中获取表信息
        /// </summary>
        /// <param name="sourceFilePaths">sourceCodeFiles</param>
        /// <returns></returns>
        public static List<TableEntity> GeTableEntityFromSourceCode(params string[] sourceFilePaths)
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

            var systemReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var annotationReference = MetadataReference.CreateFromFile(typeof(TableAttribute).Assembly.Location);
            var weihanliCommonReference = MetadataReference.CreateFromFile(typeof(IDependencyResolver).Assembly.Location);

            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCodeText, new CSharpParseOptions(LanguageVersion.Latest));

            // A single, immutable invocation to the compiler
            // to produce a library
            var assemblyName = $"DbTool.DynamicGenerated.{ObjectIdGenerator.Instance.NewId()}";
            var compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(systemReference, annotationReference, weihanliCommonReference)
                .AddSyntaxTrees(syntaxTree);
            var assemblyPath = ApplicationHelper.MapPath($"{assemblyName}.dll");
            using (var ms = new MemoryStream())
            {
                var compilationResult = compilation.Emit(ms);
                if (compilationResult.Success)
                {
                    var assemblyBytes = ms.ToArray();
                    return GeTableEntityFromAssembly(Assembly.Load(assemblyBytes));
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
        /// <returns></returns>
        private static List<TableEntity> GeTableEntityFromAssembly(Assembly assembly)
        {
            var tables = new List<TableEntity>(4);

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && type.IsPublic && !type.IsAbstract)
                {
                    var table = new TableEntity
                    {
                        TableName = type.Name.TrimModelName(),
                        TableDescription = type.GetCustomAttribute<DescriptionAttribute>()?.Description
                    };
                    var defaultVal = Activator.CreateInstance(type);
                    foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (property.IsDefined(typeof(NotMappedAttribute)) || property.IsDefined(typeof(ForeignKeyAttribute)))
                        {
                            continue; // not mapped or is navigationProperty
                        }
                        if (property.GetGetMethod().IsVirtual)
                        {
                            continue; // virtual navigationProperty
                        }
                        if (!property.PropertyType.IsBasicType())
                        {
                            continue; // none basic type
                        }

                        var columnInfo = new ColumnEntity
                        {
                            ColumnName = property.Name,
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
                            null == val || property.PropertyType.GetDefaultValue().Equals(val) || columnInfo.IsNullable
                            ? null : val;
                        columnInfo.IsPrimaryKey = property.Name == "Id" || columnInfo.ColumnDescription?.Contains("主键") == true;
                        columnInfo.DataType = Utility.FclType2DbType(property.PropertyType).ToString();

                        // use VARCHAR for MySql
                        if (!ConfigurationHelper.AppSetting(ConfigurationConstants.DbType).EqualsIgnoreCase("SqlServer") && columnInfo.DataType.Equals("NVARCHAR"))
                        {
                            columnInfo.DataType = "VARCHAR";
                        }
                        columnInfo.Size = Utility.GetDefaultSizeForDbType(columnInfo.DataType);
                        table.Columns.Add(columnInfo);
                    }
                    tables.Add(table);
                }
            }

            return tables;
        }
    }
}
