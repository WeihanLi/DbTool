using System.Collections.Generic;
using System.Linq;
using WeihanLi.Extensions;

namespace DbTool.Core
{
    public class DbProviderFactory
    {
        private readonly IReadOnlyCollection<IDbProvider> _dbProviders;

        public DbProviderFactory(IEnumerable<IDbProvider> dbProviders) => _dbProviders = dbProviders.ToArray();

        public IDbProvider GetDbProvider(string dbType) => _dbProviders.FirstOrDefault(p => p.DbType.EqualsIgnoreCase(dbType));

        public IReadOnlyCollection<string> AllowedDbTypes => _dbProviders.Select(_ => _.DbType).ToArray();
    }
}
