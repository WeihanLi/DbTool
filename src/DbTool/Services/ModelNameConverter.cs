// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core;
using System;
using System.Linq;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

// ReSharper disable once CheckNamespace
namespace DbTool;

public sealed class ModelNameConverter : DefaultModelNameConverter
{
    public override string ConvertTableToModel(string tableName)
    {
        var modelName = tableName?.Trim();
        if (modelName.IsNullOrEmpty())
            return string.Empty;

        if (modelName.StartsWith("tab_", StringComparison.OrdinalIgnoreCase)
            || modelName.StartsWith("tbl_", StringComparison.OrdinalIgnoreCase))
            modelName = modelName[4..];

        if (modelName.StartsWith("tab", StringComparison.OrdinalIgnoreCase)
            || modelName.StartsWith("tbl", StringComparison.OrdinalIgnoreCase))
            modelName = modelName[3..];

        modelName = modelName.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(StringHelper.ToPascalCase)
            .StringJoin("");

        return modelName;
    }
}
