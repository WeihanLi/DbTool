using DbTool.Core.Entity;

namespace DbTool.Core
{
    public class ModelCodeGenerateOptions
    {
        /// <summary>
        /// Model 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Model 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Model 后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 是否生成 Description Attribute 描述
        /// </summary>
        public bool GenerateDataAnnotation { get; set; } = true;

        /// <summary>
        /// 是否生成私有字段
        /// </summary>
        public bool GeneratePrivateFields { get; set; }

        /// <summary>
        /// 是否使用 NameConverter 将 TableName 转换成 Model class 名称
        /// </summary>
        public bool ApplyNameConverter { get; set; }
    }

    public interface IModelCodeGenerator
    {
        /// <summary>
        /// 生成 Model 代码
        /// </summary>
        /// <param name="tableEntity">表信息</param>
        /// <param name="options">options</param>
        /// <param name="dbType">database type</param>
        /// <returns></returns>
        string GenerateModelCode(TableEntity tableEntity, ModelCodeGenerateOptions options, string dbType);
    }
}
