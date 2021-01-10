using System;
using System.Collections.Generic;
using System.Linq;
using WeihanLi.Extensions;

namespace DbTool.Core
{
    public class DbProviderFactory
    {
        private readonly IDictionary<string, IDbProvider> _dbProviders;

        public DbProviderFactory(IEnumerable<IDbProvider> dbProviders)
        {
            _dbProviders = dbProviders.ToDictionary(p => p.DbType, p => p, StringComparer.OrdinalIgnoreCase);
            SupportedDbTypes = _dbProviders.Keys.ToArray();
        }

        public IDbProvider GetDbProvider(string dbType) => _dbProviders[dbType];

        public IReadOnlyList<string> SupportedDbTypes { get; }
    }
}
