using System.Collections.Generic;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DbEntity
    {
        private string databaseDescription;
        private string databaseName;
        private List<TableEntity> tables;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get => databaseName; set => databaseName = value?.Trim(); }

        /// <summary>
        /// 数据库描述
        /// </summary>
        public string DatabaseDescription { get => databaseDescription.GetValueOrDefault(DatabaseName); set => databaseDescription = value; }

        /// <summary>
        /// 表信息
        /// </summary>
        public List<TableEntity> Tables
        {
            get => tables; 
            set
            {
                if(value != null)
                { 
                    tables = value;
                }
            }
        }

        public DbEntity()
        {
            tables = new List<TableEntity>();
        }
    }
}
