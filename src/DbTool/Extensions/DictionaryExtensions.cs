// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;
using WeihanLi.Extensions;

// ReSharper disable once CheckNamespace
namespace DbTool;

public static class DictionaryExtensions
{
    public static string ToFileChooseFilter(this Dictionary<string, string> supportedFileExtensions)
    {
        return supportedFileExtensions.Select(x => $"{x.Value}|*{x.Key}")
            .StringJoin("|");
    }
}
