using System;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;

namespace DbTool
{
    public class DefaultModelCodeGenerator : IModelCodeGenerator
    {
        private readonly DbProviderFactory _dbProviderFactory;

        public DefaultModelCodeGenerator(DbProviderFactory dbProviderFactory)
        {
            _dbProviderFactory = dbProviderFactory;
        }

        public string GenerateModelCode(TableEntity tableEntity, ModelCodeGenerateOptions options, string databaseType)
        {
            if (tableEntity == null)
            {
                throw new ArgumentNullException(nameof(tableEntity));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(tableEntity));
            }

            var dbProvider = _dbProviderFactory.GetDbProvider(databaseType);
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
            sbText.AppendLine($"\tpublic class {options.Prefix}{tableEntity.TableName.TrimTableName()}{options.Suffix}");
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

                    var tmpColName = item.ColumnName.ToPrivateFieldName();
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
    }
}
