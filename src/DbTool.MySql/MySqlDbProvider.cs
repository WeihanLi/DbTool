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

        public virtual string QueryDbTablesSqlFormat => @"
SELECT @dbName AS DatabaseName,
    `ENGINE` AS TableSchema,
	TABLE_NAME AS TableName,
	TABLE_COMMENT AS TableDescription
FROM
	information_schema.`TABLES`
WHERE
	table_schema = @dbName";

        public virtual string QueryTableColumnsSqlFormat => @"
SELECT TABLE_NAME AS TableName,
	COLUMN_NAME AS ColumnName,
	COLUMN_Comment AS ColumnDescription,
	IF(IS_NULLABLE='YES','True','False') AS IsNullable,
	DATA_TYPE AS DataType,
	IF(CHARACTER_MAXIMUM_LENGTH IS NULL, 0, CHARACTER_MAXIMUM_LENGTH) AS Size,
	IF(COLUMN_KEY != '','True','False') AS IsPrimaryKey,
	COLUMN_DEFAULT AS DefaultValue
FROM
	information_schema.`COLUMNS`
WHERE
	table_schema = @dbName
AND table_name = @tableName;";

        public virtual string DbType2ClrType(string dbType, bool isNullable)
        {
            var sqlDbType = (InternalDbType)Enum.Parse(typeof(InternalDbType), dbType, true);
            var type = sqlDbType switch
            {
                InternalDbType.Bit => isNullable ? "bool?" : "bool",
                InternalDbType.Float => isNullable ? "double?" : "double",
                InternalDbType.Real => isNullable ? "double?" : "double",
                InternalDbType.Binary => "byte[]",
                InternalDbType.VarBinary => "byte[]",
                InternalDbType.Image => "byte[]",
                InternalDbType.Timestamp => "byte[]",
                InternalDbType.RowVersion => "byte[]",
                InternalDbType.TinyInt => isNullable ? "byte?" : "byte",
                InternalDbType.SmallInt => isNullable ? "int?" : "int",
                InternalDbType.Int => isNullable ? "int?" : "int",
                InternalDbType.BigInt => isNullable ? "long?" : "long",
                InternalDbType.Char => "string",
                InternalDbType.NChar => "string",
                InternalDbType.NText => "string",
                InternalDbType.NVarChar => "string",
                InternalDbType.VarChar => "string",
                InternalDbType.Text => "string",
                InternalDbType.LongText => "string",
                InternalDbType.Numeric => isNullable ? "decimal?" : "decimal",
                InternalDbType.Money => isNullable ? "decimal?" : "decimal",
                InternalDbType.Decimal => isNullable ? "decimal?" : "decimal",
                InternalDbType.SmallMoney => isNullable ? "decimal?" : "decimal",
                InternalDbType.UniqueIdentifier => isNullable ? "Guid?" : "Guid",
                InternalDbType.Date => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.SmallDateTime => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.DateTime => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.DateTime2 => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.Time => isNullable ? "TimeSpan?" : "TimeSpan",
                InternalDbType.DateTimeOffset => isNullable ? "DateTimeOffset?" : "DateTimeOffset",
                _ => "object"
            };
            return type;
        }

        public virtual string ClrType2DbType(Type type)
        {
            var typeFullName = type.Unwrap().FullName;
            return typeFullName switch
            {
                "System.Boolean" => InternalDbType.Bit.ToString(),
                "System.Byte" => InternalDbType.TinyInt.ToString(),
                "System.Int16" => InternalDbType.SmallInt.ToString(),
                "System.Int32" => InternalDbType.Int.ToString(),
                "System.Int64" => InternalDbType.BigInt.ToString(),
                "System.Single" => InternalDbType.Numeric.ToString(),
                "System.Double" => InternalDbType.Float.ToString(),
                "System.Decimal" => InternalDbType.Money.ToString(),
                "System.DateTime" => InternalDbType.DateTime.ToString(),
                "System.DateTimeOffset" => InternalDbType.DateTimeOffset.ToString(),
                "System.Guid" => InternalDbType.UniqueIdentifier.ToString(),
                "System.Object" => InternalDbType.Variant.ToString(),
                _ => InternalDbType.NVarChar.ToString()
            };
        }

        public virtual uint GetDefaultSizeForDbType(string dbType, uint defaultLength = 64)
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

        public virtual DbConnection GetDbConnection(string connectionString) => new MySqlConnection(connectionString);

        public virtual string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true)
        {
            if (string.IsNullOrWhiteSpace(tableEntity.TableName))
            {
                return string.Empty;
            }
            var sbSqlText = new StringBuilder();
            sbSqlText.AppendLine($"# ---------- Create Table 【{tableEntity.TableName}】 Sql -----------");
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
                    var defaultValueStr = col.DefaultValue?.ToString();
                    if (defaultValueStr is not null)
                    {
                        if (!col.IsPrimaryKey)
                        {
                            if ((col.DataType.Contains("CHAR")
                                 || col.DataType.Contains("TEXT"))
                                && !defaultValueStr.StartsWith("'"))
                            {
                                sbSqlText.AppendFormat(" DEFAULT '{0}'", defaultValueStr);
                            }
                            else
                            {
                                sbSqlText.AppendFormat(" DEFAULT {0}", defaultValueStr);
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
