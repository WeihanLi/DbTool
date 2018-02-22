using Xunit;

namespace DbTool.Test
{
    public class MySqlTest : BaseDbTest
    {
        public override string ConnStringKey => "MySqlConn";

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
