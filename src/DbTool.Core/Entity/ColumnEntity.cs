using System;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 列信息
    /// </summary>
    public record ColumnEntity
    {
        private readonly string _dataType = null!;
        private readonly string _columnName = null!;

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName
        {
            get => _columnName;
            init => _columnName = (value ?? throw new ArgumentNullException(nameof(value))).Trim();
        }

        /// <summary>
        /// 列描述
        /// </summary>
        public string? ColumnDescription { get; set; }

        /// <summary>
        /// 获取描述信息，如果描述信息为空则返回列名
        /// </summary>
        public string NotEmptyDescription => ColumnDescription.GetValueOrDefault(ColumnName);

        /// <summary>
        /// 是否可以为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 是否是主键列
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get => _dataType;
            init => _dataType = (value ?? throw new ArgumentNullException(nameof(value))).ToUpper();
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public object? DefaultValue { get; set; }
    }
}
