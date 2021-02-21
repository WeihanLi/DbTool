using System.Collections.Generic;
using System.Threading.Tasks;
using DbTool.Core;
using DbTool.Core.Entity;

namespace DbTool.Test
{
    internal class MockDbHelper : IDbHelper
    {
        public MockDbHelper(IDbProvider dbProvider)
        {
            DbType = dbProvider.DbType;
        }

        public string DbType { get; }
        public string DatabaseName => "Test";

        public Task<List<TableEntity>> GetTablesInfoAsync()
        {
            return Task.FromResult(new List<TableEntity>()
            {
                new()
                {
                    TableName = "tabUser111",
                    TableDescription = "测试用户表"
                }
            });
        }

        public Task<List<ColumnEntity>> GetColumnsInfoAsync(string tableName)
        {
            return Task.FromResult(new List<ColumnEntity>
            {
                new()
                {
                    ColumnName = "Id",
                    ColumnDescription = "主键",
                    IsPrimaryKey = true,
                    DataType = "int",
                    IsNullable = false,
                    Size = 4
                },
                new()
                {
                    ColumnName = "UserName",
                    ColumnDescription = "用户名",
                    IsPrimaryKey = false,
                    DataType = "VARCHAR",
                    IsNullable = false,
                    Size = 50
                },
                new()
                {
                    ColumnName = "NickName",
                    ColumnDescription = "昵称",
                    IsPrimaryKey = false,
                    DataType = "VARCHAR",
                    IsNullable = true,
                    Size = 50
                },
                new()
                {
                    ColumnName = "IsAdmin",
                    ColumnDescription = "是否是管理员",
                    IsPrimaryKey = false,
                    DataType = "bit",
                    IsNullable = true,
                    Size = 1,
                    DefaultValue = 0
                },
                new()
                {
                    ColumnName = "CreatedTime",
                    ColumnDescription = "创建时间",
                    IsPrimaryKey = false,
                    DataType = "DateTime",
                    IsNullable = false,
                    Size = 8
                }
            });
        }
    }

    internal class MockDbHelperFactory : IDbHelperFactory
    {
        public IDbHelper GetDbHelper(IDbProvider dbProvider, string connectionString)
        {
            return new MockDbHelper(dbProvider);
        }
    }
}
