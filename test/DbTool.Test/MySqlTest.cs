using System.Threading.Tasks;
using DbTool.Core;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DbTool.Test
{
    public class MySqlTest : BaseDbTest
    {
        public override string ConnStringKey => "MySqlConn";

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

        public MySqlTest(IConfiguration configuration, DbProviderFactory dbProviderFactory) : base(configuration, dbProviderFactory)
        {
        }
    }
}
