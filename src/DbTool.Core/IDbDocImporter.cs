using System.Collections.Generic;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public interface IDbDocImporter
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        string ExportType { get; }

        /// <summary>
        /// 导出文件后缀
        /// </summary>
        ICollection<string> SupportedFiles { get; }

        /// <summary>
        /// 导出数据库文档
        /// </summary>
        /// <param name="bytes">file bytes</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>tables info</returns>
        TableEntity[] Import(byte[] bytes, string dbType);
    }
}
