using System.Data.Common;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
using MySql.Data.MySqlClient;

namespace DbTool.MySql
{
    public class MySqlDbProvider : IDbProvider
    {
        public string DbType => "MySql";

        public string QueryDbTablesSqlFormat => @"SELECT
	@dbName AS DatabaseName,
    `ENGINE` AS TableSchema,
	TABLE_NAME AS TableName,
	TABLE_COMMENT AS TableDescription
FROM
	information_schema.`TABLES`
WHERE
	table_schema = @dbName";

        public string QueryTableColumnsSqlFormat => @"SELECT
	TABLE_NAME AS TableName,
	COLUMN_NAME AS ColumnName,
	COLUMN_Comment AS ColumnDescription,
	IF(IS_NULLABLE='YES','True','False') AS IsNullable,
	DATA_TYPE AS DataType,
	CHARACTER_MAXIMUM_LENGTH AS Size,
	IF(COLUMN_KEY != '','True','False') AS IsPrimaryKey,
	COLUMN_DEFAULT AS DefaultValue
FROM
	information_schema.`COLUMNS`
WHERE
	table_schema = @dbName
AND table_name = @tableName;";

        public DbConnection GetDbConnection(string connectionString) => new MySqlConnection(connectionString);

        public string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true)
        {
            if (string.IsNullOrWhiteSpace(tableEntity?.TableName))
            {
                return "";
            }
            var sbSqlText = new StringBuilder();
            sbSqlText.AppendLine($"# ---------- Create Table 【{tableEntity.TableName.Trim()}】 Sql -----------");
            sbSqlText.Append($"CREATE TABLE {tableEntity.TableName.Trim()}(");

            if (tableEntity.Columns.Count > 0)
            {
                foreach (var col in tableEntity.Columns)
                {
                    sbSqlText.AppendLine();
                    sbSqlText.Append($"\t{col.ColumnName} {col.DataType}");
                    if (col.DataType.Contains("CHAR"))
                    {
                        sbSqlText.Append($"({(col.Size == 0 ? Utility.GetDefaultSizeForDbType(col.DataType) : col.Size)})");
                    }
                    if (col.IsPrimaryKey)
                    {
                        sbSqlText.Append(" PRIMARY KEY");
                        if (col.DataType.Contains("INT"))
                        {
                            sbSqlText.Append(" AUTO_INCREMENT");
                        }
                    }
                    //Nullable
                    if (!col.IsNullable)
                    {
                        sbSqlText.Append(" NOT NULL");
                    }
                    //Default Value
                    if (!string.IsNullOrEmpty(col.DefaultValue?.ToString()))
                    {
                        if (!col.IsPrimaryKey)
                        {
                            if ((col.DataType.Contains("CHAR") || col.DataType.Contains("TEXT"))
                                && !col.DefaultValue.ToString().StartsWith("'"))
                            {
                                sbSqlText.AppendFormat(" DEFAULT '{0}'", col.DefaultValue);
                            }
                            else
                            {
                                sbSqlText.AppendFormat(" DEFAULT {0}", col.DefaultValue);
                            }
                        }
                    }
                    //Comment
                    if (generateDescription && !string.IsNullOrEmpty(col.ColumnDescription))
                    {
                        sbSqlText.Append($" COMMENT '{col.ColumnDescription}'");
                    }

                    sbSqlText.Append(",");
                }
                sbSqlText.Remove(sbSqlText.Length - 1, 1);
                sbSqlText.AppendLine();
            }

            sbSqlText.Append($") ENGINE={(string.IsNullOrWhiteSpace(tableEntity.TableSchema) ? "InnoDB" : tableEntity.TableSchema)}");
            sbSqlText.AppendLine(!string.IsNullOrWhiteSpace(tableEntity.TableDescription) && generateDescription
                ? $" COMMENT='{tableEntity.TableDescription}'" : "");
            sbSqlText.AppendLine();
            return sbSqlText.ToString();
        }
    }
}
