using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using DbTool.Core;
using DbTool.DbProvider.MySql;
using DbTool.DbProvider.SqlServer;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;

namespace DbTool
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SqlServerDbProvider>()
                .As<IDbProvider>()
                .SingleInstance();
            builder.RegisterType<MySqlDbProvider>()
                .As<IDbProvider>()
                .SingleInstance();
            builder.RegisterType<DbProviderFactory>()
                .SingleInstance();
            builder.RegisterType<DefaultModelCodeGenerator>()
                .As<IModelCodeGenerator>()
                .SingleInstance();
            builder.RegisterType<ExcelDbDocExporter>()
                .As<IDbDocExporter>()
                .SingleInstance();

            var pluginDir = ApplicationHelper.MapPath("plugins");
            if (Directory.Exists(pluginDir))
            {
                // load plugins
                var plugins = Directory.GetFiles(pluginDir)
                    .Where(_ => _.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                if (plugins.Length > 0)
                {
                    var assemblies = plugins.Select(Assembly.LoadFrom).ToArray();
                    builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces().SingleInstance();
                }
            }

            var container = builder.Build();
            DependencyResolver.SetDependencyResolver(t => container.Resolve(t));
        }
    }
}
