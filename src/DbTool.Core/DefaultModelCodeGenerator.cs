using System;
using System.Text;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public class DefaultCSharpModelCodeGenerator : IModelCodeGenerator
    {
        private readonly IModelNameConverter _modelNameConverter;

        public string FileExtension => ".cs";

        public string CodeType => "C#";

        public DefaultCSharpModelCodeGenerator(IModelNameConverter modelNameConverter)
        {
            _modelNameConverter = modelNameConverter;
        }

        public virtual string GenerateModelCode(TableEntity tableEntity, ModelCodeGenerateOptions options, IDbProvider dbProvider)
        {
            if (tableEntity == null)
            {
                throw new ArgumentNullException(nameof(tableEntity));
            }
            if (dbProvider == null)
            {
                throw new ArgumentNullException(nameof(dbProvider));
            }
            options ??= new();

            var sbText = new StringBuilder();
            sbText.AppendLine("using System;");
            if (options.GenerateDataAnnotation)
            {
                sbText.AppendLine("using System.ComponentModel;");
                sbText.AppendLine("using System.ComponentModel.DataAnnotations;");
                sbText.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            }
            sbText.AppendLine();
            sbText.AppendLine($"namespace {options.Namespace}");
            sbText.AppendLine("{");
            if (options.GenerateDataAnnotation && !string.IsNullOrEmpty(tableEntity.TableDescription))
            {
                sbText.AppendLine(
                    $"\t/// <summary>{Environment.NewLine}\t/// {tableEntity.TableDescription.Replace(Environment.NewLine, " ")}{Environment.NewLine}\t/// </summary>");
                sbText.AppendLine($"\t[Table(\"{tableEntity.TableName}\")]");
                sbText.AppendLine($"\t[Description(\"{tableEntity.TableDescription.Replace(Environment.NewLine, " ")}\")]");
            }
            sbText.AppendLine($"\tpublic class {options.Prefix}{_modelNameConverter.ConvertTableToModel(tableEntity.TableName)}{options.Suffix}");
            sbText.AppendLine("\t{");
            var index = 0;
            if (options.GeneratePrivateFields)
            {
                foreach (var item in tableEntity.Columns)
                {
                    if (index > 0)
                    {
                        sbText.AppendLine();
                    }
                    else
                    {
                        index++;
                    }
                    var fclType = dbProvider.DbType2ClrType(item.DataType, item.IsNullable);

                    var tmpColName = ToPrivateFieldName(item.ColumnName);
                    sbText.AppendLine($"\t\tprivate {fclType} {tmpColName};");
                    if (options.GenerateDataAnnotation)
                    {
                        if (!string.IsNullOrEmpty(item.ColumnDescription))
                        {
                            sbText.AppendLine(
                                $"\t\t/// <summary>{Environment.NewLine}\t\t/// {item.ColumnDescription.Replace(Environment.NewLine, " ")}{Environment.NewLine}\t\t/// </summary>");
                            if (options.GenerateDataAnnotation)
                            {
                                sbText.AppendLine($"\t\t[Description(\"{item.ColumnDescription.Replace(Environment.NewLine, " ")}\")]");
                            }
                        }
                        else
                        {
                            if (item.IsPrimaryKey)
                            {
                                sbText.AppendLine($"\t\t[Description(\"主键\")]");
                            }
                        }
                        if (item.IsPrimaryKey)
                        {
                            sbText.AppendLine($"\t\t[Key]");
                        }
                        if (fclType == "string" && item.Size > 0 && item.Size < int.MaxValue)
                        {
                            sbText.AppendLine($"\t\t[StringLength({item.Size})]");
                        }
                        sbText.AppendLine($"\t\t[Column(\"{item.ColumnName}\")]");
                    }
                    sbText.AppendLine($"\t\tpublic {fclType} {item.ColumnName}");
                    sbText.AppendLine("\t\t{");
                    sbText.AppendLine($"\t\t\tget {{ return {tmpColName}; }}");
                    sbText.AppendLine($"\t\t\tset {{ {tmpColName} = value; }}");
                    sbText.AppendLine("\t\t}");
                    sbText.AppendLine();
                }
            }
            else
            {
                foreach (var item in tableEntity.Columns)
                {
                    if (index > 0)
                    {
                        sbText.AppendLine();
                    }
                    else
                    {
                        index++;
                    }
                    var fclType = dbProvider.DbType2ClrType(item.DataType, item.IsNullable);

                    if (options.GenerateDataAnnotation)
                    {
                        if (!string.IsNullOrEmpty(item.ColumnDescription))
                        {
                            sbText.AppendLine(
                                $"\t\t/// <summary>{Environment.NewLine}\t\t/// {item.ColumnDescription.Replace(Environment.NewLine, " ")}{Environment.NewLine}\t\t/// </summary>");
                            if (options.GenerateDataAnnotation)
                            {
                                sbText.AppendLine($"\t\t[Description(\"{item.ColumnDescription.Replace(Environment.NewLine, " ")}\")]");
                            }
                        }
                        if (item.IsPrimaryKey)
                        {
                            sbText.AppendLine($"\t\t[Key]");
                        }
                        if (fclType == "string" && item.Size > 0 && item.Size < int.MaxValue)
                        {
                            sbText.AppendLine($"\t\t[StringLength({item.Size})]");
                        }
                        sbText.AppendLine($"\t\t[Column(\"{item.ColumnName}\")]");
                    }
                    sbText.AppendLine($"\t\tpublic {fclType} {item.ColumnName} {{ get; set; }}");
                }
            }
            sbText.AppendLine("\t}");
            sbText.AppendLine("}");
            return sbText.ToString();
        }

        /// <summary>
        /// 将属性名称转换为私有字段名称
        /// </summary>
        /// <param name="propertyName"> 属性名称 </param>
        /// <returns> 私有字段名称 </returns>
        protected virtual string ToPrivateFieldName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return string.Empty;
            }
            // 全部大写的专有名词
            if (propertyName.Equals(propertyName.ToUpperInvariant()))
            {
                return propertyName.ToLowerInvariant();
            }
            // 首字母大写转成小写
            if (char.IsUpper(propertyName[0]))
            {
                return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            }

            return $"_{propertyName}";
        }
    }
}
