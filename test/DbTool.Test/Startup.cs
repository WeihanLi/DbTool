using DbTool.Core;
using DbTool.DbProvider.MySql;
using DbTool.DbProvider.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DbTool.Test
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDbProvider, SqlServerDbProvider>();
            services.AddSingleton<IDbProvider, MySqlDbProvider>();
            services.AddSingleton<DbProviderFactory>();
            services.AddSingleton<IDbHelperFactory, MockDbHelperFactory>();
        }
    }
}
