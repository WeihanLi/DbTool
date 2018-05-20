export interface DbToolSetting {
    /**数据库类型 */
    DbType: string;

    /**数据库链接字符串 */
    DefaultConnString: string;

    GenDescripionAttr: boolean;

    /**是否要生成私有字段 */
    GeneratePrivateField: boolean;
}
