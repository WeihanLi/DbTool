using System;
using System.Linq;
using DbTool.Core;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

// ReSharper disable once CheckNamespace
namespace DbTool
{
    public class ModelNameConverter : DefaultModelNameConverter
    {
        public override string ConvertTableToModel(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return string.Empty;

            var modelName = tableName.Trim();

            if (modelName.StartsWith("tab_", StringComparison.OrdinalIgnoreCase)
                || modelName.StartsWith("tbl_", StringComparison.OrdinalIgnoreCase))
                modelName = modelName.Substring(4);

            if (modelName.StartsWith("tab", StringComparison.OrdinalIgnoreCase)
                || modelName.StartsWith("tbl", StringComparison.OrdinalIgnoreCase))
                modelName = modelName.Substring(3);

            modelName = modelName.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(StringHelper.ToPascalCase)
                .StringJoin("");

            return modelName;
        }
    }
}
