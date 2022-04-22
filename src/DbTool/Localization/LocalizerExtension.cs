// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Localization;
using System;
using System.Windows.Markup;
using WeihanLi.Common;

namespace DbTool.Localization;

public class LocalizerExtension : MarkupExtension
{
    private readonly IStringLocalizer _localizer;
    public string Key { get; }

    public LocalizerExtension(string key)
    {
        Key = key;
        _localizer = DependencyResolver.
            ResolveRequiredService<IStringLocalizerFactory>()
            .Create(typeof(MainWindow));
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var value = _localizer[Key];
        return (string)value;
    }
}
