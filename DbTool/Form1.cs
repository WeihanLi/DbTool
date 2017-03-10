using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DbTool
{
    public partial class MainForm : Form
    {
        private DbHelper dbHelper = null;

        public MainForm()
        {
            InitializeComponent();
        }

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
                cbTables.ValueMember = "TableName";
                //
                lblConnStatus.Text = "数据库连接成功！";
                btnGenerateModel0.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接数据库失败," + ex.Message);
                return;
            }
        }

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
                    foreach (var item in cbTables.CheckedItems)
                    {
                        var tableName = (item as TableInfo).TableName;
                        var cols = dbHelper.GetColumnsInfo(tableName);
                        string content = cols.GenerateModelText(ns , prefix , suffix);
                        string path = dir + "\\" + tableName.TrimTableName() + ".cs";
                        System.IO.File.WriteAllText(path , content,Encoding.UTF8);
                    }
                    MessageBox.Show("保存成功");
                }
                catch (System.IO.IOException ex)
                {
                    MessageBox.Show("IOException:" + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                System.Diagnostics.Process.Start("Explorer.exe" , dir);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.tabControl1.Width = this.Size.Width;
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
                ////数据库连接字符串不为空则创建表
                //if (!String.IsNullOrEmpty(txtConnString.Text))
                //{
                //    new DbHelper(txtConnString.Text).ExecuteNonQuery(sql);
                //}
                Clipboard.SetText(sql);
                MessageBox.Show("生成成功,sql语句已赋值至粘贴板");
            }
            
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {

        }
    }
}