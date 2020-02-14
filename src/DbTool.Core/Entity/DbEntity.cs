using System.Collections.Generic;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DbEntity
    {
        private string _description;
        private string _databaseName;
        private List<TableEntity> _tables;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get => _databaseName; set => _databaseName = value?.Trim(); }

        /// <summary>
        /// 数据库描述
        /// </summary>
        public string DatabaseDescription { get => _description.GetValueOrDefault(DatabaseName); set => _description = value; }

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
