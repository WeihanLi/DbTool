using System.Collections.Generic;
using System.Threading.Tasks;
using DbTool.Core.Entity;

namespace DbTool.Core
{
    public interface IDbHelper
    {
        string DbType { get; }

        string DatabaseName { get; }

        Task<List<TableEntity>> GetTablesInfoAsync();

        Task<List<ColumnEntity>> GetColumnsInfoAsync(string tableName);
    }
}
