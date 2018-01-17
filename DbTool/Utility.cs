using System;
using System.Text;

namespace DbTool
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 根据数据库表列信息，生成model
        /// </summary>
        /// <param name="tableEntity"> 表信息 </param>
        /// <param name="modelNamespace"> model class 命名空间 </param>
        /// <param name="prefix"> model class前缀 </param>
        /// <param name="suffix"> model class后缀 </param>
        /// <param name="genPrivateField">生成private的字段</param>
        /// <returns></returns>
        public static string GenerateModelText(this TableEntity tableEntity, string modelNamespace, string prefix, string suffix, bool genPrivateField = false)
        {
            if (tableEntity == null)
            {
                throw new ArgumentNullException(nameof(tableEntity), "表信息不能为空");
            }
            var sbText = new StringBuilder();
            sbText.AppendLine("using System;");
            sbText.AppendLine();
            sbText.AppendLine("namespace " + modelNamespace);
            sbText.AppendLine("{");
            if (!string.IsNullOrEmpty(tableEntity.TableDesc))
            {
                sbText.AppendLine("\t/// <summary>" + Environment.NewLine + "\t/// " + tableEntity.TableDesc + Environment.NewLine + "\t/// </summary>");
            }
            sbText.AppendLine("\tpublic class " + prefix + tableEntity.TableName + suffix);
            sbText.AppendLine("\t{");
            var index = 0;
            if (genPrivateField)
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
                    item.DataType = SqlDbType2FclType(item.DataType, item.IsNullable); //转换为FCL数据类型

                    var tmpColName = item.ColumnName.ToPrivateFieldName();
                    sbText.AppendLine("\t\tprivate " + item.DataType + " " + tmpColName + ";");
                    if (!string.IsNullOrEmpty(item.ColumnDesc))
                    {
                        sbText.AppendLine("\t\t/// <summary>" + Environment.NewLine + "\t\t/// " + item.ColumnDesc + Environment.NewLine + "\t\t/// </summary>");
                    }
                    sbText.AppendLine("\t\tpublic " + item.DataType + " " + item.ColumnName);
                    sbText.AppendLine("\t\t{");
                    sbText.AppendLine("\t\t\tget { return " + tmpColName + "; }");
                    sbText.AppendLine("\t\t\tset { " + tmpColName + " = value; }");
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
                    item.DataType = SqlDbType2FclType(item.DataType, item.IsNullable); //转换为FCL数据类型

                    if (!string.IsNullOrEmpty(item.ColumnDesc))
                    {
                        sbText.AppendLine("\t\t/// <summary>" + Environment.NewLine + "\t\t/// " + item.ColumnDesc + Environment.NewLine + "\t\t/// </summary>");
                    }
                    sbText.AppendLine("\t\tpublic " + item.DataType + " " + item.ColumnName + " { get; set; }");
                }
            }
            sbText.AppendLine("\t}");
            sbText.AppendLine("}");
            return sbText.ToString();
        }

        /// <summary>
        /// 数据库数据类型转换为FCL类型
        /// </summary>
        /// <param name="dbType"> 数据库数据类型 </param>
        /// <param name="isNullable"> 该数据列是否可以为空 </param>
        /// <returns></returns>
        public static string SqlDbType2FclType(string dbType, bool isNullable = true)
        {
            var sqlDbType = (DbType)Enum.Parse(typeof(DbType), dbType, true);
            string type;
            switch (sqlDbType)
            {
                case DbType.Bit:
                    type = isNullable ? "bool?" : "bool";
                    break;

                case DbType.Float:
                case DbType.Real:
                    type = isNullable ? "double?" : "double";
                    break;

                case DbType.Binary:
                case DbType.VarBinary:
                case DbType.Image:
                case DbType.Timestamp:
                case DbType.RowVersion:
                    type = "byte[]";
                    break;

                case DbType.TinyInt:
                    type = isNullable ? "byte?" : "byte";
                    break;

                case DbType.SmallInt:
                case DbType.Int:
                    type = isNullable ? "int?" : "int";
                    break;

                case DbType.BigInt:
                    type = isNullable ? "long?" : "long";
                    break;

                case DbType.Char:
                case DbType.NChar:
                case DbType.NText:
                case DbType.NVarChar:
                case DbType.VarChar:
                case DbType.Text:
                    type = "string";
                    break;

                case DbType.Numeric:
                case DbType.Money:
                case DbType.Decimal:
                case DbType.SmallMoney:
                    type = isNullable ? "decimal?" : "decimal";
                    break;

                case DbType.UniqueIdentifier:
                    type = isNullable ? "Guid?" : "Guid";
                    break;

                case DbType.Date:
                case DbType.SmallDateTime:
                case DbType.DateTime:
                case DbType.DateTime2:
                    type = isNullable ? "DateTime?" : "DateTime";
                    break;

                case DbType.Time:
                    type = isNullable ? "TimeSpan?" : "TimeSpan";
                    break;

                case DbType.DateTimeOffset:
                    type = isNullable ? "DateTimeOffset?" : "DateTimeOffset";
                    break;

                default:
                    type = "object";
                    break;
            }
            return type;
        }

        /// <summary>
        /// 获取数据库类型对应的默认长度
        /// </summary>
        /// <param name="dbType">数据类型</param>
        /// <param name="defaultLength">自定义默认长度</param>
        /// <returns></returns>
        public static int GetDefaultSizeForDbType(string dbType, int defaultLength = 64)
        {
            var sqlDbType = (DbType)Enum.Parse(typeof(DbType), dbType, true);
            var len = defaultLength;
            switch (sqlDbType)
            {
                case DbType.BigInt:
                    len = 8;
                    break;

                case DbType.Binary:
                    len = 8;
                    break;

                case DbType.Bit:
                    len = 1;
                    break;

                case DbType.Char:
                    break;

                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    len = 8;
                    break;

                case DbType.Decimal:
                    len = 20;
                    break;

                case DbType.Float:
                    len = 20;
                    break;

                case DbType.Image:
                    break;

                case DbType.Int:
                    len = 4;
                    break;

                case DbType.Money:
                    len = 20;
                    break;

                case DbType.NChar:
                    break;

                case DbType.NText:
                    len = 200;
                    break;

                case DbType.Numeric:
                    len = 20;
                    break;

                case DbType.NVarChar:
                    break;

                case DbType.Real:
                    len = 10;
                    break;

                case DbType.RowVersion:
                    break;

                case DbType.SmallDateTime:
                    len = 4;
                    break;

                case DbType.SmallInt:
                    len = 2;
                    break;

                case DbType.SmallMoney:
                    break;

                case DbType.Text:
                    len = 500;
                    break;

                case DbType.Time:
                    len = 8;
                    break;

                case DbType.Timestamp:
                    break;

                case DbType.TinyInt:
                    len = 1;
                    break;

                case DbType.UniqueIdentifier:
                    len = 16;
                    break;

                    //case DbType.VarBinary:
                    //case DbType.VarChar:
                    //case DbType.Variant:
                    //case DbType.Xml:
                    //case DbType.Structured:
                    //default:
                    //  break;
            }
            return len;
        }

        /// <summary>
        /// 根据表信息生成sql语句
        /// </summary>
        /// <param name="tableEntity"> 表信息 </param>
        /// <param name="genDescriotion">生成描述信息</param>
        /// <returns></returns>
        public static string GenerateSqlStatement(this TableEntity tableEntity, bool genDescriotion = true)
        {
            if (string.IsNullOrEmpty(tableEntity.TableName))
            {
                return "";
            }
            StringBuilder sbSqlText = new StringBuilder(), sbSqlDescText = new StringBuilder();
            //create table
            sbSqlText.AppendFormat("CREATE TABLE [{0}].[{1}](", tableEntity.TableSchema, tableEntity.TableName);
            //create description
            if (genDescriotion && !string.IsNullOrEmpty(tableEntity.TableDesc))
            {
                sbSqlDescText.AppendFormat(DbHelper.AddTableDescSqlFormat, tableEntity.TableName, tableEntity.TableDesc);
            }
            if (tableEntity.Columns.Count > 0)
            {
                foreach (var col in tableEntity.Columns)
                {
                    sbSqlText.AppendLine();
                    sbSqlText.AppendFormat("[{0}] {1}", col.ColumnName, col.DataType);
                    if (col.DataType.ToUpperInvariant().Contains("CHAR"))
                    {
                        sbSqlText.AppendFormat("({0})", col.Size.ToString());
                    }
                    if (col.IsPrimaryKey)
                    {
                        sbSqlText.Append(" PRIMARY KEY ");
                    }
                    //Nullable
                    if (!col.IsNullable)
                    {
                        sbSqlText.Append(" NOT NULL ");
                    }
                    //Default Value
                    if (col.DefaultValue != null && !string.IsNullOrEmpty(col.DefaultValue.ToString()))
                    {
                        if (col.IsPrimaryKey && col.DataType.ToUpper().Contains("INT"))
                        {
                            sbSqlText.Append(" IDENTITY(1,1) ");
                        }
                        else
                        {
                            if (col.DataType.ToUpperInvariant().Contains("CHAR") && !col.DefaultValue.ToString().StartsWith("N'"))
                            {
                                sbSqlText.AppendFormat(" DEFAULT(N'{0}')", col.DefaultValue);
                            }
                            else
                            {
                                sbSqlText.AppendFormat(" DEFAULT({0}) ", col.DefaultValue);
                            }
                        }
                    }
                    //
                    sbSqlText.Append(",");
                    //
                    if (genDescriotion && !string.IsNullOrEmpty(col.ColumnDesc))
                    {
                        sbSqlDescText.AppendLine();
                        sbSqlDescText.AppendFormat(DbHelper.AddColumnDescSqlFormat, tableEntity.TableName, col.ColumnName, col.ColumnDesc);
                    }
                }
                sbSqlText.Remove(sbSqlText.Length - 1, 1);
                sbSqlText.AppendLine();
            }
            sbSqlText.AppendLine(");");
            sbSqlText.Append(sbSqlDescText);
            return sbSqlText.ToString();
        }

        /// <summary>
        /// TrimTableName
        /// </summary>
        /// <returns></returns>
        public static string TrimTableName(this string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return "";
            }
            tableName = tableName.Trim();
            if (tableName.StartsWith("tab") || tableName.StartsWith("tbl"))
            {
                return tableName.Substring(3);
            }
            else if (tableName.StartsWith("tab_") || tableName.StartsWith("tbl_"))
            {
                tableName = tableName.Substring(4);
            }
            return tableName;
        }

        /// <summary>
        /// 将属性名称转换为私有字段名称
        /// </summary>
        /// <param name="propertyName"> 属性名称 </param>
        /// <returns> 私有字段名称 </returns>
        public static string ToPrivateFieldName(this string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return "";
            }
            if (propertyName.Equals(propertyName.ToUpperInvariant()))
            {
                return propertyName.ToLowerInvariant();
            }
            if (char.IsUpper(propertyName[0]))//首字母大写，首字母转换为小写
            {
                return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            }
            else
            {
                return "_" + propertyName;
            }
        }
    }
}
