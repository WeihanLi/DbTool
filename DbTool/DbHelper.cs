using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        /// 查询数据库表字段信息SQL
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
        /// 表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        public static string AddTableDescSqlFormat => @"
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
        /// 列描述信息
        /// 0：表名称，1：列名称，2：列描述信息
        /// </summary>
        public static string AddColumnDescSqlFormat => @"
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

        private readonly string _connString;

        private SqlConnection _conn;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName => _conn.Database;

        public DbHelper(string connString)
        {
            _connString = connString;
            InitSqlConnection();
        }

        private void InitSqlConnection()
        {
            if (null == _conn || _conn.ConnectionString.IsNullOrEmpty())
            {
                _conn = new SqlConnection(_connString);
            }
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <returns></returns>
        public List<TableEntity> GetTablesInfo()
        {
            return _conn.Select<TableEntity>(QueryDbTablesSql).Chain(t => _conn.Close());
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
            return _conn.Select<ColumnEntity>(QueryColumnsSql, new { tableName }).Chain(t => _conn.Close());
        }
    }
}
