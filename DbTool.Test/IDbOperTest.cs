namespace DbTool.Test
{
    internal interface IDbOperTest
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnStringKey { get; }

        /// <summary>
        /// 数据库查询测试
        /// </summary>
        void QueryTest();

        /// <summary>
        /// 创建表测试
        /// </summary>
        void CreateTest();
    }
}
