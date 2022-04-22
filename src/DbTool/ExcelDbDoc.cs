// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core;
using DbTool.Core.Entity;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Collections.Generic;
using System.Linq;
using WeihanLi.Npoi;
using WeihanLi.Npoi.Configurations;

namespace DbTool;

public sealed class ExcelDbDocImporter : IDbDocImporter
{
    public string ImportType => "Excel";

    public Dictionary<string, string> SupportedFileExtensions => new()
    {
        { ".xls", "Excel file(*.xls)" },
        { ".xlsx", "Excel file(*.xlsx)" }
    };

    public TableEntity[] Import(string filePath, IDbProvider dbProvider)
    {
        var workbook = ExcelHelper.LoadExcel(filePath);
        var tables = new TableEntity[workbook.NumberOfSheets];
        for (var i = 0; i < workbook.NumberOfSheets; i++)
        {
            var sheet = workbook.GetSheetAt(i);
            tables[i] = new TableEntity
            {
                TableName = sheet.SheetName,
                TableDescription = sheet.GetRow(0).GetCell(0).StringCellValue
            };
            tables[i].Columns.AddRange(sheet.ToEntityList<ColumnEntity>()
                .Where(x => !string.IsNullOrEmpty(x?.ColumnName))
            );
        }

        return tables;
    }
}

public sealed class ExcelDbDocExporter : IDbDocExporter
{
    public string ExportType => "Excel";

    public string FileExtension => ".xlsx";

    public byte[] Export(TableEntity[] tableInfo, IDbProvider dbProvider)
    {
        var workbook =
            ExcelHelper.PrepareWorkbook(FileExtension.EndsWith(".xls") ? ExcelFormat.Xls : ExcelFormat.Xlsx);
        foreach (var tableEntity in tableInfo)
        {
            var sheet = workbook.CreateSheet(tableEntity.TableName);

            var titleRow = sheet.CreateRow(0);
            var titleCell = titleRow.CreateCell(0);
            titleCell.SetCellValue(tableEntity.NotEmptyDescription);

            sheet.ImportData(tableEntity.Columns);
        }

        return workbook.ToExcelBytes();
    }
}

public sealed class ColumnEntityMappingProfile : IMappingProfile<ColumnEntity>
{
    public void Configure(IExcelConfiguration<ColumnEntity> settings)
    {
        settings.HasExcelSetting(x => { x.Author = "DbTool"; })
            .HasSheetSetting(x =>
            {
                x.StartRowIndex = 2;
                x.AutoColumnWidthEnabled = true;
                x.RowAction = row =>
                {
                    // apply header row style
                    if (row.RowNum == 1)
                    {
                        var headerStyle = row.Sheet.Workbook.CreateCellStyle();
                        headerStyle.Alignment = HorizontalAlignment.Center;
                        var headerFont = row.Sheet.Workbook.CreateFont();
                        headerFont.FontHeight = 180;
                        headerFont.IsBold = true;
                        headerFont.FontName = "微软雅黑";
                        headerStyle.SetFont(headerFont);
                        row.Cells.ForEach(c => c.CellStyle = headerStyle);
                    }
                };
                x.SheetAction = sheet =>
                {
                    // set merged region
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                    // apply title style
                    var titleStyle = sheet.Workbook.CreateCellStyle();
                    titleStyle.Alignment = HorizontalAlignment.Left;
                    var font = sheet.Workbook.CreateFont();
                    font.FontHeight = 200;
                    font.FontName = "微软雅黑";
                    font.IsBold = true;
                    titleStyle.SetFont(font);
                    titleStyle.FillBackgroundColor = IndexedColors.Black.Index;
                    titleStyle.FillForegroundColor = IndexedColors.SeaGreen.Index;
                    titleStyle.FillPattern = FillPattern.SolidForeground;
                    sheet.GetRow(0).GetCell(0).CellStyle = titleStyle;
                };
            });
        settings.Property(x => x.ColumnName)
            .HasColumnIndex(0)
            .HasColumnTitle("列名称");
        settings.Property(x => x.ColumnDescription)
            .HasColumnIndex(1)
            .HasColumnTitle("列描述");
        settings.Property(x => x.IsPrimaryKey)
            .HasColumnIndex(2)
            .HasColumnTitle("是否主键")
            .HasColumnOutputFormatter(x => x ? "Y" : "N")
            .HasColumnInputFormatter(x => "Y".Equals(x));
        settings.Property(x => x.IsNullable)
            .HasColumnIndex(3)
            .HasColumnTitle("是否可以为空")
            .HasColumnOutputFormatter(x => x ? "Y" : "N")
            .HasColumnInputFormatter(x => "Y".Equals(x));
        settings.Property(x => x.DataType)
            .HasColumnIndex(4)
            .HasColumnTitle("数据类型")
            .HasColumnOutputFormatter(x => x?.ToUpper());
        settings.Property(x => x.Size)
            .HasColumnIndex(5)
            .HasColumnTitle("数据长度")
            .HasColumnOutputFormatter(x => x > 0 && x < int.MaxValue ? x.ToString() : string.Empty);
        settings.Property(x => x.DefaultValue)
            .HasColumnIndex(6)
            .HasColumnTitle("默认值")
            .HasOutputFormatter((x, _) =>
            {
                if (x?.DefaultValue != null) return x.DefaultValue.ToString();
                if (x?.IsPrimaryKey == true && x.DataType.ToUpper().Contains("INT")) return "IDENTITY(1,1)";
                return null;
            });
        settings.Property(x => x.NotEmptyDescription).Ignored();
    }
}
