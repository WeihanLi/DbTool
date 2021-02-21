using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DbTool.Core.Entity;
using WeihanLi.Common;
using WeihanLi.Extensions;

namespace DbTool.Core
{
    /// <summary>
    /// 数据库操作查询帮助类
    /// </summary>
    internal sealed class DbHelper : IDbHelper, IDisposable
    {
        private readonly DbConnection _conn;

        private readonly IDbProvider _dbProvider;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName => _conn.Database;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType => _dbProvider.DbType;

        public DbHelper(IDbProvider dbProvider, string connString)
        {
            if (connString.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(connString));
            }

            _dbProvider = dbProvider ?? throw new ArgumentNullException(nameof(dbProvider));
            _conn = _dbProvider.GetDbConnection(connString);
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TableEntity>> GetTablesInfoAsync()
        {
            return (await _conn.QueryAsync<TableEntity>(
                    _dbProvider.QueryDbTablesSqlFormat,
                    new { dbName = DatabaseName }))
                .ToList();
        }

        /// <summary>
        /// 获取数据库表的列信息
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public async Task<List<ColumnEntity>> GetColumnsInfoAsync(string tableName)
        {
            Guard.NotNullOrEmpty(tableName, nameof(tableName));
            return (await _conn.QueryAsync<ColumnEntity>(
                    _dbProvider.QueryTableColumnsSqlFormat,
                    new { dbName = DatabaseName, tableName }))
                .ToList();
        }

        private bool _disposed;

        public void Dispose()
        {
            if (!_disposed)
            {
                _conn.Dispose();
                _disposed = true;
            }
        }
    }
}
