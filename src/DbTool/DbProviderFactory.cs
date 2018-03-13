using System.Collections.Generic;
using System.Linq;
using WeihanLi.Common;
using WeihanLi.Extensions;

namespace DbTool
{
    public class DbProviderFactory
    {
        private readonly IReadOnlyCollection<IDbProvider> _dbProviders;
        
        public DbProviderFactory(IReadOnlyCollection<IDbProvider> dbProviders) => _dbProviders = dbProviders;

        public IDbProvider GetDbProvider(string dbType) => _dbProviders.FirstOrDefault(p => p.DbType.EqualsIgnoreCase(dbType));

        public IReadOnlyCollection<string> AllowedDbTypes => _dbProviders.Select(_ => _.DbType).ToArray();
    }
}
