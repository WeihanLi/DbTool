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
        /// 导出数据库文档
        /// </summary>
        /// <param name="tableInfo">表信息</param>
        /// <param name="path">要保存的路径</param>
        /// <returns>whether success</returns>
        bool Export(TableEntity[] tableInfo, string path);
    }
}
