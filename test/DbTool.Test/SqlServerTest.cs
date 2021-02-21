using System.Threading.Tasks;
using DbTool.Core;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DbTool.Test
{
    public class SqlServerTest : BaseDbTest
    {
        public override string ConnStringKey => "SqlServerConn";

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

        public SqlServerTest(IConfiguration configuration, IDbHelperFactory dbHelperFactory, DbProviderFactory dbProviderFactory) : base(configuration, dbHelperFactory, dbProviderFactory)
        {
        }
    }
}
