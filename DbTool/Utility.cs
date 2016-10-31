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
        /// <param name="cols">列信息</param>
        /// <param name="modelNamespace">model class 命名空间</param>
        /// <param name="prefix">model class前缀</param>
        /// <param name="suffix">model class后缀</param>
        /// <returns></returns>
        public static string GenerateModelText(this IEnumerable<ColumnInfo> cols , string modelNamespace , string prefix , string suffix)
        {
            if (cols== null)
            {
                throw new ArgumentNullException("cols","model信息不能为空");
            }
            StringBuilder sbText = new StringBuilder();
            sbText.AppendLine("using System;");
            sbText.AppendLine("namespace " + modelNamespace);
            sbText.AppendLine("{");
            sbText.AppendLine("\tpublic class "+ prefix + cols.FirstOrDefault().ModelName + suffix);
            sbText.AppendLine("\t{");
            foreach (var item in cols)
            {
                string tmpColName = "_"+item.ColumeName;
                sbText.AppendLine("\t\tprivate " + item.DataType + " " + tmpColName+";");
                sbText.AppendLine("\t\tpublic " + item.DataType + " " + item.ColumeName);
                sbText.AppendLine("\t\t{");
                sbText.AppendLine("\t\t\tget { return " + tmpColName + "; }");
                sbText.AppendLine("\t\t\tset { "+tmpColName+"= value; }");
                sbText.AppendLine("\t\t}");
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
        /// <returns></returns>
        public static string SqlDbType2FclType(this SqlDbType dbType , bool isNullable = true)
        {
            string type = null;
            switch (dbType)
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
        /// TrimTableName
        /// </summary>
        /// <returns></returns>
        public static string TrimTableName(this string tableName)
        {
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
    }
}