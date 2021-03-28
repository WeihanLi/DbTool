using System;
using System.Windows.Markup;
using Microsoft.Extensions.Localization;
using WeihanLi.Common;

namespace DbTool.Localization
{
    public class LocalizerExtension : MarkupExtension
    {
        private readonly IStringLocalizer _localizer;
        public string Key { get; }

        public LocalizerExtension(string key)
        {
            Key = key;
            _localizer = DependencyResolver.
                ResolveService<IStringLocalizerFactory>()
                .Create(typeof(MainWindow));
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var value = _localizer[Key];
            return (string)value;
        }
    }
}
