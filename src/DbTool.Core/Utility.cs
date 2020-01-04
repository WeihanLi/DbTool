using DbTool.Core.Entity;
using WeihanLi.Common;
using WeihanLi.Extensions;

namespace DbTool.Core
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 根据表信息生成sql语句
        /// </summary>
        /// <param name="tableEntity"> 表信息 </param>
        /// <param name="genDescription">生成描述信息</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string GenerateSqlStatement(this TableEntity tableEntity, bool genDescription = true, string dbType = "SqlServer")
        {
            if (string.IsNullOrEmpty(tableEntity?.TableName))
            {
                return "";
            }
            return DependencyResolver.Current.ResolveService<DbProviderFactory>().GetDbProvider(dbType)?
                .GenerateSqlStatement(tableEntity, genDescription);
        }

        /// <summary>
        /// TrimTableName
        /// TODO: 提供用户可以自定义的方式
        /// </summary>
        /// <returns></returns>
        public static string TrimTableName(this string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return "";
            }
            tableName = tableName.Trim();
            if (tableName.Substring(0, 4).EqualsIgnoreCase("tab_")
                || tableName.Substring(0, 4).EqualsIgnoreCase("tbl_"))
            {
                return tableName.Substring(4);
            }
            if (tableName.Substring(0, 3).EqualsIgnoreCase("tab")
                || tableName.Substring(0, 3).EqualsIgnoreCase("tbl"))
            {
                return tableName.Substring(3);
            }
            return tableName;
        }

        /// <summary>
        /// TrimModelName
        /// </summary>
        /// <param name="modelName">modelName</param>
        /// <returns></returns>
        public static string TrimModelName(this string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                return "";
            }
            modelName = modelName.Trim();
            if (modelName.EndsWith("Model"))
            {
                return modelName.Substring(0, modelName.Length - 5);
            }
            if (modelName.EndsWith("Entity"))
            {
                return modelName.Substring(0, modelName.Length - 6);
            }
            return modelName;
        }

        public static string GetDefaultFromDbDefaultValue(string dataType, object defaultValue)
        {
            if (null == defaultValue)
            {
                return null;
            }

            dataType = dataType.Trim().ToUpper();
            var str = defaultValue.ToString().ToUpper();
            if ((str.Contains("GETDATE") || str.Contains("NOW") || str.Contains("CURRENT_TIMESTAMP")) && (dataType.Contains("DATE") || dataType.Contains("TIME")))
            {
                return "DateTime.Now";
            }

            if (dataType.Equals("BIT"))
            {
                if (str.StartsWith("b'"))
                {
                    return str.Substring(2, 1).Equals("1") ? "true" : "false";
                }
                if (defaultValue is int)
                {
                    return defaultValue.To<int>() == 1 ? "true" : "false";
                }

                return defaultValue.ToString().ToLower();
            }

            return defaultValue.ToString();
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
