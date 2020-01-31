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
        public static List<TableEntity> GetTableEntityFromSourceCode(params string[] sourceFilePaths)
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
            var weihanLiCommonReference = MetadataReference.CreateFromFile(typeof(IDependencyResolver).Assembly.Location);

            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCodeText, new CSharpParseOptions(LanguageVersion.Latest));

            // A single, immutable invocation to the compiler
            // to produce a library
            var assemblyName = $"DbTool.DynamicGenerated.{GuidIdGenerator.Instance.NewId()}";
            var compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(systemReference, annotationReference, weihanLiCommonReference)
                .AddSyntaxTrees(syntaxTree);
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
            var currentDbType = ConfigurationHelper.AppSetting(ConfigurationConstants.DbType);
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
                            (columnInfo.IsNullable
                             || null == val
                             || val.Equals(defaultPropertyValue))
                            ? null : val;
                        columnInfo.IsPrimaryKey = property.Name == "Id" || columnInfo.ColumnDescription?.Contains("主键") == true;
                        columnInfo.DataType = ClrType2DbType(property.PropertyType, currentDbType);

                        // use VARCHAR for MySql
                        if (!currentDbType.EqualsIgnoreCase("SqlServer") && columnInfo.DataType.Equals("NVARCHAR"))
                        {
                            columnInfo.DataType = "VARCHAR";
                        }
                        columnInfo.Size = GetDefaultSizeForDbType(columnInfo.DataType, 64, currentDbType);
                        table.Columns.Add(columnInfo);
                    }
                    tables.Add(table);
                }
            }

            return tables;
        }

        /// <summary>
        /// FCL类型转换为DbType
        /// </summary>
        /// <param name="type">Fcl 类型</param>
        /// <param name="databaseType">数据库类型</param>
        /// <returns></returns>
        public static string ClrType2DbType(Type type, string databaseType)
        {
            if (databaseType.IsNullOrEmpty())
            {
                databaseType = ConfigurationHelper.AppSetting(ConfigurationConstants.DbType);
            }
            return DependencyResolver.Current.ResolveService<DbProviderFactory>()
                .GetDbProvider(databaseType).ClrType2DbType(type);
        }

        /// <summary>
        /// 数据库数据类型转换为FCL类型
        /// </summary>
        /// <param name="dbType"> 数据库数据类型 </param>
        /// <param name="isNullable"> 该数据列是否可以为空 </param>
        /// <param name="databaseType">数据库类型</param>
        /// <returns></returns>
        public static string SqlDbType2ClrType(string dbType, bool isNullable, string databaseType)
        {
            if (databaseType.IsNullOrEmpty())
            {
                databaseType = ConfigurationHelper.AppSetting(ConfigurationConstants.DbType);
            }
            return DependencyResolver.Current.ResolveService<DbProviderFactory>()
                .GetDbProvider(databaseType)?.DbType2ClrType(dbType, isNullable);
        }

        /// <summary>
        /// 获取数据库类型对应的默认长度
        /// </summary>
        /// <param name="dbType">数据类型</param>
        /// <param name="defaultLength">自定义默认长度</param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static uint GetDefaultSizeForDbType(string dbType, uint defaultLength = 64, string databaseType = null)
        {
            if (databaseType.IsNullOrEmpty())
            {
                databaseType = ConfigurationHelper.AppSetting(ConfigurationConstants.DbType);
            }
            return DependencyResolver.Current.ResolveService<DbProviderFactory>()
                       .GetDbProvider(databaseType)?.GetDefaultSizeForDbType(dbType, defaultLength) ?? defaultLength;
        }
    }
}
