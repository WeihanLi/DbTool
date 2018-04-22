using Xunit;

namespace DbTool.Test
{
    public class MySqlTest : BaseDbTest
    {
        protected override string DbType => "MySql";

        /// <summary>
        /// ConnectionString
        /// sslMode=none
        /// https://stackoverflow.com/questions/45086283/mysql-data-mysqlclient-mysqlexception-the-host-localhost-does-not-support-ssl
        /// </summary>
        protected override string ConnectionString => "server=localhost;database=testdb;uid=liweihan;pwd=Admin888;SslMode=none";

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
