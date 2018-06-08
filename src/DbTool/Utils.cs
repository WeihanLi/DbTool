using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
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
            var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();

            var result = provider.CompileAssemblyFromFile(new CompilerParameters(new[] { "System.dll" }), sourceFilePaths);
            if (result.Errors.HasErrors)
            {
                var error = new StringBuilder(result.Errors.Count * 1024);
                for (var i = 0; i < result.Errors.Count; i++)
                {
                    error.AppendLine($"{result.Errors[i].FileName}({result.Errors[i].Line},{result.Errors[i].Column}):{result.Errors[i].ErrorText}");
                }
                throw new ArgumentException($"所选文件编译有错误{Environment.NewLine}{error}");
            }
            var tables = new List<TableEntity>(2);
            foreach (var type in result.CompiledAssembly.GetTypes())
            {
                if (type.IsClass && type.IsPublic && !type.IsAbstract)
                {
                    var table = new TableEntity
                    {
                        TableName = type.Name.TrimModelName(),
                        TableDescription = type.GetCustomAttribute<DescriptionAttribute>()?.Description
                    };
                    var defaultVal = Activator.CreateInstance(type);
                    foreach (var property in type.GetProperties(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Instance))
                    {
                        var columnInfo = new ColumnEntity
                        {
                            ColumnName = property.Name,
                            ColumnDescription = property.GetCustomAttribute<DescriptionAttribute>()?.Description,
                            IsNullable = property.PropertyType.Unwrap() != property.PropertyType
                        };
                        var val = property.GetValue(defaultVal);
                        columnInfo.DefaultValue =
                            null == val || property.PropertyType.GetDefaultValue().Equals(val) || columnInfo.IsNullable
                            ? null : val;
                        columnInfo.IsPrimaryKey = columnInfo.ColumnDescription?.Contains("主键") ?? false;
                        columnInfo.DataType = Utility.FclType2DbType(property.PropertyType).ToString();
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
