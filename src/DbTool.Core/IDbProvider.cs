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
        /// 0:DbName
        /// </summary>
        string QueryDbTablesSqlFormat { get; }

        /// <summary>
        /// 查询数据库表中的列信息 sql
        /// 0:DbName
        /// 1:TableName
        /// </summary>
        string QueryTableColumnsSqlFormat { get; }

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
