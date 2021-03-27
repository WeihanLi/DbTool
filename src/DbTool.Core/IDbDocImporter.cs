using System.Collections.Generic;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public interface IDbDocImporter
    {
        /// <summary>
        /// 导入类型
        /// </summary>
        string ImportType { get; }

        /// <summary>
        /// 导入文件后缀
        /// Key, 文件后缀名
        /// Value，文件后缀类型描述
        /// </summary>
        Dictionary<string, string> SupportedFileExtensions { get; }

        /// <summary>
        /// 导入数据库文档
        /// </summary>
        /// <param name="filePath">filePath</param>
        /// <param name="dbProvider">数据库类型</param>
        /// <returns>tables info</returns>
        TableEntity[] Import(string filePath, IDbProvider dbProvider);
    }
}
