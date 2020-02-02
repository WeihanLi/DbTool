using System;
using System.Windows.Markup;
using Microsoft.Extensions.Localization;
using WeihanLi.Common;

namespace DbTool.Localization
{
    public class LocalizerExtension : MarkupExtension
    {
        private readonly IStringLocalizer<MainWindow> _localizer;
        public string Key { get; set; }

        public LocalizerExtension()
        {
            _localizer = DependencyResolver.Current.ResolveService<IStringLocalizer<MainWindow>>();
        }

        public LocalizerExtension(string key)
        {
            Key = key;
            _localizer = DependencyResolver.Current.ResolveService<IStringLocalizer<MainWindow>>();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var value = _localizer[Key];
            return (string)value;
        }
    }
}
