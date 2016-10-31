namespace DbTool
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnInfo
    {
        private string tableName;

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set
            {
                if (value.StartsWith("tab") || value.StartsWith("tab"))
                {
                    modelName = value.Substring(3);
                }
                else if (value.StartsWith("tab_") || value.StartsWith("tbl_"))
                {
                    modelName = value.Substring(4);
                }
                else
                {
                    modelName = value;
                }
                tableName = value;
            }
        }

        private string modelName;

        /// <summary>
        /// Model名称
        /// </summary>
        public string ModelName
        {
            get
            {
                return modelName;
            }
        }

        private string columnName;

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumeName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private string isNullable;

        /// <summary>
        /// 是否可以为空
        /// </summary>
        public string IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        private string dataType;

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                System.Data.SqlDbType dbType = (System.Data.SqlDbType) System.Enum.Parse(typeof(System.Data.SqlDbType) , value , true);
                dataType = dbType.SqlDbType2FclType(isNullable.ToUpperInvariant().Equals("YES"));
            }
        }
    }
}