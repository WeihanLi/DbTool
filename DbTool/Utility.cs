using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
        /// <param name="tableEntity">表信息</param>
        /// <param name="modelNamespace">model class 命名空间</param>
        /// <param name="prefix">model class前缀</param>
        /// <param name="suffix">model class后缀</param>
        /// <returns></returns>
        public static string GenerateModelText(this TableEntity tableEntity, string modelNamespace , string prefix , string suffix)
        {
            if (tableEntity == null)
            {
                throw new ArgumentNullException("tableEntity", "表信息不能为空");
            }
            StringBuilder sbText = new StringBuilder();
            sbText.AppendLine("using System;");
            sbText.AppendLine("namespace " + modelNamespace);
            sbText.AppendLine("{");
            if (!String.IsNullOrEmpty(tableEntity.TableDesc))
            {
                sbText.AppendLine("\t/// <summary>" + System.Environment.NewLine + "\t/// " + tableEntity.TableDesc + System.Environment.NewLine + "\t/// </summary>");
            }
            sbText.AppendLine("\tpublic class "+ prefix + tableEntity.TableName + suffix);
            sbText.AppendLine("\t{");
            foreach (var item in tableEntity.Columns)
            {
                item.DataType = SqlDbType2FclType(item.DataType, item.IsNullable); //转换为FCL数据类型
                string tmpColName = item.ColumnName.ToPrivateFieldName();
                sbText.AppendLine("\t\tprivate " + item.DataType + " " + tmpColName+";");
                if (!String.IsNullOrEmpty(item.ColumnDesc))
                {
                    sbText.AppendLine("\t\t/// <summary>" + System.Environment.NewLine + "\t\t/// " + item.ColumnDesc + System.Environment.NewLine + "\t\t/// </summary>");
                }
                sbText.AppendLine("\t\tpublic " + item.DataType + " " + item.ColumnName);
                sbText.AppendLine("\t\t{");
                sbText.AppendLine("\t\t\tget { return " + tmpColName + "; }");
                sbText.AppendLine("\t\t\tset { "+tmpColName+" = value; }");
                sbText.AppendLine("\t\t}");
                sbText.AppendLine();
            }
            sbText.AppendLine("\t}");
            sbText.AppendLine("}");
            return sbText.ToString();
        }

        /// <summary>
        /// 利用反射和泛型将 DataTable 转换为 List
        /// </summary>
        /// <param name="dt">DataTable 对象</param>
        /// <returns>List对象</returns>
        public static List<T> DataTableToList<T>(this DataTable dt) where T : class, new()
        {
            // 定义集合
            List<T> ts = new List<T>();
            // 获得此模型的类型
            Type type = typeof(T);
            //定义一个临时变量
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量
                                       //检查DataTable是否包含此列（列名==对象的属性名）
                    if (dt.Columns.Contains(tempName))
                    {
                        //取值
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性
                        if (value != DBNull.Value)
                            pi.SetValue(t , value , null);
                    }
                }
                //对象添加到泛型集合中
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 数据库数据类型转换为FCL类型
        /// </summary>
        /// <param name="dbType">数据库数据类型</param>
        /// <param name="isNullable">该数据列是否可以为空</param>
        /// <returns></returns>
        public static string SqlDbType2FclType(string dbType , bool isNullable = true)
        {
            SqlDbType sqlDbType = (SqlDbType) System.Enum.Parse(typeof(System.Data.SqlDbType), dbType, true);
            string type = null;
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                    type = isNullable ? "long?" : "long";
                    break;

                case SqlDbType.Bit:
                    type = isNullable ? "bool?" : "bool";
                    break;

                case SqlDbType.Float:
                case SqlDbType.Real:
                    type = isNullable ? "double?" : "double";
                    break;

                case SqlDbType.Binary:
                case SqlDbType.VarBinary:
                case SqlDbType.Image:
                    type = "byte[]";
                    break;

                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                case SqlDbType.Int:
                    type = isNullable ? "int?" : "int";
                    break;

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                case SqlDbType.Text:
                    type = "string";
                    break;

                case SqlDbType.Money:
                case SqlDbType.Decimal:
                case SqlDbType.SmallMoney:
                    type = isNullable ? "decimal?" : "decimal";
                    break;

                case SqlDbType.UniqueIdentifier:
                    type = isNullable ? "Guid?" : "Guid";
                    break;

                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.Timestamp:
                case SqlDbType.SmallDateTime:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                    type = isNullable ? "DateTime?" : "DateTime";
                    break;

                case SqlDbType.DateTimeOffset:
                    type = isNullable ? "TimeSpan?" : "TimeSpan";
                    break;

                case SqlDbType.Variant:
                case SqlDbType.Xml:
                case SqlDbType.Udt:
                case SqlDbType.Structured:
                default:
                    type = "object";
                    break;
            }
            return type;
        }

        /// <summary>
        /// 根据表信息生成sql语句
        /// </summary>
        /// <param name="tableEntity">表信息</param>
        /// <returns></returns>
        public static string GenerateSqlStatement(this TableEntity tableEntity)
        {
            if (String.IsNullOrEmpty(tableEntity.TableName))
            {
                return "";
            }
            StringBuilder sbSqlText = new StringBuilder(), sbSqlDescText = new StringBuilder();
            //create table
            sbSqlText.AppendFormat("CREATE TABLE {0}(", tableEntity.TableName);
            //create description
            if (!String.IsNullOrEmpty(tableEntity.TableDesc))
            {
                sbSqlDescText.AppendFormat("EXECUTE sp_addextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';", tableEntity.TableName, tableEntity.TableDesc);
            }
            if (tableEntity.Columns.Count > 0)
            {
                foreach (var col in tableEntity.Columns)
                {
                    sbSqlText.AppendLine();
                    sbSqlText.AppendFormat("{0} {1}", col.ColumnName, col.DataType);
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
                    if (col.DefaultValue != null && !String.IsNullOrEmpty(col.DefaultValue.ToString()))
                    {
                        if (col.IsPrimaryKey && col.DataType.ToUpper().Equals("INT"))
                        {
                            sbSqlText.Append(" IDENTITY(1,1) ");
                        }
                        else
                        {
                            if (col.DataType.ToUpperInvariant().Contains("CHAR"))
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
                    if (!String.IsNullOrEmpty(col.ColumnDesc))
                    {
                        sbSqlDescText.AppendLine();
                        sbSqlDescText.AppendFormat("EXECUTE sp_addextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}'; ", tableEntity.TableName, col.ColumnName, col.ColumnDesc);
                    }
                }
                sbSqlText.Remove(sbSqlText.Length - 1, 1);
                sbSqlText.AppendLine();
            }
            sbSqlText.AppendLine(");");
            sbSqlText.Append(sbSqlDescText.ToString());
            return sbSqlText.ToString();
        }

        /// <summary>
        /// TrimTableName
        /// </summary>
        /// <returns></returns>
        public static string TrimTableName(this string tableName)
        {
            if (String.IsNullOrEmpty(tableName))
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
        /// <param name="propertyName">属性名称</param>
        /// <returns>私有字段名称</returns>
        public static string ToPrivateFieldName(this string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
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