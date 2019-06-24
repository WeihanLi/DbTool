namespace DbTool
{
    public partial class MainForm
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
            _dbHelper?.Dispose();
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDbPage = new System.Windows.Forms.TabPage();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.txtConnString = new System.Windows.Forms.TextBox();
            this.cbGenDescriptionAttr = new System.Windows.Forms.CheckBox();
            this.cbGenField = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTables = new System.Windows.Forms.CheckedListBox();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.btnGenerateModel0 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabModelPage = new System.Windows.Forms.TabPage();
            this.cbGenDbDescription = new System.Windows.Forms.CheckBox();
            this.lnkExcelTemplate = new System.Windows.Forms.LinkLabel();
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
            this.tabCodePage = new System.Windows.Forms.TabPage();
            this.cbGenCodeSqlDescription = new System.Windows.Forms.CheckBox();
            this.treeViewTable = new System.Windows.Forms.TreeView();
            this.txtCodeModelSql = new System.Windows.Forms.TextBox();
            this.btnImportModel = new System.Windows.Forms.Button();
            this.tabSettingPage = new System.Windows.Forms.TabPage();
            this.lblDefaultDbConn = new System.Windows.Forms.Label();
            this.txtDefaultDbConn = new System.Windows.Forms.TextBox();
            this.lblDefaultDbType = new System.Windows.Forms.Label();
            this.cbDefaultDbType = new System.Windows.Forms.ComboBox();
            this.btnUpdateSetting = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabDbPage.SuspendLayout();
            this.tabModelPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabCodePage.SuspendLayout();
            this.tabSettingPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.AutoSize = true;
            this.lblConnStatus.Location = new System.Drawing.Point(3, 446);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(107, 20);
            this.lblConnStatus.TabIndex = 6;
            this.lblConnStatus.Text = "数据库未连接！";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabDbPage);
            this.tabControl.Controls.Add(this.tabModelPage);
            this.tabControl.Controls.Add(this.tabCodePage);
            this.tabControl.Controls.Add(this.tabSettingPage);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(938, 509);
            this.tabControl.TabIndex = 7;
            // 
            // tabDbPage
            // 
            this.tabDbPage.Controls.Add(this.btnConnect);
            this.tabDbPage.Controls.Add(this.txtSuffix);
            this.tabDbPage.Controls.Add(this.txtConnString);
            this.tabDbPage.Controls.Add(this.cbGenDescriptionAttr);
            this.tabDbPage.Controls.Add(this.cbGenField);
            this.tabDbPage.Controls.Add(this.label1);
            this.tabDbPage.Controls.Add(this.cbTables);
            this.tabDbPage.Controls.Add(this.btnExportExcel);
            this.tabDbPage.Controls.Add(this.txtPrefix);
            this.tabDbPage.Controls.Add(this.btnGenerateModel0);
            this.tabDbPage.Controls.Add(this.label2);
            this.tabDbPage.Controls.Add(this.label3);
            this.tabDbPage.Controls.Add(this.txtNamespace);
            this.tabDbPage.Controls.Add(this.label4);
            this.tabDbPage.Controls.Add(this.label5);
            this.tabDbPage.Controls.Add(this.lblConnStatus);
            this.tabDbPage.Location = new System.Drawing.Point(4, 29);
            this.tabDbPage.Name = "tabDbPage";
            this.tabDbPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabDbPage.Size = new System.Drawing.Size(930, 476);
            this.tabDbPage.TabIndex = 0;
            this.tabDbPage.Text = "DbFirst";
            this.tabDbPage.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(629, 13);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(81, 30);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(627, 53);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(87, 26);
            this.txtSuffix.TabIndex = 22;
            this.txtSuffix.Text = "Model";
            // 
            // txtConnString
            // 
            this.txtConnString.Location = new System.Drawing.Point(136, 17);
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.Size = new System.Drawing.Size(487, 26);
            this.txtConnString.TabIndex = 17;
            // 
            // cbGenDescriptionAttr
            // 
            this.cbGenDescriptionAttr.AutoSize = true;
            this.cbGenDescriptionAttr.Checked = true;
            this.cbGenDescriptionAttr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenDescriptionAttr.Location = new System.Drawing.Point(119, 83);
            this.cbGenDescriptionAttr.Name = "cbGenDescriptionAttr";
            this.cbGenDescriptionAttr.Size = new System.Drawing.Size(201, 24);
            this.cbGenDescriptionAttr.TabIndex = 18;
            this.cbGenDescriptionAttr.Text = "生成 Description Atrribute";
            this.cbGenDescriptionAttr.UseVisualStyleBackColor = true;
            // 
            // cbGenField
            // 
            this.cbGenField.AutoSize = true;
            this.cbGenField.Location = new System.Drawing.Point(10, 83);
            this.cbGenField.Name = "cbGenField";
            this.cbGenField.Size = new System.Drawing.Size(112, 24);
            this.cbGenField.TabIndex = 18;
            this.cbGenField.Text = "生成私有字段";
            this.cbGenField.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "数据库连接字符串";
            // 
            // cbTables
            // 
            this.cbTables.FormattingEnabled = true;
            this.cbTables.Location = new System.Drawing.Point(10, 158);
            this.cbTables.Name = "cbTables";
            this.cbTables.Size = new System.Drawing.Size(327, 277);
            this.cbTables.TabIndex = 17;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Enabled = false;
            this.btnExportExcel.Location = new System.Drawing.Point(219, 112);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(118, 38);
            this.btnExportExcel.TabIndex = 16;
            this.btnExportExcel.Text = "导出Excel文档";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(430, 55);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(100, 26);
            this.txtPrefix.TabIndex = 23;
            // 
            // btnGenerateModel0
            // 
            this.btnGenerateModel0.Enabled = false;
            this.btnGenerateModel0.Location = new System.Drawing.Point(119, 112);
            this.btnGenerateModel0.Name = "btnGenerateModel0";
            this.btnGenerateModel0.Size = new System.Drawing.Size(94, 38);
            this.btnGenerateModel0.TabIndex = 16;
            this.btnGenerateModel0.Text = "生成Model";
            this.btnGenerateModel0.UseVisualStyleBackColor = true;
            this.btnGenerateModel0.Click += new System.EventHandler(this.btnGenerateModel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "选择要生成的表";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 20);
            this.label3.TabIndex = 21;
            this.label3.Text = "model的命名空间名称";
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(162, 53);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(166, 26);
            this.txtNamespace.TabIndex = 24;
            this.txtNamespace.Text = "Models";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(344, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 20;
            this.label4.Text = "model前缀";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(541, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 19;
            this.label5.Text = "model后缀";
            // 
            // tabModelPage
            // 
            this.tabModelPage.AutoScroll = true;
            this.tabModelPage.Controls.Add(this.cbGenDbDescription);
            this.tabModelPage.Controls.Add(this.lnkExcelTemplate);
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
            this.tabModelPage.Size = new System.Drawing.Size(930, 476);
            this.tabModelPage.TabIndex = 1;
            this.tabModelPage.Text = "ModelFirst";
            this.tabModelPage.UseVisualStyleBackColor = true;
            // 
            // cbGenDbDescription
            // 
            this.cbGenDbDescription.AutoSize = true;
            this.cbGenDbDescription.Checked = true;
            this.cbGenDbDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenDbDescription.Location = new System.Drawing.Point(410, 13);
            this.cbGenDbDescription.Name = "cbGenDbDescription";
            this.cbGenDbDescription.Size = new System.Drawing.Size(126, 24);
            this.cbGenDbDescription.TabIndex = 26;
            this.cbGenDbDescription.Text = "生成数据库描述";
            this.cbGenDbDescription.UseVisualStyleBackColor = true;
            // 
            // lnkExcelTemplate
            // 
            this.lnkExcelTemplate.AutoSize = true;
            this.lnkExcelTemplate.Location = new System.Drawing.Point(770, 13);
            this.lnkExcelTemplate.Name = "lnkExcelTemplate";
            this.lnkExcelTemplate.Size = new System.Drawing.Size(99, 20);
            this.lnkExcelTemplate.TabIndex = 25;
            this.lnkExcelTemplate.TabStop = true;
            this.lnkExcelTemplate.Text = "下载Excel模板";
            this.lnkExcelTemplate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkExcelTemplate_LinkClicked);
            // 
            // txtGeneratedSqlText
            // 
            this.txtGeneratedSqlText.Location = new System.Drawing.Point(0, 270);
            this.txtGeneratedSqlText.Multiline = true;
            this.txtGeneratedSqlText.Name = "txtGeneratedSqlText";
            this.txtGeneratedSqlText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGeneratedSqlText.Size = new System.Drawing.Size(927, 177);
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
            this.dataGridView.Size = new System.Drawing.Size(927, 225);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
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
            "NUMERIC",
            "DATETIME",
            "DATETIME2",
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
            this.btnImport.Location = new System.Drawing.Point(635, 8);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(109, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "导入Excel";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // btnGenerateSQL
            // 
            this.btnGenerateSQL.Location = new System.Drawing.Point(536, 9);
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
            // tabCodePage
            // 
            this.tabCodePage.Controls.Add(this.cbGenCodeSqlDescription);
            this.tabCodePage.Controls.Add(this.treeViewTable);
            this.tabCodePage.Controls.Add(this.txtCodeModelSql);
            this.tabCodePage.Controls.Add(this.btnImportModel);
            this.tabCodePage.Location = new System.Drawing.Point(4, 29);
            this.tabCodePage.Name = "tabCodePage";
            this.tabCodePage.Padding = new System.Windows.Forms.Padding(3);
            this.tabCodePage.Size = new System.Drawing.Size(930, 476);
            this.tabCodePage.TabIndex = 2;
            this.tabCodePage.Text = "CodeFirst";
            this.tabCodePage.UseVisualStyleBackColor = true;
            // 
            // cbGenCodeSqlDescription
            // 
            this.cbGenCodeSqlDescription.AutoSize = true;
            this.cbGenCodeSqlDescription.Checked = true;
            this.cbGenCodeSqlDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenCodeSqlDescription.Location = new System.Drawing.Point(213, 21);
            this.cbGenCodeSqlDescription.Name = "cbGenCodeSqlDescription";
            this.cbGenCodeSqlDescription.Size = new System.Drawing.Size(126, 24);
            this.cbGenCodeSqlDescription.TabIndex = 4;
            this.cbGenCodeSqlDescription.Text = "生成数据库描述";
            this.cbGenCodeSqlDescription.UseVisualStyleBackColor = true;
            // 
            // treeViewTable
            // 
            this.treeViewTable.Location = new System.Drawing.Point(22, 65);
            this.treeViewTable.Name = "treeViewTable";
            this.treeViewTable.ShowNodeToolTips = true;
            this.treeViewTable.Size = new System.Drawing.Size(206, 382);
            this.treeViewTable.TabIndex = 3;
            // 
            // txtCodeModelSql
            // 
            this.txtCodeModelSql.Location = new System.Drawing.Point(234, 65);
            this.txtCodeModelSql.Multiline = true;
            this.txtCodeModelSql.Name = "txtCodeModelSql";
            this.txtCodeModelSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCodeModelSql.Size = new System.Drawing.Size(686, 379);
            this.txtCodeModelSql.TabIndex = 2;
            // 
            // btnImportModel
            // 
            this.btnImportModel.Location = new System.Drawing.Point(22, 17);
            this.btnImportModel.Name = "btnImportModel";
            this.btnImportModel.Size = new System.Drawing.Size(164, 30);
            this.btnImportModel.TabIndex = 1;
            this.btnImportModel.Text = "选择Model文件（C#）";
            this.btnImportModel.UseVisualStyleBackColor = true;
            this.btnImportModel.Click += new System.EventHandler(this.btnImportModel_Click);
            // 
            // tabSettingPage
            // 
            this.tabSettingPage.Controls.Add(this.btnUpdateSetting);
            this.tabSettingPage.Controls.Add(this.cbDefaultDbType);
            this.tabSettingPage.Controls.Add(this.lblDefaultDbType);
            this.tabSettingPage.Controls.Add(this.txtDefaultDbConn);
            this.tabSettingPage.Controls.Add(this.lblDefaultDbConn);
            this.tabSettingPage.Location = new System.Drawing.Point(4, 29);
            this.tabSettingPage.Name = "tabSettingPage";
            this.tabSettingPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettingPage.Size = new System.Drawing.Size(930, 476);
            this.tabSettingPage.TabIndex = 3;
            this.tabSettingPage.Text = "Settings";
            this.tabSettingPage.UseVisualStyleBackColor = true;
            // 
            // lblDefaultDbConn
            // 
            this.lblDefaultDbConn.AutoSize = true;
            this.lblDefaultDbConn.Location = new System.Drawing.Point(20, 45);
            this.lblDefaultDbConn.Name = "lblDefaultDbConn";
            this.lblDefaultDbConn.Size = new System.Drawing.Size(149, 20);
            this.lblDefaultDbConn.TabIndex = 0;
            this.lblDefaultDbConn.Text = "默认数据库连接字符串";
            // 
            // txtDefaultDbConn
            // 
            this.txtDefaultDbConn.Location = new System.Drawing.Point(177, 42);
            this.txtDefaultDbConn.Name = "txtDefaultDbConn";
            this.txtDefaultDbConn.Size = new System.Drawing.Size(714, 26);
            this.txtDefaultDbConn.TabIndex = 1;
            // 
            // lblDefaultDbType
            // 
            this.lblDefaultDbType.AutoSize = true;
            this.lblDefaultDbType.Location = new System.Drawing.Point(24, 98);
            this.lblDefaultDbType.Name = "lblDefaultDbType";
            this.lblDefaultDbType.Size = new System.Drawing.Size(107, 20);
            this.lblDefaultDbType.TabIndex = 2;
            this.lblDefaultDbType.Text = "默认数据库类型";
            // 
            // cbDefaultDbType
            // 
            this.cbDefaultDbType.FormattingEnabled = true;
            this.cbDefaultDbType.Location = new System.Drawing.Point(177, 95);
            this.cbDefaultDbType.Name = "cbDefaultDbType";
            this.cbDefaultDbType.Size = new System.Drawing.Size(133, 28);
            this.cbDefaultDbType.TabIndex = 3;
            // 
            // btnUpdateSetting
            // 
            this.btnUpdateSetting.Location = new System.Drawing.Point(28, 199);
            this.btnUpdateSetting.Name = "btnUpdateSetting";
            this.btnUpdateSetting.Size = new System.Drawing.Size(103, 33);
            this.btnUpdateSetting.TabIndex = 4;
            this.btnUpdateSetting.Text = "修改默认配置";
            this.btnUpdateSetting.UseVisualStyleBackColor = true;
            this.btnUpdateSetting.Click += new System.EventHandler(this.BtnUpdateSetting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 506);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainForm";
            this.Text = "DbTool";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tabControl.ResumeLayout(false);
            this.tabDbPage.ResumeLayout(false);
            this.tabDbPage.PerformLayout();
            this.tabModelPage.ResumeLayout(false);
            this.tabModelPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabCodePage.ResumeLayout(false);
            this.tabCodePage.PerformLayout();
            this.tabSettingPage.ResumeLayout(false);
            this.tabSettingPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblConnStatus;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDbPage;
        private System.Windows.Forms.CheckedListBox cbTables;
        private System.Windows.Forms.Button btnGenerateModel0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabModelPage;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView;
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
        private System.Windows.Forms.LinkLabel lnkExcelTemplate;
        private System.Windows.Forms.CheckBox cbGenDbDescription;
        private System.Windows.Forms.CheckBox cbGenField;
        private System.Windows.Forms.TabPage tabCodePage;
        private System.Windows.Forms.CheckBox cbGenCodeSqlDescription;
        private System.Windows.Forms.TreeView treeViewTable;
        private System.Windows.Forms.TextBox txtCodeModelSql;
        private System.Windows.Forms.Button btnImportModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldDesc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsKey;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanBeNull;
        private System.Windows.Forms.DataGridViewComboBoxColumn DataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn DefaultValue;
        private System.Windows.Forms.CheckBox cbGenDescriptionAttr;
        private System.Windows.Forms.TabPage tabSettingPage;
        private System.Windows.Forms.ComboBox cbDefaultDbType;
        private System.Windows.Forms.Label lblDefaultDbType;
        private System.Windows.Forms.TextBox txtDefaultDbConn;
        private System.Windows.Forms.Label lblDefaultDbConn;
        private System.Windows.Forms.Button btnUpdateSetting;
    }
}
