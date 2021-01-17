using System;
using System.Collections.Generic;
using WeihanLi.Extensions;

namespace DbTool.Core.Entity
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    public class DbEntity
    {
        private readonly string _databaseName = null!;
        private List<TableEntity> _tables = new();

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName
        {
            get => _databaseName;
            init => _databaseName = (value ?? throw new ArgumentNullException(nameof(value))).Trim();
        }

        /// <summary>
        /// 数据库描述
        /// </summary>
        public string? DatabaseDescription { get; set; }

        /// <summary>
        /// 获取描述信息，如果描述信息为空则返回列名
        /// </summary>
        public string NotEmptyDescription => DatabaseDescription.GetValueOrDefault(DatabaseName);

        /// <summary>
        /// 表信息
        /// </summary>
#nullable disable

        public List<TableEntity> Tables
        {
            get => _tables;
            set => _tables = value ?? new();
        }

#nullable restore
    }
}
