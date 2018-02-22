using System.Collections.Generic;
using WeihanLi.Common.Helpers;
using Xunit;

namespace DbTool.Test
{
    public abstract class BaseDbTest : IDbOperTest
    {
        public abstract string ConnStringKey { get; }

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
            var dbHelper = new DbHelper(ConfigurationHelper.ConnectionString(ConnStringKey), ConnStringKey.Contains("SqlServer"));
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
            var sql = TableEntity.GenerateSqlStatement(isSqlServer: ConnStringKey.Contains("SqlServer"));
            Assert.NotEmpty(sql);
            sql = TableEntity.GenerateSqlStatement(false, ConnStringKey.Contains("SqlServer"));
            Assert.NotEmpty(sql);
        }
    }
}
