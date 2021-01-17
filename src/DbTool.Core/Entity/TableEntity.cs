using System;
using System.Collections.Generic;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 表信息
    /// </summary>
    public class TableEntity
    {
        private readonly string _tableName = null!;
        private List<ColumnEntity> _columns = new();

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get => _tableName;
            init => _tableName = (value ?? throw new ArgumentNullException(nameof(value))).Trim();
        }

        /// <summary>
        /// 表描述
        /// </summary>
        public string? TableDescription { get; set; }

        /// <summary>
        /// 获取描述信息，如果描述信息为空则返回列名
        /// </summary>
        public string NotEmptyDescription => TableDescription.GetValueOrDefault(TableName);

        /// <summary>
        /// 表架构 scheme
        /// </summary>
        public string? TableSchema { get; set; }

        /// <summary>
        /// 列信息
        /// </summary>
#nullable disable

        public List<ColumnEntity> Columns
        {
            get => _columns;
            set => _columns = value ?? new();
        }

#nullable restore
    }
}
