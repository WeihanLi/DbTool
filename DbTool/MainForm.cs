using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;

namespace DbTool
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 数据库助手
        /// </summary>
        private DbHelper dbHelper = null;

        public MainForm()
        {
            InitializeComponent();
            lnkExcelTemplate.Links.Add(0,2, "https://github.com/WeihanLi/DbTool/raw/master/DbTool/template.xls");
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender , EventArgs e)
        {
            if (String.IsNullOrEmpty(txtConnString.Text))
            {
                return;
            }
            try
            {
                dbHelper = new DbHelper(txtConnString.Text);
                var tables = dbHelper.GetTablesInfo();
                var tableList = (from table in tables orderby table.TableName select table).ToList();
                //
                cbTables.DataSource = tableList;
                cbTables.DisplayMember = "TableName";
                cbTables.ValueMember = "TableDesc";
                //
                lblConnStatus.Text = "数据库连接成功！当前数据库："+dbHelper.DatabaseName;
                btnGenerateModel0.Enabled = true;
                btnExportExcel.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接数据库失败," + ex.Message);
            }
        }

        /// <summary>
        /// 根据数据库表信息生成Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender , EventArgs e)
        {
            if (dbHelper == null)
            {
                MessageBox.Show("请先连接数据库");
                return;
            }
            if (cbTables.CheckedItems.Count <= 0)
            {
                MessageBox.Show("请先选择要生成model的表");
                return;
            }
            string prefix = txtPrefix.Text, suffix = txtSuffix.Text;
            string ns = txtNamespace.Text;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择要保存model的文件夹";
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string dir = dialog.SelectedPath;
                if (String.IsNullOrEmpty(ns))
                {
                    ns = "Models";
                }
                try
                {
                    TableEntity tableEntity = new TableEntity();
                    if (cbTables.CheckedItems.Count > 0)
                    {
                        foreach (var item in cbTables.CheckedItems)
                        {
                            var currentTable = item as TableEntity;
                            if (currentTable == null)
                            {
                                continue;
                            }
                            tableEntity.TableName = currentTable.TableName;
                            tableEntity.TableDesc = currentTable.TableDesc;
                            tableEntity.Columns = dbHelper.GetColumnsInfo(tableEntity.TableName);
                            string content = tableEntity.GenerateModelText(ns, prefix, suffix);
                            string path = dir + "\\" + tableEntity.TableName.TrimTableName() + ".cs";
                            System.IO.File.WriteAllText(path, content, Encoding.UTF8);
                        }
                        MessageBox.Show("保存成功");
                        System.Diagnostics.Process.Start("Explorer.exe", dir);
                    }
                    else
                    {
                        MessageBox.Show("请选择要生成的表");
                    }
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show("IOException:" + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 窗口大小变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.tabControl1.Width = this.Size.Width;
            this.tabControl1.Height = this.Size.Height - 160;
            this.dataGridView.Width = this.Size.Width-20;
            this.dataGridView.Height = this.tabControl1.Height*2/5;
            this.txtGeneratedSqlText.Width = this.Size.Width-20;
            this.txtGeneratedSqlText.Height = this.tabControl1.Height*2/5;
            this.cbTables.Height = this.tabControl1.Height - 100;
        }

        /// <summary>
        /// 根据表信息生成创建SQL表信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateSQL_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtTableName.Text) || String.IsNullOrEmpty(txtTableDesc.Text))
            {
                return;
            }
            if (dataGridView.Rows.Count > 1)
            {
                var tableInfo = new TableEntity()
                {
                    TableName = txtTableName.Text.Trim(),
                    TableDesc = txtTableDesc.Text.Trim(),
                    Columns = new List<ColumnEntity>()
                };
                ColumnEntity column;
                for (int k = 0; k < dataGridView.Rows.Count-1; k++)
                {
                    column = new ColumnEntity();
                    column.ColumnName = dataGridView.Rows[k].Cells[0].Value.ToString();
                    column.ColumnDesc = dataGridView.Rows[k].Cells[1].Value.ToString();
                    column.IsPrimaryKey = dataGridView.Rows[k].Cells[2].Value != null && (bool) dataGridView.Rows[k].Cells[2].Value;
                    column.IsNullable = dataGridView.Rows[k].Cells[3].Value != null && (bool) dataGridView.Rows[k].Cells[3].Value;
                    column.DataType = dataGridView.Rows[k].Cells[4].Value.ToString();
                    column.Size = dataGridView.Rows[k].Cells[5].Value == null? 0 :Convert.ToInt32(dataGridView.Rows[k].Cells[5].Value.ToString());
                    column.DefaultValue = dataGridView.Rows[k].Cells[6].Value;
                    //
                    tableInfo.Columns.Add(column);
                }
                //sql
                string sql = tableInfo.GenerateSqlStatement();
                //注：创建数据表个人觉得属于危险操作，暂时先不考虑直接在数据库中生成表，可以将创建表的sql粘贴到所需执行的地方二次确认后再创建数据库表，如果确实要在数据库中直接生成表可以取消注释以下代码
                ////数据库连接字符串不为空则创建表
                //if (!String.IsNullOrEmpty(txtConnString.Text))
                //{
                //    new DbHelper(txtConnString.Text).ExecuteNonQuery(sql);
                //}
                txtGeneratedSqlText.Text = sql;
                Clipboard.SetText(sql);
                MessageBox.Show("生成成功,sql语句已赋值至粘贴板");
            }
            
        }

        /// <summary>
        /// 导入Excel生成创建数据库表sql与创建数据库表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Multiselect = false;
            if (ofg.ShowDialog() == DialogResult.OK)
            {
                string path = ofg.FileName;
                TableEntity table = new TableEntity();
                using (Stream stream = File.OpenRead(path))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);                    
                    table.TableName = sheet.SheetName;
                    var rows = sheet.GetRowEnumerator();
                    dataGridView.Rows.Clear();
                    while (rows.MoveNext())
                    {
                        var row = (IRow) rows.Current;
                        if (row.RowNum == 0)
                        {
                            table.TableDesc = row.Cells[0].StringCellValue;
                            txtTableName.Text = table.TableName;
                            txtTableDesc.Text = table.TableDesc;
                            continue;
                        }
                        if (row.RowNum > 1)
                        {
                            var column = new ColumnEntity();
                            column.ColumnName = row.Cells[0].StringCellValue;
                            if (String.IsNullOrWhiteSpace(column.ColumnName))
                            {
                                continue;
                            }
                            column.ColumnDesc = row.Cells[1].StringCellValue;
                            column.IsPrimaryKey = row.Cells[2].StringCellValue.Equals("Y");
                            column.IsNullable = row.Cells[3].StringCellValue.Equals("Y");
                            column.DataType = row.Cells[4].StringCellValue;
                            column.Size = Convert.ToInt32(row.Cells[5].NumericCellValue);
                            column.DefaultValue = row.Cells[6];
                            table.Columns.Add(column);

                            DataGridViewRow rowView = new DataGridViewRow();
                            rowView.CreateCells(
                                dataGridView,
                                column.ColumnName,
                                column.ColumnDesc , 
                                column.IsPrimaryKey,
                                column.IsNullable,
                                column.DataType,
                                column.Size,
                                column.DefaultValue
                            );
                            dataGridView.Rows.Add(rowView);
                        }
                    }
                }
                //sql
                string sql = table.GenerateSqlStatement();
                //注：创建数据表个人觉得属于危险操作，暂时先不考虑直接在数据库中生成表，可以将创建表的sql粘贴到所需执行的地方二次确认后再创建数据库表，如果确实要在数据库中直接生成表可以取消注释以下代码
                ////数据库连接字符串不为空则创建表
                //if (!String.IsNullOrEmpty(txtConnString.Text))
                //{
                //    new DbHelper(txtConnString.Text).ExecuteNonQuery(sql);
                //}
                txtGeneratedSqlText.Text = sql;
                Clipboard.SetText(sql);
                MessageBox.Show("生成成功，sql语句已赋值至粘贴板");
            }
        }

        /// <summary>
        /// 导出数据库表信息到Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (dbHelper == null)
            {
                MessageBox.Show("请先连接数据库");
                return;
            }
            if (cbTables.CheckedItems.Count <= 0)
            {
                MessageBox.Show("请先选择要生成model的表");
                return;
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择要保存excel文件的文件夹";
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string dir = dialog.SelectedPath;
                try
                {
                    TableEntity tableEntity = new TableEntity();
                    if (cbTables.CheckedItems.Count > 0)
                    {
                        HSSFWorkbook workbook = new HSSFWorkbook();
                        string tempFileName = (cbTables.CheckedItems.Count>1? dbHelper.DatabaseName : (cbTables.CheckedItems[0] as TableEntity)?.TableName);

                        foreach (var item in cbTables.CheckedItems)
                        {
                            var currentTable = item as TableEntity;
                            if (currentTable == null)
                            {
                                continue;
                            }
                            tableEntity.TableName = currentTable.TableName;
                            tableEntity.TableDesc = currentTable.TableDesc;
                            tableEntity.Columns = dbHelper.GetColumnsInfo(tableEntity.TableName);                           
                            //Create Sheet
                            var tempSheet = workbook.CreateSheet(tableEntity.TableName);
                            //create title
                            IRow titleRow = tempSheet.CreateRow(0);
                            ICell titleCell = titleRow.CreateCell(0);
                            titleCell.SetCellValue(tableEntity.TableDesc);
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
                            var headerfont = workbook.CreateFont();
                            font.FontHeight = 11 * 11;
                            font.FontName = "微软雅黑";
                            titleStyle.SetFont(headerfont);

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
                                for (int i = 1; i <= tableEntity.Columns.Count; i++)
                                {
                                    tempRow = tempSheet.CreateRow(i + 1);
                                    tempCell = tempRow.CreateCell(0);
                                    tempCell.SetCellValue(tableEntity.Columns[i-1].ColumnName);
                                    tempCell = tempRow.CreateCell(1);
                                    tempCell.SetCellValue(tableEntity.Columns[i - 1].ColumnDesc);
                                    tempCell = tempRow.CreateCell(2);
                                    tempCell.SetCellValue(tableEntity.Columns[i - 1].IsPrimaryKey?"Y":"N");
                                    tempCell = tempRow.CreateCell(3);
                                    tempCell.SetCellValue(tableEntity.Columns[i - 1].IsNullable?"Y":"N");
                                    tempCell = tempRow.CreateCell(4);
                                    tempCell.SetCellValue(tableEntity.Columns[i - 1].DataType.ToUpper());
                                    tempCell = tempRow.CreateCell(5);
                                    tempCell.SetCellValue(tableEntity.Columns[i - 1].Size>0? tableEntity.Columns[i - 1].Size.ToString() : "");
                                    tempCell = tempRow.CreateCell(6);
                                    if (tableEntity.Columns[i - 1].DefaultValue != null)
                                    {
                                        tempCell.SetCellValue(tableEntity.Columns[i - 1].DefaultValue.ToString());
                                    }
                                    else
                                    {
                                        if (tableEntity.Columns[i - 1].IsPrimaryKey)
                                        {
                                            tempCell.SetCellValue("IDENTITY");
                                        }
                                    }
                                }
                            }
                            tempSheet.AutoSizeColumn(0);
                            tempSheet.AutoSizeColumn(1);
                            tempSheet.AutoSizeColumn(2);
                            tempSheet.AutoSizeColumn(3);
                            tempSheet.AutoSizeColumn(4);
                            tempSheet.AutoSizeColumn(5);
                            tempSheet.AutoSizeColumn(6);
                        }
                        string path = dir +"\\"+ tempFileName + ".xls";
                        using (FileStream file = new FileStream(path, FileMode.Create))
                        {
                            workbook.Write(file);
                        }
                        MessageBox.Show("保存成功");
                        System.Diagnostics.Process.Start("Explorer.exe", dir);
                    }
                    else
                    {
                        MessageBox.Show("请选择要生成的表");
                    }
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show("IOException:" + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void lnkExcelTemplate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}