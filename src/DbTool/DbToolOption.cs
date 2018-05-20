namespace DbTool
{
    public class DbToolOption
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public string DefaultConnString { get; set; }

        public bool GenDescripionAttr { get; set; }

        /// <summary>
        /// 是否要生成私有字段
        /// </summary>
        public bool GeneratePrivateField { get; set; }
    }
}
