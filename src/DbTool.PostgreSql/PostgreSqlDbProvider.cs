using System;
using System.Data.Common;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
using Npgsql;
using WeihanLi.Extensions;
using InternalDbType = DbTool.DbProvider.PostgreSql.SqlDbType;

namespace DbTool.DbProvider.PostgreSql
{
    public class PostgreSqlDbProvider : IDbProvider
    {
        public string DbType => "PostgreSql";

        public virtual string QueryDbTablesSqlFormat => @"
SELECT @dbName AS DatabaseName, nspname AS TableSchema, relname AS TableName,
  description AS TableDescription
FROM pg_class AS cls
JOIN pg_namespace AS ns ON ns.oid = cls.relnamespace
LEFT OUTER JOIN pg_description AS des ON des.objoid = cls.oid AND des.objsubid=0
WHERE
  cls.relkind IN ('r', 'v', 'm') AND
  ns.nspname NOT IN ('pg_catalog', 'information_schema')
";

        public virtual string QueryTableColumnsSqlFormat => @"
SELECT cls.relname AS TableName,
       attr.attname AS ColumnName,
       attr.atttypmod - 4 AS Size,
       (CASE WHEN cons.conname IS NOT NULL THEN 1 ELSE 0 END) AS IsPrimaryKey,
       description AS ColumnDescription,
       typ.typname AS DataType,
       NOT (attnotnull OR typ.typnotnull) AS IsNullable,
       CASE
           WHEN atthasdef THEN (SELECT pg_get_expr(adbin, cls.oid)
                                FROM pg_attrdef
                                WHERE adrelid = cls.oid AND adnum = attr.attnum)
           END AS DefaultValue

FROM pg_class AS cls
         JOIN pg_namespace AS ns ON ns.oid = cls.relnamespace
         LEFT JOIN pg_attribute AS attr ON attrelid = cls.oid
         LEFT JOIN pg_constraint AS cons ON cons.conrelid = cls.oid AND attr.attnum = cons.conkey[1] AND cons.contype='p'
         LEFT JOIN pg_type AS typ ON attr.atttypid = typ.oid
         LEFT JOIN pg_proc ON pg_proc.oid = typ.typreceive
         LEFT JOIN pg_description AS des ON des.objoid = cls.oid AND des.objsubid = attnum
WHERE cls.relkind IN ('r', 'v', 'm')
  AND nspname NOT IN ('pg_catalog', 'information_schema')
  AND attnum > 0
  AND cls.relname=@tableName
ORDER BY attnum
";

        public virtual string DbType2ClrType(string dbType, bool isNullable)
        {
            var sqlDbType = (InternalDbType)Enum.Parse(typeof(InternalDbType), dbType, true);
            var type = sqlDbType switch
            {
                InternalDbType.Bool => isNullable ? "bool?" : "bool",
                InternalDbType.Boolean => isNullable ? "bool?" : "bool",
                InternalDbType.Real => isNullable ? "float?" : "float",
                InternalDbType.Float4 => isNullable ? "float?" : "float",
                InternalDbType.Float8 => isNullable ? "double?" : "double",
                InternalDbType.Double => isNullable ? "double?" : "double",
                InternalDbType.ByteA => "byte[]",
                InternalDbType.Int2 => isNullable ? "short?" : "short",
                InternalDbType.SmallInt => isNullable ? "short?" : "short",
                InternalDbType.SmallSerial => isNullable ? "short?" : "short",
                InternalDbType.Serial2 => isNullable ? "short?" : "short",
                InternalDbType.Int4 => isNullable ? "int?" : "int",
                InternalDbType.Integer => isNullable ? "int?" : "int",
                InternalDbType.Serial => isNullable ? "int?" : "int",
                InternalDbType.Serial4 => isNullable ? "int?" : "int",
                InternalDbType.BigInt => isNullable ? "long?" : "long",
                InternalDbType.Int8 => isNullable ? "long?" : "long",
                InternalDbType.BigSerial => isNullable ? "long?" : "long",
                InternalDbType.Serial8 => isNullable ? "long?" : "long",
                InternalDbType.Char => "string",
                InternalDbType.VarChar => "string",
                InternalDbType.Text => "string",
                InternalDbType.Character => "string",
                InternalDbType.Numeric => isNullable ? "decimal?" : "decimal",
                InternalDbType.Money => isNullable ? "decimal?" : "decimal",
                InternalDbType.Decimal => isNullable ? "decimal?" : "decimal",
                InternalDbType.Uuid => isNullable ? "Guid?" : "Guid",
                InternalDbType.Date => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.Time => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.Timestamp => isNullable ? "DateTime?" : "DateTime",
                InternalDbType.TimeZ => isNullable ? "DateTimeOffset?" : "DateTimeOffset",
                InternalDbType.TimestampZ => isNullable ? "DateTimeOffset?" : "DateTimeOffset",
                _ => "object"
            };
            return type;
        }

        public virtual string ClrType2DbType(Type type)
        {
            var typeFullName = type.Unwrap().FullName;

            switch (typeFullName)
            {
                case "System.Boolean":
                    return InternalDbType.Boolean.ToString();

                case "System.Int16":
                    return InternalDbType.SmallInt.ToString();

                case "System.Int32":
                    return InternalDbType.Integer.ToString();

                case "System.Int64":
                    return InternalDbType.BigInt.ToString();

                case "System.Single":
                    return InternalDbType.Real.ToString();

                case "System.Double":
                    return InternalDbType.Double.ToString();

                case "System.Decimal":
                    return InternalDbType.Decimal.ToString();

                case "System.DateTime":
                    return InternalDbType.Timestamp.ToString();

                case "System.DateTimeOffset":
                    return InternalDbType.TimestampZ.ToString();

                case "System.Guid":
                    return InternalDbType.Uuid.ToString();

                default:
                    return InternalDbType.VarChar.ToString();
            }
        }

        public virtual uint GetDefaultSizeForDbType(string dbType, uint defaultLength = 64)
        {
            var sqlDbType = (InternalDbType)Enum.Parse(typeof(InternalDbType), dbType, true);
            uint len = sqlDbType switch
            {
                InternalDbType.Bool => 1,
                InternalDbType.Boolean => 1,
                InternalDbType.Real => 4,
                InternalDbType.Float4 => 4,
                InternalDbType.Float8 => 8,
                InternalDbType.Double => 8,

                InternalDbType.Int2 => 2,
                InternalDbType.SmallInt => 2,
                InternalDbType.SmallSerial => 2,
                InternalDbType.Serial2 => 2,
                InternalDbType.Int4 => 4,
                InternalDbType.Integer => 4,
                InternalDbType.Serial => 4,
                InternalDbType.Serial4 => 4,
                InternalDbType.BigInt => 8,
                InternalDbType.BigSerial => 8,
                InternalDbType.Serial8 => 8,

                InternalDbType.Money => 8,

                InternalDbType.Uuid => 32,

                InternalDbType.Date => 4,
                InternalDbType.Time => 8,
                InternalDbType.Timestamp => 8,
                InternalDbType.TimeZ => 12,
                InternalDbType.TimestampZ => 8,
                _ => defaultLength
            };
            return len;
        }

        public virtual DbConnection GetDbConnection(string connectionString) => new NpgsqlConnection(connectionString);

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
