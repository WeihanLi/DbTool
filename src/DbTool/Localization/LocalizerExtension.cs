using System;
using System.Windows.Markup;
using Microsoft.Extensions.Localization;
using WeihanLi.Common;

namespace DbTool.Localization
{
    public class LocalizerExtension : MarkupExtension
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        public string Key { get; }

        public LocalizerExtension(string key)
        {
            Key = key;
            _localizerFactory = DependencyResolver.
                ResolveService<IStringLocalizerFactory>();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //var type = serviceProvider.GetType();
            //var targetRootType = type.GetProperty("System.Xaml.IRootObjectProvider.RootObject", BindingFlags.Instance | BindingFlags.NonPublic)
            //    ?.GetValue(serviceProvider)
            //    ?.GetType();
            //if (null == targetRootType)
            //{
            //    targetRootType = typeof(MainWindow);
            //}

            var targetRootType = typeof(MainWindow);
            var localizer = _localizerFactory.Create(targetRootType);
            var value = localizer[Key];
            return (string)value;
        }
    }
}
