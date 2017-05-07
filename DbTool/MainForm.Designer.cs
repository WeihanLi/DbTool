namespace DbTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDbPage = new System.Windows.Forms.TabPage();
            this.cbTables = new System.Windows.Forms.CheckedListBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnGenerateModel0 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tabModelPage = new System.Windows.Forms.TabPage();
            this.txtGeneratedSqlText = new System.Windows.Forms.TextBox();
            this.txtTableDesc = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsKey = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanBeNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.FieldSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DefaultValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnGenerateSQL = new System.Windows.Forms.Button();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtConnString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabDbPage.SuspendLayout();
            this.tabModelPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.AutoSize = true;
            this.lblConnStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblConnStatus.Location = new System.Drawing.Point(0, 470);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(107, 20);
            this.lblConnStatus.TabIndex = 6;
            this.lblConnStatus.Text = "数据库未连接！";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDbPage);
            this.tabControl1.Controls.Add(this.tabModelPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 81);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(839, 386);
            this.tabControl1.TabIndex = 7;
            // 
            // tabDbPage
            // 
            this.tabDbPage.Controls.Add(this.cbTables);
            this.tabDbPage.Controls.Add(this.btnExportExcel);
            this.tabDbPage.Controls.Add(this.btnGenerateModel0);
            this.tabDbPage.Controls.Add(this.label2);
            this.tabDbPage.Location = new System.Drawing.Point(4, 29);
            this.tabDbPage.Name = "tabDbPage";
            this.tabDbPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabDbPage.Size = new System.Drawing.Size(831, 353);
            this.tabDbPage.TabIndex = 0;
            this.tabDbPage.Text = "DbFirst";
            this.tabDbPage.UseVisualStyleBackColor = true;
            // 
            // cbTables
            // 
            this.cbTables.FormattingEnabled = true;
            this.cbTables.Location = new System.Drawing.Point(13, 46);
            this.cbTables.Name = "cbTables";
            this.cbTables.Size = new System.Drawing.Size(314, 235);
            this.cbTables.TabIndex = 17;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Enabled = false;
            this.btnExportExcel.Location = new System.Drawing.Point(233, 11);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(94, 32);
            this.btnExportExcel.TabIndex = 16;
            this.btnExportExcel.Text = "导出Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnGenerateModel0
            // 
            this.btnGenerateModel0.Enabled = false;
            this.btnGenerateModel0.Location = new System.Drawing.Point(133, 11);
            this.btnGenerateModel0.Name = "btnGenerateModel0";
            this.btnGenerateModel0.Size = new System.Drawing.Size(94, 32);
            this.btnGenerateModel0.TabIndex = 16;
            this.btnGenerateModel0.Text = "生成Model";
            this.btnGenerateModel0.UseVisualStyleBackColor = true;
            this.btnGenerateModel0.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "选择要生成的表";
            // 
            // tabModelPage
            // 
            this.tabModelPage.Controls.Add(this.txtGeneratedSqlText);
            this.tabModelPage.Controls.Add(this.txtTableDesc);
            this.tabModelPage.Controls.Add(this.dataGridView);
            this.tabModelPage.Controls.Add(this.btnImport);
            this.tabModelPage.Controls.Add(this.btnGenerateSQL);
            this.tabModelPage.Controls.Add(this.txtTableName);
            this.tabModelPage.Controls.Add(this.label6);
            this.tabModelPage.Controls.Add(this.label7);
            this.tabModelPage.Location = new System.Drawing.Point(4, 29);
            this.tabModelPage.Name = "tabModelPage";
            this.tabModelPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabModelPage.Size = new System.Drawing.Size(831, 353);
            this.tabModelPage.TabIndex = 1;
            this.tabModelPage.Text = "ModelFirst";
            this.tabModelPage.UseVisualStyleBackColor = true;
            // 
            // txtGeneratedSqlText
            // 
            this.txtGeneratedSqlText.Location = new System.Drawing.Point(3, 252);
            this.txtGeneratedSqlText.Multiline = true;
            this.txtGeneratedSqlText.Name = "txtGeneratedSqlText";
            this.txtGeneratedSqlText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGeneratedSqlText.Size = new System.Drawing.Size(828, 101);
            this.txtGeneratedSqlText.TabIndex = 24;
            // 
            // txtTableDesc
            // 
            this.txtTableDesc.Location = new System.Drawing.Point(279, 12);
            this.txtTableDesc.Name = "txtTableDesc";
            this.txtTableDesc.Size = new System.Drawing.Size(115, 26);
            this.txtTableDesc.TabIndex = 22;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldName,
            this.FieldDesc,
            this.IsKey,
            this.CanBeNull,
            this.DataType,
            this.FieldSize,
            this.DefaultValue});
            this.dataGridView.Location = new System.Drawing.Point(0, 48);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(831, 207);
            this.dataGridView.TabIndex = 0;
            // 
            // FieldName
            // 
            this.FieldName.HeaderText = "列名称";
            this.FieldName.Name = "FieldName";
            // 
            // FieldDesc
            // 
            this.FieldDesc.HeaderText = "列描述";
            this.FieldDesc.Name = "FieldDesc";
            // 
            // IsKey
            // 
            this.IsKey.HeaderText = "是否是主键列";
            this.IsKey.Name = "IsKey";
            this.IsKey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // CanBeNull
            // 
            this.CanBeNull.HeaderText = "是否可以为空";
            this.CanBeNull.Name = "CanBeNull";
            this.CanBeNull.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CanBeNull.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // DataType
            // 
            this.DataType.HeaderText = "数据类型";
            this.DataType.Items.AddRange(new object[] {
            "INT",
            "BIGINT",
            "FLOAT",
            "REAL",
            "DATETIME",
            "MONEY",
            "BIT",
            "VARCHAR",
            "NVARCHAR",
            "UNIQUEIDENTIFIER"});
            this.DataType.MinimumWidth = 20;
            this.DataType.Name = "DataType";
            this.DataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // FieldSize
            // 
            this.FieldSize.HeaderText = "列长度";
            this.FieldSize.Name = "FieldSize";
            // 
            // DefaultValue
            // 
            this.DefaultValue.HeaderText = "默认值";
            this.DefaultValue.Name = "DefaultValue";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(495, 10);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(109, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "导入Excel";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnGenerateSQL
            // 
            this.btnGenerateSQL.Location = new System.Drawing.Point(403, 11);
            this.btnGenerateSQL.Name = "btnGenerateSQL";
            this.btnGenerateSQL.Size = new System.Drawing.Size(84, 30);
            this.btnGenerateSQL.TabIndex = 0;
            this.btnGenerateSQL.Text = "生成SQL";
            this.btnGenerateSQL.UseVisualStyleBackColor = true;
            this.btnGenerateSQL.Click += new System.EventHandler(this.btnGenerateSQL_Click);
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(63, 12);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(125, 26);
            this.txtTableName.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "表名称";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(194, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 19;
            this.label7.Text = "表描述信息";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(630, 8);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(81, 30);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtConnString
            // 
            this.txtConnString.Location = new System.Drawing.Point(137, 12);
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.Size = new System.Drawing.Size(487, 26);
            this.txtConnString.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "数据库连接字符串";
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(630, 47);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(87, 26);
            this.txtSuffix.TabIndex = 22;
            this.txtSuffix.Text = "Model";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(433, 49);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(100, 26);
            this.txtPrefix.TabIndex = 23;
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(165, 47);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(166, 26);
            this.txtNamespace.TabIndex = 24;
            this.txtNamespace.Text = "Models";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(544, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "model后缀";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(347, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "model前缀";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 20);
            this.label3.TabIndex = 21;
            this.label3.Text = "model的命名空间名称";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 490);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtConnString);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.lblConnStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "DbTool";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabDbPage.ResumeLayout(false);
            this.tabDbPage.PerformLayout();
            this.tabModelPage.ResumeLayout(false);
            this.tabModelPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblConnStatus;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDbPage;
        private System.Windows.Forms.CheckedListBox cbTables;
        private System.Windows.Forms.Button btnGenerateModel0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabModelPage;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldDesc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsKey;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanBeNull;
        private System.Windows.Forms.DataGridViewComboBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultValue;
        private System.Windows.Forms.Button btnGenerateSQL;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTableDesc;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.TextBox txtGeneratedSqlText;
    }
}

