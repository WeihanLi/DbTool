using System.Threading.Tasks;
using DbTool.Core;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DbTool.Test
{
    public class PostgreSqlTest : BaseDbTest
    {
        public override string DbType => "PostgreSql";

        [Fact]
        public override Task QueryTest()
        {
            return base.QueryTest();
        }

        [Fact]
        public override void CreateTest()
        {
            base.CreateTest();
        }

        public PostgreSqlTest(IConfiguration configuration, IDbHelperFactory dbHelperFactory, DbProviderFactory dbProviderFactory) : base(configuration, dbHelperFactory, dbProviderFactory)
        {
        }
    }
}
