using System;
using System.Text;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public class DefaultModelCodeGenerator : IModelCodeGenerator
    {
        public string GenerateModelCode(TableEntity tableEntity, ModelCodeGenerateOptions options)
        {
            if (tableEntity == null)
            {
                throw new ArgumentNullException(nameof(tableEntity));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(tableEntity));
            }

            var sbText = new StringBuilder();
            sbText.AppendLine("using System;");
            if (options.GenerateDescriptionAttribute)
            {
                sbText.AppendLine("using System.ComponentModel;");
            }
            sbText.AppendLine();
            sbText.AppendLine($"namespace {options.Namespace}");
            sbText.AppendLine("{");
            if (!string.IsNullOrEmpty(tableEntity.TableDescription))
            {
                sbText.AppendLine(
                    $"\t/// <summary>{Environment.NewLine}\t/// {tableEntity.TableDescription.Replace(Environment.NewLine, " ")}{Environment.NewLine}\t/// </summary>");
                if (options.GenerateDescriptionAttribute)
                {
                    sbText.AppendLine($"\t[Description(\"{tableEntity.TableDescription.Replace(Environment.NewLine, " ")}\")]");
                }
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
                    var fclType = Utility.SqlDbType2FclType(item.DataType, item.IsNullable); //转换为FCL数据类型

                    var tmpColName = item.ColumnName.Trim().ToPrivateFieldName();
                    sbText.AppendLine($"\t\tprivate {fclType} {tmpColName};");
                    if (!string.IsNullOrEmpty(item.ColumnDescription))
                    {
                        sbText.AppendLine(
                            $"\t\t/// <summary>{Environment.NewLine}\t\t/// {item.ColumnDescription.Replace(Environment.NewLine, " ")}{Environment.NewLine}\t\t/// </summary>");
                        if (options.GenerateDescriptionAttribute)
                        {
                            sbText.AppendLine($"\t\t[Description(\"{item.ColumnDescription.Replace(Environment.NewLine, " ")}\")]");
                        }
                    }
                    else
                    {
                        if (item.IsPrimaryKey && options.GenerateDescriptionAttribute)
                        {
                            sbText.AppendLine($"\t\t[Description(\"主键\")]");
                        }
                    }
                    sbText.AppendLine($"\t\tpublic {fclType} {item.ColumnName.Trim()}");
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
                    var fclType = Utility.SqlDbType2FclType(item.DataType, item.IsNullable); //转换为FCL数据类型

                    if (!string.IsNullOrEmpty(item.ColumnDescription))
                    {
                        sbText.AppendLine(
                            $"\t\t/// <summary>{Environment.NewLine}\t\t/// {item.ColumnDescription.Replace(Environment.NewLine, " ")}{Environment.NewLine}\t\t/// </summary>");
                        if (options.GenerateDescriptionAttribute)
                        {
                            sbText.AppendLine($"\t\t[Description(\"{item.ColumnDescription.Replace(Environment.NewLine, " ")}\")]");
                        }
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
