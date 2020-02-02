using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DbTool.Core;
using DbTool.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions.Localization.Json;

namespace DbTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Init();
            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient<MainWindow>();
            services.AddJsonLocalization(options => options.ResourcesPathType = ResourcesPathType.CultureBased);
        }

        private void Init()
        {
            #region Init Settings

            var settings = new SettingsViewModel
            {
                ExcelTemplateDownloadLink =
                    ConfigurationHelper.AppSetting(ConfigurationConstants.ExcelTemplateDownloadLink),
                DefaultConnectionString = ConfigurationHelper.AppSetting(ConfigurationConstants.DefaultConnectionString),
                DefaultDbType = ConfigurationHelper.AppSetting(ConfigurationConstants.DbType),
                GenerateDataAnnotation = ConfigurationHelper.AppSetting<bool>(ConfigurationConstants.GenerateDataAnnotation),
                GeneratePrivateField = ConfigurationHelper.AppSetting<bool>(ConfigurationConstants.GeneratePrivateField),
                GenerateDbDescription = ConfigurationHelper.AppSetting<bool>(ConfigurationConstants.GenerateDbDescription),
            };
            settings.DefaultCulture = ConfigurationHelper.AppSetting(nameof(settings.DefaultCulture));
            settings.SupportedCultures = ConfigurationHelper.AppSetting(nameof(settings.SupportedCultures))
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            settings.ConnectionString = settings.DefaultConnectionString;

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.DefaultCulture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.DefaultCulture);

            #endregion Init Settings

            #region Init Services and plugins

            var builder = new ContainerBuilder();
            builder.RegisterInstance(settings);

            builder.RegisterType<DbProviderFactory>().AsSelf().SingleInstance();
            var interfaces = typeof(IDbProvider).Assembly
                .GetExportedTypes()
                .Where(x => x.IsInterface)
                .ToArray();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Select(x => x.GetTypes())
                .SelectMany(t => t)
                .Where(t => !t.IsInterface && !t.IsAbstract && interfaces.Any(i => i.IsAssignableFrom(t)))
                .ToArray();
            foreach (var type in types)
            {
                builder.RegisterType(type).AsImplementedInterfaces();
            }

            var pluginDir = ApplicationHelper.MapPath("plugins");
            if (Directory.Exists(pluginDir))
            {
                // load plugins
                var plugins = Directory.GetFiles(pluginDir)
                    .Where(_ => _.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                if (plugins.Length > 0)
                {
                    var pluginTypes = plugins.Select(Assembly.LoadFrom)
                        .Select(x => x.GetTypes())
                        .SelectMany(t => t)
                        .Where(t => !t.IsInterface && !t.IsAbstract && interfaces.Any(i => i.IsAssignableFrom(t)))
                        .ToArray();
                    foreach (var type in pluginTypes)
                    {
                        builder.RegisterType(type).AsImplementedInterfaces();
                    }
                }
            }

            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            builder.Populate(services);

            var container = builder.Build();
            DependencyResolver.SetDependencyResolver(container.Resolve);

            #endregion Init Services and plugins

            container.Resolve<MainWindow>().Show();
        }
    }
}
