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
        /// TrimTableName
        /// </summary>
        /// <returns>normalized model name</returns>
        public static string TrimTableName(this string tableName)
        {
            return DependencyResolver.Current.ResolveService<IModelNameConverter>()
                .ConvertTableToModel(tableName);
        }

        /// <summary>
        /// TrimModelName
        /// </summary>
        /// <param name="modelName">modelName</param>
        /// <returns>normalized table name</returns>
        public static string TrimModelName(this string modelName)
        {
            return DependencyResolver.Current.ResolveService<IModelNameConverter>()
                .ConvertModelToTable(modelName);
        }

        public static string GetDefaultFromDbDefaultValue(string dataType, object defaultValue)
        {
            if (null == defaultValue)
            {
                return null;
            }

            dataType = dataType.Trim().ToUpper();
            var str = defaultValue.ToString().ToUpper();
            if (dataType.Contains("DATE") || dataType.Contains("TIME"))
            {
                if (str.Contains("GETDATE") || str.Contains("NOW") || str.Contains("CURRENT_TIMESTAMP"))
                {
                    return "DateTime.Now";
                }
                if (str.Contains("GETUTCDATE") || str.Contains("UTCNOW"))
                {
                    return "DateTime.UtcNow";
                }
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
            if (char.IsUpper(propertyName[0]))
            {
                return char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
            }

            return "_" + propertyName;
        }
    }
}
