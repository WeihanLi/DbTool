using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 列信息
    /// </summary>
    public class ColumnEntity
    {
        private string _columnDescription;
        private string _dataType;
        private string columnName;

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get => columnName; set => columnName = value?.Trim(); }

        /// <summary>
        /// 列描述
        /// </summary>
        public string ColumnDescription
        {
            get => _columnDescription.GetValueOrDefault(ColumnName);
            set => _columnDescription = !string.IsNullOrEmpty(value) ? value : string.Empty;
        }

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
        public uint Size { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get => _dataType;
            set => _dataType = value?.ToUpper();
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
    }
}
