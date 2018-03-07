using System;
using System.Windows.Forms;
using Autofac;
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
            var container = new ContainerBuilder();
            container.RegisterType<SqlServerDbProvider>().As<IDbProvider>();
            container.RegisterType<MySqlDbProvider>().As<IDbProvider>();
            container.RegisterType<DbProviderFactory>().SingleInstance();
            DependencyResolver.SetDependencyResolver(new AutofacDependencyResolver(container.Build()));
        }
    }
}
