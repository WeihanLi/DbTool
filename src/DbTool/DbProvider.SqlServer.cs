using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace DbTool
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

        public string QueryDbTablesSqlFormat => @"SELECT  IT.TABLE_NAME AS TableName ,
                                                IT.TABLE_CATALOG AS DatabaseName ,
                                                IT.TABLE_TYPE AS TableType ,
                                                IT.TABLE_SCHEMA AS TableScheme ,
                                                [EP].[value] AS TableDescription
                                        FROM    INFORMATION_SCHEMA.TABLES AS IT
                                                LEFT JOIN sys.extended_properties AS EP ON EP.major_id = OBJECT_ID([IT].[TABLE_NAME])
                                                                                           AND [EP].[minor_id] = 0;";

        public string QueryTableColumnsSqlFormat => @"SELECT  t.[name] AS TableName ,
                                                                                        c.[name] AS ColumnName ,
                                                                                        p.[value] AS ColumnDescription ,
                                                                                        c.[is_nullable] AS IsNullable ,
                                                                                        ty.[name] AS DataType ,
                                                                                        [ty].[max_length] AS Size ,
                                                                                        c.[is_identity] AS IsPrimaryKey ,
                                                                                        SUBSTRING(dc.[definition], 2, LEN([dc].[definition]) - 2) AS DefaultValue
                                                                                FROM    sys.columns c
                                                                                        JOIN sys.tables t ON c.object_id = t.object_id
                                                                                        JOIN sys.[types] ty ON ty.[system_type_id] = c.[system_type_id]
                                                                                                               AND ty.[name] != 'sysname'
                                                                                        LEFT JOIN sys.extended_properties p ON p.minor_id = c.column_id
                                                                                                                               AND p.major_id = c.object_id
                                                                                        LEFT JOIN [sys].[default_constraints] dc ON dc.[parent_object_id] = c.[object_id]
                                                                                                                                    AND dc.[parent_column_id] = c.[column_id]
                                                                                                                                    AND dc.[type] = 'D'
                                                                                WHERE   t.name = @tableName
                                                                                ORDER BY c.[column_id];";

        public DbConnection GetDbConnection(string connectionString) => new SqlConnection(connectionString);

        public string GenerateSqlStatement(TableEntity tableEntity, bool generateDescription = true)
        {
            if (string.IsNullOrWhiteSpace(tableEntity?.TableName))
            {
                return "";
            }
            var sbSqlText = new StringBuilder();

            var sbSqlDescText = new StringBuilder();
            //create table
            sbSqlText.AppendLine($"---------- Create Table 【{tableEntity.TableName}】 Sql -----------");
            sbSqlText.Append($"CREATE TABLE [{(string.IsNullOrWhiteSpace(tableEntity.TableSchema) ? "dbo" : tableEntity.TableSchema)}].[{tableEntity.TableName.Trim()}](");
            //create description
            if (generateDescription && !string.IsNullOrEmpty(tableEntity.TableDescription))
            {
                sbSqlDescText.AppendFormat(CreateTableDescSqlFormat, tableEntity.TableName, tableEntity.TableDescription);
            }
            if (tableEntity.Columns.Count > 0)
            {
                foreach (var col in tableEntity.Columns)
                {
                    sbSqlText.AppendLine();
                    sbSqlText.AppendFormat("\t[{0}] {1}", col.ColumnName.Trim(), col.DataType);
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
                    if (!string.IsNullOrEmpty(col.DefaultValue?.ToString()))
                    {
                        if ((col.DataType.Contains("CHAR") || col.DataType.Contains("TEXT"))
                            && !col.DefaultValue.ToString().StartsWith("N'") && !col.DefaultValue.ToString().StartsWith("'"))
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
                    //
                    sbSqlText.Append(",");
                    //
                    if (generateDescription && !string.IsNullOrEmpty(col.ColumnDescription))
                    {
                        sbSqlDescText.AppendLine();
                        sbSqlDescText.AppendFormat(ConfigurationHelper.AppSetting(ConfigurationConstants.DbDescriptionGenType).EqualsIgnoreCase("AddOrUpdate") ? CreateOrUpdateColumnDescSqlFormat : CreateColumnDescSqlFormat, tableEntity.TableName, col.ColumnName, col.ColumnDescription);
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
