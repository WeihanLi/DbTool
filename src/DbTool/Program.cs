using System;
using System.Windows.Forms;
using Autofac;
using DbTool.Core;
using DbTool.MySql;
using DbTool.SqlServer;
using WeihanLi.Common;

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
            builder.RegisterType<SqlServerDbProvider>().As<IDbProvider>();
            builder.RegisterType<MySqlDbProvider>().As<IDbProvider>();
            builder.RegisterType<DbProviderFactory>().SingleInstance();
            var container = builder.Build();
            DependencyResolver.SetDependencyResolver(t => container.Resolve(t));
        }
    }
}
