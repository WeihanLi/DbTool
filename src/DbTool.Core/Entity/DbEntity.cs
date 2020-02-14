using System.Collections.Generic;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DbEntity
    {
        private string _databaseName;
        private List<TableEntity> _tables;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get => _databaseName; set => _databaseName = value?.Trim(); }

        /// <summary>
        /// 数据库描述
        /// </summary>
        public string DatabaseDescription { get; set; }

        /// <summary>
        /// 获取描述信息，如果描述信息为空则返回列名
        /// </summary>
        public string NotEmptyDescription => DatabaseDescription.GetValueOrDefault(_databaseName);

        /// <summary>
        /// 表信息
        /// </summary>
        public List<TableEntity> Tables
        {
            get => _tables;
            set
            {
                if (value != null)
                {
                    _tables = value;
                }
            }
        }

        public DbEntity()
        {
            _tables = new List<TableEntity>();
        }
    }
}
