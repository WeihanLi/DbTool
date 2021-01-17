using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using DbTool.Core;
using DbTool.Core.Entity;
using WeihanLi.Extensions;
using InternalDbType = DbTool.DbProvider.SqlServer.SqlServerDbType;

namespace DbTool.DbProvider.SqlServer
{
    public class SqlServerDbProvider : IDbProvider
    {
        #region Create Description

        /// <summary>
        /// 创建或更新表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        private const string CreateOrUpdateTableDescSqlFormat = @"
BEGIN
IF EXISTS (
       SELECT 1
    FROM sys.extended_properties p,
         sys.tables t,
         sys.schemas s
    WHERE t.schema_id = s.schema_id
          AND p.major_id = t.object_id
          AND p.minor_id = 0
          AND p.name = N'MS_Description'
          AND s.name = N'dbo'
          AND t.name = N'{0}'
    )
        EXECUTE sp_updateextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';
ELSE
        EXECUTE sp_addextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';
END";

        /// <summary>
        /// 创建表描述
        /// 0：表名称
        /// 1：表描述
        /// </summary>
        private const string CreateTableDescSqlFormat = @"EXECUTE sp_addextendedproperty N'MS_Description', N'{1}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}';";

        /// <summary>
        /// 列描述信息
        /// 0：表名称，1：列名称，2：列描述信息
        /// </summary>
        private const string CreateOrUpdateColumnDescSqlFormat = @"
BEGIN
IF EXISTS (
        select 1
        from
            sys.extended_properties p,
            sys.columns c,
            sys.tables t,
            sys.schemas s
        where
            t.schema_id = s.schema_id and
            c.object_id = t.object_id and
            p.major_id = t.object_id and
            p.minor_id = c.column_id and
            p.name = N'MS_Description' and
            s.name = N'dbo' and
            t.name = N'{0}' and
            c.name = N'{1}'
    )
        EXECUTE sp_updateextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}';
ELSE
        EXECUTE sp_addextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}';
END";

        /// <summary>
        /// 列描述信息
        /// 0：表名称，1：列名称，2：列描述信息
        /// </summary>
        private const string CreateColumnDescSqlFormat = @"EXECUTE sp_addextendedproperty N'MS_Description', N'{2}', N'SCHEMA', N'dbo',  N'TABLE', N'{0}', N'COLUMN', N'{1}';";

        #endregion Create Description

        public string DbType => "SqlServer";

        public string QueryDbTablesSqlFormat => @"
SELECT IT.TABLE_NAME AS TableName,
       IT.TABLE_CATALOG AS DatabaseName,
       IT.TABLE_TYPE AS TableType,
       IT.TABLE_SCHEMA AS TableScheme,
       [EP].[value] AS TableDescription
FROM INFORMATION_SCHEMA.TABLES AS IT
    LEFT JOIN sys.extended_properties AS EP
        ON EP.major_id = OBJECT_ID([IT].[TABLE_NAME])
           AND [EP].[minor_id] = 0
WHERE IT.TABLE_TYPE = 'BASE TABLE';
";

        public string QueryTableColumnsSqlFormat => @"
SELECT t.[name] AS TableName,
       c.[name] AS ColumnName,
       p.[value] AS ColumnDescription,
       c.[is_nullable] AS IsNullable,
       IIF(k.COLUMN_NAME IS NULL, 0, 1) AS IsPrimaryKey,
       ty.[name] AS DataType,
       IIF([col].[CHARACTER_MAXIMUM_LENGTH] IS NULL, [c].[max_length], [col].[CHARACTER_MAXIMUM_LENGTH]) AS Size,
       SUBSTRING(dc.[definition], 2, LEN([dc].[definition]) - 2) AS DefaultValue
