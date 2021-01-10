using System.Collections.Generic;
using System.Threading.Tasks;
using DbTool.Core;
using DbTool.Core.Entity;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DbTool.Test
{
    public abstract class BaseDbTest
    {
        public abstract string ConnStringKey { get; }

        protected readonly IDbProvider DbProvider;

        public IConfiguration Configuration { get; }

        protected BaseDbTest(IConfiguration configuration, DbProviderFactory dbProviderFactory)
        {
            Configuration = configuration;

            var dbType = ConnStringKey.Substring(0, ConnStringKey.Length - 4);
            DbProvider = dbProviderFactory.GetDbProvider(dbType);
        }

        protected TableEntity TableEntity = new()
        {
            TableName = "tabUser111",
            TableDescription = "测试用户表",
            Columns = new List<ColumnEntity>
            {
                new ()
                {
                    ColumnName = "Id",
                    ColumnDescription = "主键",
                    IsPrimaryKey = true,
                    DataType = "int",
                    IsNullable = false,
                    Size = 4
                },
                new ()
                {
                    ColumnName = "UserName",
                    ColumnDescription = "用户名",
                    IsPrimaryKey = false,
                    DataType = "VARCHAR",
                    IsNullable = false,
                    Size = 50
                },
                new ()
                {
                    ColumnName = "NickName",
                    ColumnDescription = "昵称",
                    IsPrimaryKey = false,
                    DataType = "VARCHAR",
                    IsNullable = true,
                    Size = 50
                },
                new ()
                {
                    ColumnName = "IsAdmin",
                    ColumnDescription = "是否是管理员",
                    IsPrimaryKey = false,
                    DataType = "bit",
                    IsNullable = true,
                    Size = 1,
                    DefaultValue = 0
                },
                new ()
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

        public virtual async Task QueryTest()
        {
            var connString = Configuration.GetConnectionString(ConnStringKey);
            IDbHelper dbHelper = new DbHelper(connString, DbProvider);
            Assert.NotNull(dbHelper.DatabaseName);
            var tables = await dbHelper.GetTablesInfoAsync();
            Assert.NotNull(tables);
            Assert.NotEmpty(tables);
            foreach (var table in tables)
            {
                Assert.NotNull(table.TableName);
                var columns = await dbHelper.GetColumnsInfoAsync(table.TableName ?? string.Empty);
                Assert.NotNull(columns);
                Assert.NotEmpty(columns);
            }
        }

        public virtual void CreateTest()
        {
            var sql = DbProvider.GenerateSqlStatement(TableEntity);
            Assert.NotEmpty(sql);

            var sql1 = DbProvider.GenerateSqlStatement(TableEntity, false);
            Assert.NotEmpty(sql);

            Assert.Equal(sql1, sql);
        }
    }
}
