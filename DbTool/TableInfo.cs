namespace DbTool
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableInfo
    {
        private string tableName;

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private string databaseName;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        private string tableType;

        /// <summary>
        /// 表类型
        /// </summary>
        public string TableType
        {
            get { return tableType; }
            set { tableType = value; }
        }

        private string tableSchema;

        /// <summary>
        /// 表架构 scheme
        /// </summary>
        public string TableSchema
        {
            get { return tableSchema; }
            set { tableSchema = value; }
        }
    }
}