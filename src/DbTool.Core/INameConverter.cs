using System;

namespace DbTool.Core
{
    public interface IModelNameConverter
    {
        string ConvertTableToModel(string tableName);

        string ConvertModelToTable(string modelName);
    }

    public class DefaultModelNameConverter : IModelNameConverter
    {
        public string ConvertTableToModel(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return "";
            }
            tableName = tableName.Trim();
            if (tableName.StartsWith("tab_", StringComparison.OrdinalIgnoreCase)
                || tableName.StartsWith("tbl_", StringComparison.OrdinalIgnoreCase))
            {
                return tableName.Substring(4);
            }
            if (tableName.StartsWith("tab", StringComparison.OrdinalIgnoreCase)
                || tableName.StartsWith("tbl", StringComparison.OrdinalIgnoreCase))
            {
                return tableName.Substring(3);
            }
            return tableName;
        }

        public string ConvertModelToTable(string modelName)
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
    }
}
