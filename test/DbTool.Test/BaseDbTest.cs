using System.Collections.Generic;
using DbTool.Core;
using DbTool.Core.Entity;
using DbTool.DbProvider.MySql;
using DbTool.DbProvider.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;
using Xunit;

namespace DbTool.Test
{
    public abstract class BaseDbTest : IDbOperTest
    {
        public abstract string ConnStringKey { get; }
       
        protected readonly IDbProvider DbProvider;

        static BaseDbTest()
        {
            Init();
        }

        private static void Init()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IDbProvider, SqlServerDbProvider>();
            services.AddSingleton<IDbProvider, MySqlDbProvider>();
            services.AddSingleton<DbProviderFactory>();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddSingleton(configuration);

            DependencyResolver.SetDependencyResolver(services);
        }

        protected BaseDbTest()
        {
            var dbType = ConnStringKey.Substring(0, ConnStringKey.Length - 4);
            DbProvider = DependencyResolver.Current.GetRequiredService<DbProviderFactory>()
                            .GetDbProvider(dbType);
        }

        protected TableEntity TableEntity = new TableEntity()
        {
            TableName = "tabUser111",
            TableDescription = "测试用户表",
            Columns = new List<ColumnEntity>
            {
                new ColumnEntity
                {
                    ColumnName = "Id",
                    ColumnDescription = "主键",
                    IsPrimaryKey = true,
                    DataType = "int",
                    IsNullable = false,
                    Size = 4
                },
                new ColumnEntity
                {
                    ColumnName = "UserName",
                    ColumnDescription = "用户名",
                    IsPrimaryKey = false,
                    DataType = "VARCHAR",
                    IsNullable = false,
                    Size = 50
                },
                new ColumnEntity
                {
                    ColumnName = "NickName",
                    ColumnDescription = "昵称",
                    IsPrimaryKey = false,
                    DataType = "VARCHAR",
                    IsNullable = true,
                    Size = 50
                },
                new ColumnEntity
                {
                    ColumnName = "IsAdmin",
                    ColumnDescription = "是否是管理员",
                    IsPrimaryKey = false,
                    DataType = "bit",
                    IsNullable = true,
                    Size = 1,
                    DefaultValue = 0
                },
                new ColumnEntity
                {
                    ColumnName = "CreatedTime",
                    ColumnDescription = "创建时间",
                    IsPrimaryKey = false,
                    DataType = "DateTime",
                    IsNullable = false,
                    Size = 8
                }
            }
        };

        public virtual void QueryTest()
        {
            var connString = DependencyResolver.Current.GetService<IConfiguration>()
                .GetConnectionString(ConnStringKey);
            var dbHelper = new DbHelper(connString, DbProvider);
            Assert.NotNull(dbHelper.DatabaseName);
            var tables = dbHelper.GetTablesInfo();
            Assert.NotNull(tables);
            Assert.NotEmpty(tables);
            foreach (var table in tables)
            {
                var columns = dbHelper.GetColumnsInfo(table.TableName);
                Assert.NotNull(columns);
                Assert.NotEmpty(columns);
            }
        }

        public virtual void CreateTest()
        {
            var sql = DbProvider.GenerateSqlStatement(TableEntity);
            Assert.NotEmpty(sql);
            sql = DbProvider.GenerateSqlStatement(TableEntity, false);
            Assert.NotEmpty(sql);
        }
    }
}
