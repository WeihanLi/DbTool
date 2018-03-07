using Xunit;

namespace DbTool.Test
{
    public class PostgreSqlTest : BaseDbTest
    {
        public override string ConnStringKey => "PostgreSqlConn";

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
