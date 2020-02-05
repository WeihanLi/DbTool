using System;
using System.Data.Common;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
using MySql.Data.MySqlClient;
using WeihanLi.Extensions;
using InternalDbType = DbTool.DbProvider.MySql.MySqlDbType;

namespace DbTool.DbProvider.MySql
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

        public string DbType2ClrType(string dbType, bool isNullable)
        {
            var sqlDbType = (InternalDbType)Enum.Parse(typeof(InternalDbType), dbType, true);
            string type;
            switch (sqlDbType)
            {
                case InternalDbType.Bit:
                    type = isNullable ? "bool?" : "bool";
                    break;

                case InternalDbType.Float:
                case InternalDbType.Real:
                    type = isNullable ? "double?" : "double";
                    break;

                case InternalDbType.Binary:
                case InternalDbType.VarBinary:
                case InternalDbType.Image:
                case InternalDbType.Timestamp:
                case InternalDbType.RowVersion:
                    type = "byte[]";
                    break;

                case InternalDbType.TinyInt:
                    type = isNullable ? "byte?" : "byte";
                    break;

                case InternalDbType.SmallInt:
                case InternalDbType.Int:
                    type = isNullable ? "int?" : "int";
                    break;

                case InternalDbType.BigInt:
                    type = isNullable ? "long?" : "long";
                    break;

                case InternalDbType.Char:
                case InternalDbType.NChar:
                case InternalDbType.NText:
                case InternalDbType.NVarChar:
                case InternalDbType.VarChar:
                case InternalDbType.Text:
                case InternalDbType.LongText:
                    type = "string";
                    break;

                case InternalDbType.Numeric:
                case InternalDbType.Money:
                case InternalDbType.Decimal:
                case InternalDbType.SmallMoney:
                    type = isNullable ? "decimal?" : "decimal";
                    break;

                case InternalDbType.UniqueIdentifier:
                    type = isNullable ? "Guid?" : "Guid";
                    break;

                case InternalDbType.Date:
                case InternalDbType.SmallDateTime:
                case InternalDbType.DateTime:
                case InternalDbType.DateTime2:
                    type = isNullable ? "DateTime?" : "DateTime";
                    break;

                case InternalDbType.Time:
                    type = isNullable ? "TimeSpan?" : "TimeSpan";
                    break;

                case InternalDbType.DateTimeOffset:
                    type = isNullable ? "DateTimeOffset?" : "DateTimeOffset";
                    break;

                default:
                    type = "object";
                    break;
            }
            return type;
        }

        public string ClrType2DbType(Type type)
        {
            var typeFullName = type.Unwrap().FullName;

            switch (typeFullName)
            {
                case "System.Boolean":
                    return InternalDbType.Bit.ToString();

                case "System.Byte":
                    return InternalDbType.TinyInt.ToString();

                case "System.Int16":
                    return InternalDbType.SmallInt.ToString();

                case "System.Int32":
                    return InternalDbType.Int.ToString();

                case "System.Int64":
                    return InternalDbType.BigInt.ToString();

                case "System.Single":
                    return InternalDbType.Numeric.ToString();

                case "System.Double":
                    return InternalDbType.Float.ToString();

                case "System.Decimal":
                    return InternalDbType.Money.ToString();

                case "System.DateTime":
                    return InternalDbType.DateTime.ToString();

                case "System.DateTimeOffset":
                    return InternalDbType.DateTimeOffset.ToString();

                case "System.Guid":
                    return InternalDbType.UniqueIdentifier.ToString();

                case "System.Object":
                    return InternalDbType.Variant.ToString();

                default:
                    return InternalDbType.NVarChar.ToString();
            }
        }

        public uint GetDefaultSizeForDbType(string dbType, uint defaultLength = 64)
        {
            var sqlDbType = (InternalDbType)Enum.Parse(typeof(InternalDbType), dbType, true);
            var len = defaultLength;
            switch (sqlDbType)
            {
                case InternalDbType.BigInt:
                    len = 8;
                    break;

                case InternalDbType.Binary:
                    len = 8;
                    break;

                case InternalDbType.Bit:
                    len = 1;
                    break;

                case InternalDbType.Char:
                    len = 200;
                    break;

                case InternalDbType.Date:
                case InternalDbType.DateTime:
                case InternalDbType.DateTime2:
                case InternalDbType.DateTimeOffset:
                    len = 8;
                    break;

                case InternalDbType.Decimal:
                    len = 20;
                    break;

                case InternalDbType.Float:
                    len = 20;
                    break;

                case InternalDbType.Image:
                    break;

                case InternalDbType.Int:
                    len = 4;
                    break;

                case InternalDbType.Money:
                    len = 20;
                    break;

                case InternalDbType.NChar:
                    len = 500;
                    break;

                case InternalDbType.NText:
                    len = 200;
                    break;

                case InternalDbType.Numeric:
                    len = 20;
                    break;

                case InternalDbType.NVarChar:
                    len = 2000;
                    break;

                case InternalDbType.Real:
                    len = 10;
                    break;

                case InternalDbType.RowVersion:
                    break;

                case InternalDbType.SmallDateTime:
                    len = 4;
                    break;

                case InternalDbType.SmallInt:
                    len = 2;
                    break;

                case InternalDbType.SmallMoney:
                    break;

                case InternalDbType.Text:
                    len = 5000;
                    break;

                case InternalDbType.Time:
                    len = 8;
                    break;

                case InternalDbType.Timestamp:
                    len = 8;
                    break;

                case InternalDbType.TinyInt:
                    len = 1;
                    break;

                case InternalDbType.UniqueIdentifier:
                    len = 16;
                    break;

                    //case DbType.VarBinary:
                    //case DbType.VarChar:
                    //case DbType.Variant:
                    //case DbType.Xml:
                    //case DbType.Structured:
                    //default:
                    //  break;
            }
            return len;
        }

        public DbConnection GetDbConnection(string connectionString) => new MySqlConnection(connectionString);

        public string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true)
        {
            if (string.IsNullOrWhiteSpace(tableEntity?.TableName))
            {
                return "";
            }
            var sbSqlText = new StringBuilder();
            sbSqlText.AppendLine($"# ---------- Create Table 【{tableEntity.TableName.Trim()}】 Sql -----------");
            sbSqlText.Append($"CREATE TABLE {tableEntity.TableName}(");

            if (tableEntity.Columns.Count > 0)
            {
                foreach (var col in tableEntity.Columns)
                {
                    sbSqlText.AppendLine();
                    sbSqlText.Append($"\t{col.ColumnName} {col.DataType}");
                    if (col.DataType.Contains("CHAR"))
                    {
                        sbSqlText.Append($"({(col.Size == 0 ? GetDefaultSizeForDbType(col.DataType, 2048) : col.Size)})");
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
