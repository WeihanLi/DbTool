using System;
using System.Collections.Generic;

namespace DbTool
{
    /// <summary>
    /// 数据库信息 
    /// </summary>
    public class DbEntity
    {
        private string databaseName;

        /// <summary>
        /// 数据库名称 
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return databaseName;
            }

            set
            {
                databaseName = value;
            }
        }

        private string databaseDesc;

        /// <summary>
        /// 数据库描述 
        /// </summary>
        public string DatabaseDesc
        {
            get
            {
                return databaseDesc;
            }

            set
            {
                databaseDesc = value;
            }
        }

        /// <summary>
        /// 表信息 
        /// </summary>
        public List<TableEntity> Tables { get; set; }
    }

    /// <summary>
    /// 表信息 
    /// </summary>
    public class TableEntity
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

        private string tableDesc;

        /// <summary>
        /// 表描述 
        /// </summary>
        public string TableDesc
        {
            get
            {
                return tableDesc;
            }

            set
            {
                tableDesc = value;
            }
        }

        private string tableSchema = "dbo";

        /// <summary>
        /// 表架构 scheme 
        /// </summary>
        public string TableSchema
        {
            get { return tableSchema; }
            set { tableSchema = value; }
        }

        /// <summary>
        /// 列信息 
        /// </summary>
        public List<ColumnEntity> Columns { get; set; }
    }

    /// <summary>
    /// 列信息 
    /// </summary>
    public class ColumnEntity
    {
        private string columnName;

        /// <summary>
        /// 列名称 
        /// </summary>
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private string columnDesc;

        /// <summary>
        /// 列描述 
        /// </summary>
        public string ColumnDesc
        {
            get
            {
                return columnDesc;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    columnDesc = value;
                }
                else
                {
                    columnDesc = "";
                }
            }
        }

        private bool isNullable;

        /// <summary>
        /// 是否可以为空 
        /// </summary>
        public bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }

        private bool isPrimaryKey;

        /// <summary>
        /// 是否是主键列 
        /// </summary>
        public bool IsPrimaryKey
        {
            get
            {
                return isPrimaryKey;
            }

            set
            {
                isPrimaryKey = value;
            }
        }

        private int size;

        /// <summary>
        /// 字段长度 
        /// </summary>
        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
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
                dataType =value;
            }
        }

        private object defaultValue;

        /// <summary>
        /// 默认值 
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return defaultValue;
            }

            set
            {
                defaultValue = value;
            }
        }
    }
}