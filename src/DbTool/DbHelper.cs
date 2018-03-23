using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbTool.Properties;
using WeihanLi.Common;
using WeihanLi.Extensions;

namespace DbTool
{
    /// <summary>
    /// 数据库操作查询帮助类
    /// </summary>
    public class DbHelper : IDisposable
    {
        private readonly DbConnection _conn;

        private readonly IDbProvider _dbProvider;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName => _conn.Database;

        public DbHelper(string connString, string dbType)
        {
            if (connString.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(connString));
            }

            var dbProviderFactory = DependencyResolver.Current.GetService<DbProviderFactory>();
            if (!dbProviderFactory.AllowedDbTypes.Any(_ => _.EqualsIgnoreCase(dbType)))
            {
                throw new ArgumentException(Resources.UnsupportedDbType.FormatWith(dbType), nameof(dbType));
            }
            _dbProvider = dbProviderFactory.GetDbProvider(dbType);
            _conn = _dbProvider.GetDbConnection(connString);
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <returns></returns>
        public List<TableEntity> GetTablesInfo()
        {
            return _conn.Select<TableEntity>(_dbProvider.QueryDbTablesSqlFormat, new { dbName = DatabaseName })
                .Chain(t => _conn.Close());
        }

        /// <summary>
        /// 获取数据库表的列信息
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public List<ColumnEntity> GetColumnsInfo(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }
            return _conn.Select<ColumnEntity>(
                    _dbProvider.QueryTableColumnsSqlFormat,
                new { dbName = DatabaseName, tableName })
                .Chain(t => _conn.Close());
        }

        #region IDisposable Support

        private bool _disposed; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管状态(托管对象)。
                }
                _conn?.Dispose();
                _disposed = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DbHelper() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}
