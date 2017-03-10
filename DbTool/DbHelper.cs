using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
        private const string QueryDbTablesSql = @"SELECT TABLE_NAME AS TableName,TABLE_CATALOG AS DatabaseName,TABLE_TYPE AS TableType,TABLE_SCHEMA AS TableScheme FROM INFORMATION_SCHEMA.TABLES;";

        /// <summary>
        /// 查询数据库表字段信息SQL
        /// </summary>
        private const string QueryColumnsSql = @"SELECT  t.[name] AS TableName ,
                                                                                        c.[name] AS ColumnName ,
                                                                                        p.[value] AS ColumnDesc ,
                                                                                        c.[is_nullable] AS IsNullable ,
                                                                                        ty.[name] AS DataType
                                                                                FROM    sys.columns c
                                                                                        JOIN sys.tables t ON c.object_id = t.object_id
                                                                                        JOIN sys.[types] ty ON ty.[system_type_id] = c.[system_type_id]
                                                                                                               AND ty.[name] != 'sysname'
                                                                                        LEFT JOIN sys.extended_properties p ON p.minor_id = c.column_id
                                                                                                                               AND p.major_id = c.object_id
                                                                                WHERE   t.name = @tableName
                                                                                ORDER BY c.[column_id];";

        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string ConnString;

        private SqlConnection conn = null;

        public DbHelper(string connString)
        {
            ConnString = connString;
        }

        /// <summary>
        /// 获取一个数据库连接
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetSqlConnection()
        {
            if (conn == null)
            {
                conn = new SqlConnection(ConnString);
            }
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.ConnectionString = ConnString;
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// 获取一个SqlCommand对象
        /// </summary>
        /// <param name="cmdText">sql语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns></returns>
        private SqlCommand GetSqlCommand(string cmdText , params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.Parameters.AddRange(parameters);
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        /// <summary>
        /// 获取数据库表信息
        /// </summary>
        /// <returns></returns>
        public List<TableInfo> GetTablesInfo()
        {
            using (var connection = GetSqlConnection())
            {
                SqlCommand cmd = GetSqlCommand(QueryDbTablesSql);
                cmd.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt.DataTableToList<TableInfo>();
            }
        }

        /// <summary>
        /// 获取数据库表的列信息
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnsInfo(string tableName)
        {
            if (String.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            using (var connection = GetSqlConnection())
            {
                SqlCommand cmd = GetSqlCommand(QueryColumnsSql , new SqlParameter("@tableName" , tableName));
                cmd.Connection = connection;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt.DataTableToList<ColumnInfo>();
            }
        }

        /// <summary>
        /// 执行sql语句并返回受影响的行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            if (String.IsNullOrEmpty(sql))
            {
                return -1;
            }
            using (var connection = GetSqlConnection())
            {
                SqlCommand cmd = GetSqlCommand(sql);
                return cmd.ExecuteNonQuery();
            }
        }


    }
}