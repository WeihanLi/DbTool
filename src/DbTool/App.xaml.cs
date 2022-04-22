// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core;
using DbTool.DbProvider.MySql;
using DbTool.DbProvider.PostgreSql;
using DbTool.DbProvider.SqlServer;
using DbTool.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;
using System.Windows;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions.Localization.Json;
using WeihanLi.Npoi;

namespace DbTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override void OnStartup(StartupEventArgs e)
    {
        Init();
        base.OnStartup(e);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.TryAddTransient<MainWindow>();
        services.AddJsonLocalization(options => options.ResourcesPathType = ResourcesPathType.CultureBased);

        services.TryAddSingleton<IModelNameConverter, ModelNameConverter>();
        services.TryAddSingleton<IModelCodeGenerator, DefaultCSharpModelCodeGenerator>();
        services.TryAddSingleton<IModelCodeExtractor, DefaultCSharpModelCodeExtractor>();
        services.TryAddSingleton<IDbHelperFactory, DbHelperFactory>();
        services.TryAddSingleton<DbProviderFactory>();

        services
            .AddDbProvider<SqlServerDbProvider>()
            .AddDbProvider<MySqlDbProvider>()
            .AddDbProvider<PostgreSqlDbProvider>()
            ;

        services.AddDbDocExporter<ExcelDbDocExporter>()
            .AddDbDocExporter<CsvDbDocExporter>()
            ;
        services.AddDbDocImporter<ExcelDbDocImporter>()
            .AddDbDocImporter<CsvDbDocImporter>()
            ;
    }

    private static void Init()
    {
        #region Init Settings

        FluentSettings.LoadMappingProfiles(typeof(ColumnEntityMappingProfile).Assembly);

        var settings = new SettingsViewModel();
        settings.ConnectionString = settings.DefaultConnectionString;
        // set current culture
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.DefaultCulture);
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.DefaultCulture);

        #endregion Init Settings

        #region Init Services and plugins

        IServiceCollection services = new ServiceCollection();
        services.AddSingleton(settings);
        ConfigureServices(services);

        // load plugins
        var interfaces = typeof(IDbProvider).Assembly
            .GetExportedTypes()
            .Where(x => x.IsInterface)
            .ToArray();
        var pluginDir = ApplicationHelper.MapPath("plugins");
        if (Directory.Exists(pluginDir))
        {
            // load plugins
            var plugins = Directory.GetFiles(pluginDir)
                .Where(_ => _.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (plugins.Length > 0)
            {
                var assemblies = plugins.Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).ToArray();
                var exportedTypes = assemblies
                    .Select(x => x.GetExportedTypes())
                    .SelectMany(t => t)
                    .Where(t => !t.IsInterface && !t.IsAbstract)
                    .ToArray();
                var pluginTypes = exportedTypes
                    .Where(t => interfaces.Any(i => i.IsAssignableFrom(t)))
                    .ToArray();
                foreach (var type in pluginTypes)
                {
                    services.RegisterTypeAsImplementedInterfaces(type);
                }

                // load service modules
                services.RegisterAssemblyModules(assemblies);
            }
        }

        DependencyResolver.SetDependencyResolver(services);

        #endregion Init Services and plugins

        DependencyResolver.ResolveRequiredService<MainWindow>().Show();
    }
}
