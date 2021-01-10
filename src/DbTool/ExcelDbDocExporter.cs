using System.Linq;
using DbTool.Core;
using DbTool.Core.Entity;
using NPOI.SS.UserModel;
using WeihanLi.Npoi;

namespace DbTool
{
    public class ExcelDbDocExporter : IDbDocExporter
    {
        public string ExportType => "Excel";

        public string FileExtension => ".xls";

        public byte[] Export(TableEntity[] tableInfo, string dbType)
        {
            var workbook = ExcelHelper.PrepareWorkbook(!FileExtension.EndsWith(".xls"));
            foreach (var tableEntity in tableInfo)
            {
                //Create Sheet
                var tempSheet = workbook.CreateSheet(tableEntity.TableName);
                //create title
                var titleRow = tempSheet.CreateRow(0);
                var titleCell = titleRow.CreateCell(0);
                titleCell.SetCellValue(tableEntity.TableDescription);
                var titleStyle = workbook.CreateCellStyle();
                titleStyle.Alignment = HorizontalAlignment.Left;
                var font = workbook.CreateFont();
                font.FontHeight = 14 * 14;
                font.FontName = "微软雅黑";
                font.IsBold = true;
                titleStyle.SetFont(font);
                titleStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                titleStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.SeaGreen.Index;
                titleStyle.FillPattern = FillPattern.SolidForeground;
                titleCell.CellStyle = titleStyle;
                //merged cells on single row
                //ATTENTION: don't use Region class, which is obsolete
                tempSheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 6));

                //create header
                var headerRow = tempSheet.CreateRow(1);
                var headerStyle = workbook.CreateCellStyle();
                headerStyle.Alignment = HorizontalAlignment.Center;
                var headerFont = workbook.CreateFont();
                font.FontHeight = 11 * 11;
                font.FontName = "微软雅黑";
                titleStyle.SetFont(headerFont);

                var headerCell0 = headerRow.CreateCell(0);
                headerCell0.CellStyle = headerStyle;
                headerCell0.SetCellValue("列名称");
                var headerCell1 = headerRow.CreateCell(1);
                headerCell1.CellStyle = headerStyle;
                headerCell1.SetCellValue("列描述");
                var headerCell2 = headerRow.CreateCell(2);
                headerCell2.CellStyle = headerStyle;
                headerCell2.SetCellValue("是否是主键");
                var headerCell3 = headerRow.CreateCell(3);
                headerCell3.CellStyle = headerStyle;
                headerCell3.SetCellValue("是否可以为空");
                var headerCell4 = headerRow.CreateCell(4);
                headerCell4.CellStyle = headerStyle;
                headerCell4.SetCellValue("数据类型");
                var headerCell5 = headerRow.CreateCell(5);
                headerCell5.CellStyle = headerStyle;
                headerCell5.SetCellValue("数据长度");
                var headerCell6 = headerRow.CreateCell(6);
                headerCell6.CellStyle = headerStyle;
                headerCell6.SetCellValue("默认值");

                //exist any column
                if (tableEntity.Columns.Any())
                {
                    IRow tempRow;
                    ICell tempCell;
                    for (var i = 1; i <= tableEntity.Columns.Count; i++)
                    {
                        tempRow = tempSheet.CreateRow(i + 1);
                        tempCell = tempRow.CreateCell(0);
                        tempCell.SetCellValue(tableEntity.Columns[i - 1].ColumnName);
                        tempCell = tempRow.CreateCell(1);
                        tempCell.SetCellValue(tableEntity.Columns[i - 1].ColumnDescription);
                        tempCell = tempRow.CreateCell(2);
                        tempCell.SetCellValue(tableEntity.Columns[i - 1].IsPrimaryKey ? "Y" : "N");
                        tempCell = tempRow.CreateCell(3);
                        tempCell.SetCellValue(tableEntity.Columns[i - 1].IsNullable ? "Y" : "N");
                        tempCell = tempRow.CreateCell(4);
                        tempCell.SetCellValue(tableEntity.Columns[i - 1].DataType?.ToUpper());
                        tempCell = tempRow.CreateCell(5);
                        tempCell.SetCellValue(
                            tableEntity.Columns[i - 1].Size > 0 && tableEntity.Columns[i - 1].Size < int.MaxValue
                            ? tableEntity.Columns[i - 1].Size.ToString()
                            : string.Empty);
                        tempCell = tempRow.CreateCell(6);

                        var defaultVal = tableEntity.Columns[i - 1].DefaultValue;
                        if (defaultVal is not null)
                        {
                            tempCell.SetCellValue(defaultVal.ToString());
                        }
                        else
                        {
                            if (tableEntity.Columns[i - 1].DataType?.ToUpper().Contains("INT") == true && tableEntity.Columns[i - 1].IsPrimaryKey)
                            {
                                tempCell.SetCellValue("IDENTITY(1,1)");
                            }
                        }
                    }
                }

                // 自动调整单元格的宽度
                tempSheet.AutoSizeColumn(0);
                tempSheet.AutoSizeColumn(1);
                tempSheet.AutoSizeColumn(2);
                tempSheet.AutoSizeColumn(3);
                tempSheet.AutoSizeColumn(4);
                tempSheet.AutoSizeColumn(5);
                tempSheet.AutoSizeColumn(6);
            }
            return workbook.ToExcelBytes();
        }
    }
}
