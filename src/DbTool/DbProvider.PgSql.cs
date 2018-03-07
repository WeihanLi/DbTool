using System;
using System.Data.Common;

namespace DbTool
{
    public class PgSqlDbProvider : IDbProvider
    {
        public string DbType => "PgSql";
        public string QueryDbTablesSqlFormat { get; }
        public string QueryTableColumnsSqlFormat { get; }

        public DbConnection GetDbConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true)
        {
            throw new NotImplementedException();
        }
    }
}
