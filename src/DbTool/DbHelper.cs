using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using WeihanLi.Extensions;

namespace DbTool
{
    /// <summary>
    /// 数据库操作查询帮助类
    /// </summary>
    public class DbHelper
    {
        /// <summary>
        /// 查询数据库表SQL
        /// </summary>
        private const string QueryDbTablesSql = @"SELECT  IT.TABLE_NAME AS TableName ,
                                                IT.TABLE_CATALOG AS DatabaseName ,
                                                IT.TABLE_TYPE AS TableType ,
                                                IT.TABLE_SCHEMA AS TableScheme ,
                                                [EP].[value] AS TableDescription
                                        FROM    INFORMATION_SCHEMA.TABLES AS IT
                                                LEFT JOIN sys.extended_properties AS EP ON EP.major_id = OBJECT_ID([IT].[TABLE_NAME])
                                                                                           AND [EP].[minor_id] = 0;";

        /// <summary>
        /// 查询MySql数据库中的表
        /// @dbName 数据库名称
        /// </summary>
        private const string QueryDbTablesSqlForMysql = @"SELECT
	@dbName AS DatabaseName,
    `ENGINE` AS TableSchema,
	TABLE_NAME AS TableName,
	TABLE_COMMENT AS TableDescription
FROM
	information_schema.`TABLES`
WHERE
	table_schema = @dbName";

        /// <summary>
        /// 查询数据库表字段信息SQL
        /// @tableName 数据库表名称
        /// </summary>
        private const string QueryColumnsSql = @"SELECT  t.[name] AS TableName ,
                                                                                        c.[name] AS ColumnName ,
                                                                                        p.[value] AS ColumnDescription ,
                                                                                        c.[is_nullable] AS IsNullable ,
                                                                                        ty.[name] AS DataType ,
                                                                                        c.[max_length] AS Size ,
                                                                                        c.[is_identity] AS IsPrimaryKey ,
                                                                                        SUBSTRING(dc.[definition], 2, LEN([dc].[definition]) - 2) AS DefaultValue
                                                                                FROM    sys.columns c
                                                                                        JOIN sys.tables t ON c.object_id = t.object_id
                                                                                        JOIN sys.[types] ty ON ty.[system_type_id] = c.[system_type_id]
                                                                                                               AND ty.[name] != 'sysname'
                                                                                        LEFT JOIN sys.extended_properties p ON p.minor_id = c.column_id
                                                                                                                               AND p.major_id = c.object_id
                                                                                        LEFT JOIN [sys].[default_constraints] dc ON dc.[parent_object_id] = c.[object_id]
                                                                                                                                    AND dc.[parent_column_id] = c.[column_id]
                                                                                                                                    AND dc.[type] = 'D'
                                                                                WHERE   t.name = @tableName
                                                                                ORDER BY c.[column_id];";

        /// <summary>
        /// 查询MySql列信息
        /// @dbName 数据库名称
        /// @tableName 表名称
        /// </summary>
        private const string QueryColumnsSqlForMySql = @"SELECT
	TABLE_NAME AS TableName,
	COLUMN_NAME AS ColumnName,
	COLUMN_Comment AS ColumnDescription,
	IF(IS_NULLABLE='YES','True','False') AS IsNullable,
	DATA_TYPE AS DataType,
	CHARACTER_MAXIMUM_LENGTH AS Size,
	IF(COLUMN_KEY != '','True','False') AS IsPrimaryKey,
	COLUMN_DEFAULT AS DefaultValue
FROM
	information_schema.`COLUMNS`
WHERE
	table_schema = @dbName
AND table_name = @tableName;";

        /// <summary>
        /// 创建或更新表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        public static string CreateOrUpdateTableDescSqlFormat => @"
BEGIN
IF EXISTS (
       SELECT 1
    FROM sys.extended_properties p,
         sys.tables t,
         sys.schemas s
    WHERE t.schema_id = s.schema_id
          AND p.major_id = t.object_id
          AND p.minor_id = 0
          AND p.name = N'MS_Description'
          AND s.name = N'dbo'
          AND t.name = N'{0}'
    )
        EXECUTE sp_updateextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';
ELSE
        EXECUTE sp_addextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';
END";

        /// <summary>
        /// 创建表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        public static string CreateTableDescSqlFormat => @"EXECUTE sp_addextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';";

        /// <summary>
        /// 列描述信息
        /// 0：表名称，1：列名称，2：列描述信息
        /// </summary>
        public static string CreateOrUpdateColumnDescSqlFormat => @"
BEGIN
IF EXISTS (
        select 1
        from
            sys.extended_properties p,
            sys.columns c,
            sys.tables t,
            sys.schemas s
        where
            t.schema_id = s.schema_id and
            c.object_id = t.object_id and
            p.major_id = t.object_id and
            p.minor_id = c.column_id and
            p.name = N'MS_Description' and
            s.name = N'dbo' and
            t.name = N'{0}' and
            c.name = N'{1}'
    )
        EXECUTE sp_updateextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}';
ELSE
        EXECUTE sp_addextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}';
END";

        /// <summary>
        /// 列描述信息
        /// 0：表名称，1：列名称，2：列描述信息
        /// </summary>
        public static string CreateColumnDescSqlFormat => @"EXECUTE sp_addextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}';";

        private readonly string _connString;
        private readonly bool _isSqlServer;

        private DbConnection _conn;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName => _conn.Database;

        public DbHelper(string connString, bool isSqlServer = true)
        {
            _connString = connString;
            _isSqlServer = isSqlServer;
            InitSqlConnection();
        }

        private void InitSqlConnection()
        {
            if (null == _conn || _conn.ConnectionString.IsNullOrEmpty())
            {
                _conn = _isSqlServer ? (DbConnection)new SqlConnection(_connString) : new MySqlConnection(_connString);
            }
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <returns></returns>
        public List<TableEntity> GetTablesInfo()
        {
            return _conn.Select<TableEntity>(_isSqlServer ? QueryDbTablesSql : QueryDbTablesSqlForMysql, new { dbName = DatabaseName }).Chain(t => _conn.Close());
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
                   _isSqlServer ? QueryColumnsSql : QueryColumnsSqlForMySql,
                new { dbName = DatabaseName, tableName })
                .Chain(t => _conn.Close());
        }
    }
}
