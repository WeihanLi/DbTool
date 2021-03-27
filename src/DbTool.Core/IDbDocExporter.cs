using DbTool.Core.Entity;

namespace DbTool.Core
{
    public interface IDbDocExporter
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        string ExportType { get; }

        /// <summary>
        /// 导出文件后缀
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// 导出数据库文档
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="dbProvider">数据库类型</param>
        /// <returns>whether success</returns>
        byte[] Export(TableEntity[] tableInfo, IDbProvider dbProvider);
    }
}