FROM sys.columns c
    JOIN sys.tables t
        ON c.object_id = t.object_id
    JOIN sys.[types] ty
        ON ty.[system_type_id] = c.[system_type_id]
           AND ty.[name] != 'sysname'
	join INFORMATION_SCHEMA.COLUMNS col on c.name=col.COLUMN_NAME AND t.[name]=col.TABLE_NAME
    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS k
        ON k.TABLE_NAME = @tableName
           AND k.COLUMN_NAME = c.[name]
    LEFT JOIN sys.extended_properties p
        ON p.minor_id = c.column_id
           AND p.major_id = c.object_id
    LEFT JOIN [sys].[default_constraints] dc
        ON dc.[parent_object_id] = c.[object_id]
           AND dc.[parent_column_id] = c.[column_id]
           AND dc.[type] = 'D'
WHERE t.name = @tableName
ORDER BY c.[column_id];
";

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

        public DbConnection GetDbConnection(string connectionString) => new SqlConnection(connectionString);

        public string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true) =>
            GenerateSqlStatement(tableEntity, generateDescription, false);

        public string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription, bool addOrUpdateDesc)
        {
            if (string.IsNullOrWhiteSpace(tableEntity.TableName))
            {
                return "";
            }
            var sbSqlText = new StringBuilder();

            var sbSqlDescText = new StringBuilder();
            //create table
            sbSqlText.AppendLine($"---------- Create Table 【{tableEntity.TableName}】 Sql -----------");
            sbSqlText.Append($"CREATE TABLE [{(string.IsNullOrWhiteSpace(tableEntity.TableSchema) ? "dbo" : tableEntity.TableSchema)}].[{tableEntity.TableName}](");
            //create description
            if (generateDescription && !string.IsNullOrEmpty(tableEntity.TableDescription))
            {
                sbSqlDescText.AppendFormat(addOrUpdateDesc ? CreateOrUpdateTableDescSqlFormat : CreateTableDescSqlFormat, tableEntity.TableName, tableEntity.TableDescription);
            }
            if (tableEntity.Columns.Count > 0)
            {
                foreach (var col in tableEntity.Columns)
                {
                    sbSqlText.AppendLine();
                    sbSqlText.AppendFormat("\t[{0}] {1}", col.ColumnName, col.DataType);
                    if (col.DataType.ToUpperInvariant().Contains("CHAR"))
                    {
                        sbSqlText.Append($"({col.Size})");
                    }
                    if (col.IsPrimaryKey)
                    {
                        sbSqlText.Append(" PRIMARY KEY");
                        if (col.DataType.Contains("INT"))
                        {
                            sbSqlText.Append(" IDENTITY(1,1) ");
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
                            if ((col.DataType.Contains("CHAR") || col.DataType.Contains("TEXT"))
                                && !defaultValueStr.StartsWith("N'") && !defaultValueStr.StartsWith("'") != true)
                            {
                                sbSqlText.AppendFormat(" DEFAULT(N'{0}')", col.DefaultValue);
                            }
                            else
                            {
                                switch (col.DataType)
                                {
                                    case "BIT":
                                        if (col.DefaultValue is bool bVal)
                                        {
                                            sbSqlText.AppendFormat(" DEFAULT({0}) ", bVal ? 1 : 0);
                                        }

                                        break;

                                    default:
                                        sbSqlText.AppendFormat(" DEFAULT({0}) ", col.DefaultValue);
                                        break;
                                }
                            }
                        }
                    }
                    //
                    sbSqlText.Append(",");
                    //
                    if (generateDescription && !string.IsNullOrEmpty(col.ColumnDescription))
                    {
                        sbSqlDescText.AppendLine();
                        sbSqlDescText.AppendFormat(addOrUpdateDesc ? CreateOrUpdateColumnDescSqlFormat : CreateColumnDescSqlFormat, tableEntity.TableName, col.ColumnName, col.ColumnDescription);
                    }
                }
                sbSqlText.Remove(sbSqlText.Length - 1, 1);
                sbSqlText.AppendLine();
            }
            sbSqlText.AppendLine(");");
            if (generateDescription && sbSqlDescText.Length > 0)
            {
                sbSqlText.AppendLine();
                sbSqlText.AppendLine($"---------- Create Table 【{tableEntity.TableName}】 Description Sql -----------");
                sbSqlText.Append(sbSqlDescText);
            }
            sbSqlText.AppendLine();
            return sbSqlText.ToString();
        }
    }
}
