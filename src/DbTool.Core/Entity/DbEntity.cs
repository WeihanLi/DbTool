using System.Collections.Generic;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DbEntity
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 数据库描述
        /// </summary>
        public string DatabaseDescription { get; set; }

        /// <summary>
        /// 表信息
        /// </summary>
        public List<TableEntity> Tables { get; set; }

        public DbEntity()
        {
            Tables = new List<TableEntity>();
        }
    }
}
