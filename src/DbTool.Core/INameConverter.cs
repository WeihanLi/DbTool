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
        public virtual string ConvertTableToModel(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
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

        public virtual string ConvertModelToTable(string modelName)
        {
            if (string.IsNullOrWhiteSpace(modelName))
            {
                return string.Empty;
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
