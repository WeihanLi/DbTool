using System;
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
                //
                cbTables.DataSource = tables;
                cbTables.DisplayMember = "TableName";
                cbTables.ValueMember = "TableName";
                //
                lblConnStatus.Text = "数据库连接成功！";
                btnGenerate.Enabled = true;
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
                        System.IO.File.WriteAllText(path , content);
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
    }
}