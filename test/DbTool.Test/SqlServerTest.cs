using Xunit;

namespace DbTool.Test
{
    public class SqlServerTest : BaseDbTest
    {
        public override string ConnStringKey => "SqlServerConn";

        [Fact]
        public override void QueryTest()
        {
            base.QueryTest();
        }

        [Fact]
        public override void CreateTest()
        {
            base.CreateTest();
        }
    }
}
