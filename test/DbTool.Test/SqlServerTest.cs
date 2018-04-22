using Xunit;

namespace DbTool.Test
{
    public class SqlServerTest : BaseDbTest
    {
        protected override string DbType => "SqlServer";
        protected override string ConnectionString => "server=.;database=Reservation;uid=liweihan;pwd=Admin888";

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
