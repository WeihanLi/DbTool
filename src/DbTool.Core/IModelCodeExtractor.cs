using System.Collections.Generic;
using System.Threading.Tasks;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public interface IModelCodeExtractor
    {
        Dictionary<string, string> SupportedFileExtensions { get; }
        public string CodeType { get; }

        Task<List<TableEntity>> GetTablesFromSourceText(IDbProvider dbProvider, string sourceText);

        Task<List<TableEntity>> GetTablesFromSourceFiles(IDbProvider dbProvider, params string[] sourceFilePaths);
    }
}
