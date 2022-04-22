// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core;
using DbTool.Core.Entity;
using System.Collections.Generic;
using System.Linq;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;
using WeihanLi.Npoi;
using WeihanLi.Npoi.Configurations;

namespace DbTool;

public sealed class CsvDbDocExporter : IDbDocExporter
{
    public byte[] Export(TableEntity[] tableInfo, IDbProvider dbProvider)
    {
        return tableInfo.Select(t => t.Columns.Select(c => new TableInfoForCsv
        {
            TableName = t.TableName,
            TableDescription = t.TableDescription,
            TableSchema = t.TableSchema,
            ColumnName = c.ColumnName,
            ColumnDescription = c.ColumnDescription,
            DataType = c.DataType,
            IsNullable = c.IsNullable,
            IsPrimaryKey = c.IsPrimaryKey,
            Size = c.Size,
            DefaultValue = c.DefaultValue
        })).SelectMany(e => e).ToCsvBytes();
    }

    public string ExportType => "csv";
    public string FileExtension => ".csv";
}

public sealed class CsvDbDocImporter : IDbDocImporter
{
    public TableEntity[] Import(string filePath, IDbProvider dbProvider)
    {
        var entities = CsvHelper.ToEntities<TableInfoForCsv>(filePath);
        return entities.WhereNotNull().GroupBy(e => new { e.TableName, e.TableSchema, e.TableDescription }).Select(g =>
            new TableEntity()
            {
                TableName = g.Key.TableName,
                TableDescription = g.Key.TableDescription,
                TableSchema = g.Key.TableSchema,
                Columns = g.Select(MapHelper.Map<TableInfoForCsv, ColumnEntity>).ToList()
            }).ToArray();
    }

    public string ImportType => "csv";

    public Dictionary<string, string> SupportedFileExtensions { get; } = new()
    {
        { ".csv", "csv files" }
    };
}

public sealed record TableInfoForCsv : ColumnEntity
{
    public string TableName { get; set; } = string.Empty;
    public string? TableDescription { get; set; }
    public string? TableSchema { get; set; }
}

public sealed class CsvTableInfoMappingProfile : IMappingProfile<TableInfoForCsv>
{
    public void Configure(IExcelConfiguration<TableInfoForCsv> configuration)
    {
        configuration.Property(x => x.TableName).HasColumnIndex(0);
        configuration.Property(x => x.TableDescription).HasColumnIndex(1);
        configuration.Property(x => x.TableSchema).HasColumnIndex(2);

        configuration.Property(x => x.ColumnName)
            .HasColumnIndex(3);
        configuration.Property(x => x.ColumnDescription)
            .HasColumnIndex(4);
        configuration.Property(x => x.IsPrimaryKey)
            .HasColumnIndex(5)
            .HasColumnOutputFormatter(x => x ? "Y" : "N")
            .HasColumnInputFormatter(x => "Y".Equals(x));
        configuration.Property(x => x.IsNullable)
            .HasColumnIndex(6)
            .HasColumnOutputFormatter(x => x ? "Y" : "N")
            .HasColumnInputFormatter(x => "Y".Equals(x));
        configuration.Property(x => x.DataType)
            .HasColumnIndex(7)
            .HasColumnOutputFormatter(x => x?.ToUpper());
        configuration.Property(x => x.Size)
            .HasColumnIndex(8)
            .HasColumnOutputFormatter(x => x is > 0 and < int.MaxValue ? x.ToString() : string.Empty);
        configuration.Property(x => x.DefaultValue)
            .HasColumnIndex(9)
            .HasOutputFormatter((x, _) =>
            {
                if (x?.DefaultValue != null) return x.DefaultValue.ToString();
                if (x?.IsPrimaryKey == true && x.DataType.ToUpper().Contains("INT")) return "IDENTITY(1,1)";
                return string.Empty;
            });
        configuration.Property(x => x.NotEmptyDescription).Ignored();
    }
}
