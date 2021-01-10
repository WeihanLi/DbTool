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
        public string? TableName { get => _tableName; set => _tableName = value?.Trim(); }

        private string? _tableName;

        /// <summary>
        /// 表描述
        /// </summary>
        public string? TableDescription { get; set; }

        /// <summary>
        /// 获取描述信息，如果描述信息为空则返回列名
        /// </summary>
        public string? NotEmptyDescription => TableDescription.GetValueOrDefault(_tableName);

        /// <summary>
        /// 表架构 scheme
        /// </summary>
        public string? TableSchema { get; set; }

        /// <summary>
        /// 列信息
        /// </summary>
        public List<ColumnEntity> Columns { get; set; }

        public TableEntity()
        {
            Columns = new List<ColumnEntity>();
        }
    }
}
