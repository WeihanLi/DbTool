using DbTool.Core;
using DbTool.DbProvider.MySql;
using DbTool.DbProvider.PostgreSql;
using DbTool.DbProvider.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DbTool.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbProvider<SqlServerDbProvider>();
            services.AddDbProvider<MySqlDbProvider>();
            services.AddDbProvider<PostgreSqlDbProvider>();

            services.AddSingleton<DbProviderFactory>();
            services.AddSingleton<IDbHelperFactory, MockDbHelperFactory>();
        }
    }
}
