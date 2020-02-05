using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableEntity
    {

        /// <summary>
        /// 表名称
        /// </summary>
        [Required]
        public string TableName { get => tableName; set => tableName = value?.Trim(); }

        private string tableDescription;
        private string tableName;
        private List<ColumnEntity> columns;

        /// <summary>
        /// 表描述
        /// </summary>
        public string TableDescription { get => tableDescription.GetValueOrDefault(TableName); set => tableDescription = value; }

        /// <summary>
        /// 表架构 scheme
        /// </summary>
        public string TableSchema { get; set; }

        /// <summary>
        /// 列信息
        /// </summary>
        public List<ColumnEntity> Columns
        {
            get => columns; 
            set
            {
                if(value != null)
                {
                    columns = value;
                }
            }
        }

        public TableEntity()
        {
            Columns = new List<ColumnEntity>();
        }
    }
}
