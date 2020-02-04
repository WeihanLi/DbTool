using System;
using System.Data.Common;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public interface IDbProvider
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        string DbType { get; }

        /// <summary>
        /// 查询数据库表 Sql
        /// 0:dbName
        /// </summary>
        string QueryDbTablesSqlFormat { get; }

        /// <summary>
        /// 查询数据库表中的列信息 sql
        /// 0:dbName
        /// 1:tableName
        /// </summary>
        string QueryTableColumnsSqlFormat { get; }

        /// <summary>
        /// 数据库类型转换成 clr 的类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="isNullable">是否可以为null</param>
        /// <returns></returns>
        string DbType2ClrType(string dbType, bool isNullable);

        /// <summary>
        /// C# 数据类型转换成数据库类型
        /// </summary>
        /// <param name="type">clr type</param>
        /// <returns></returns>
        string ClrType2DbType(Type type);

        /// <summary>
        /// 返回数据库类型默认长度，如 varchar
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="defaultLength"></param>
        /// <returns></returns>
        uint GetDefaultSizeForDbType(string dbType, uint defaultLength = 64);

        /// <summary>
        /// 根据连接字符串获取数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        DbConnection GetDbConnection(string connectionString);

        /// <summary>
        /// 根据表信息生成创建表Sql
        /// </summary>
        /// <param name="tableEntity">tableEntity</param>
        /// <param name="generateDescription">generateDescription</param>
        /// <returns>sql text</returns>
        string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true);
    }
}
