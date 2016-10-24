using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
        private const string QueryColumnsSql = @"SELECT  TABLE_NAME AS TableName,COLUMN_NAME AS ColumeName,IS_NULLABLE AS IsNullable,DATA_TYPE AS DataType  FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";

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
    }
}