using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using DbTool.Core;
using DbTool.Core.Entity;
using DbTool.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using WeihanLi.Common;
using WeihanLi.Extensions;
using WeihanLi.Npoi;

namespace DbTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IStringLocalizer<MainWindow> _localizer;
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly SettingsViewModel _settings;

        private readonly IModelCodeGenerator _modelCodeGenerator;

        public MainWindow(
            IStringLocalizer<MainWindow> localizer,
            DbProviderFactory dbProviderFactory,
            SettingsViewModel settings,
            IModelCodeGenerator modelCodeGenerator)
        {
            InitializeComponent();

            _localizer = localizer;
            _settings = settings;
            _dbProviderFactory = dbProviderFactory;
            _modelCodeGenerator = modelCodeGenerator;

            InitDataBinding();
        }

        private void InitDataBinding()
        {
            DataContext = _settings;

            DefaultDbType.ItemsSource = _dbProviderFactory.SupportedDbTypes;
            DefaultDbType.SelectedItem = _settings.DefaultDbType;

            var supportedCultures = _settings.SupportedCultures
                .Select(c => new CultureInfo(c))
                .ToArray();
            DefaultCulture.ItemsSource = supportedCultures;
            DefaultCulture.SelectedItem = supportedCultures.FirstOrDefault(c => c.Name == _settings.DefaultCulture);

            CbGenPrivateFields.IsChecked = _settings.GeneratePrivateField;
            CbGenDataAnnotation.IsChecked = _settings.GenerateDataAnnotation;
            CodeGenDbDescCheckBox.IsChecked = _settings.GenerateDbDescription;
            ModelFirstGenDesc.IsChecked = _settings.GenerateDbDescription;

            var exporters = DependencyResolver.Current.ResolveServices<IDbDocExporter>();
            foreach (var exporter in exporters)
            {
                var exportButton = new Button()
                {
                    Content = $"{_localizer["Export"]}{exporter.ExportType}",
                    Tag = exporter,
                    MaxWidth = 160,
                    Margin = new Thickness(4)
                };
                exportButton.Click += ExportButton_Click;
                DbDocExportersPanel.Children.Add(exportButton);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckedTables.SelectedItems.Count == 0)
            {
                MessageBox.Show(_localizer["ChooseTables"], _localizer["Tip"]);
                return;
            }

            if (sender is Button btnExport && btnExport.Tag is IDbDocExporter exporter)
            {
                var tables = new List<TableEntity>();
                foreach (var item in CheckedTables.SelectedItems)
                {
                    if (item is TableEntity table)
                    {
                        tables.Add(table);
                    }
                }
                if (tables.Count == 0) return;
                //
                var dir = ChooseFolder();
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                try
                {
                    var exportBytes = exporter.Export(tables.ToArray());
                    if (null != exportBytes && exportBytes.Length > 0)
                    {
                        var fileName = tables.Count > 1
                            ? _dbHelper.DatabaseName
                            : tables[0].TableName.TrimTableName();
                        fileName = $"{fileName}.{exporter.FileExtension.TrimStart('.')}";
                        var path = Path.Combine(dir, fileName);
                        File.WriteAllBytes(path, exportBytes);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Export Error");
                }
            }
        }

        private void BtnSaveSettings_OnClick(object sender, RoutedEventArgs e)
        {
            var updated = false;
            if (null != DefaultDbType.SelectedItem && _settings.DefaultDbType != DefaultDbType.SelectedItem.ToString())
            {
                _settings.DefaultDbType = DefaultDbType.SelectedItem?.ToString();
                updated = true;
            }
            if (_settings.DefaultConnectionString != TxtDefaultConnStr.Text)
            {
                _settings.DefaultConnectionString = TxtDefaultConnStr.Text.Trim();
                updated = true;
            }
            if (DefaultCulture.SelectedItem is CultureInfo culture && culture.Name != _settings.DefaultCulture)
            {
                _settings.DefaultCulture = culture.Name;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                updated = true;
            }
            if (updated)
            {
                MessageBox.Show(_localizer["Success"], _localizer["Tip"]);
            }
        }

        private void BtnChooseModel_OnClick(object sender, RoutedEventArgs e)
        {
            var ofg = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = true,
                Filter = "C# File(*.cs)|*.cs"
            };
            if (ofg.ShowDialog() == true)
            {
                if (ofg.FileNames.Any(f => !f.EndsWith(".cs")))
                {
                    MessageBox.Show(_localizer["UnsupportedFileType", ofg.FileNames.First(f => !f.EndsWith(".cs"))]);
                    return;
                }

                try
                {
                    var dbProvider = _dbProviderFactory.GetDbProvider(_settings.DefaultDbType);
                    var tables = dbProvider.GetTableEntityFromSourceCode(ofg.FileNames);
                    if (tables == null)
                    {
                        MessageBox.Show(_localizer["NoModelFound"]);
                    }
                    else
                    {
                        TxtCodeGenSql.Clear();
                        foreach (var table in tables)
                        {
                            var tableSql = dbProvider.GenerateSqlStatement(table, CodeGenDbDescCheckBox.IsChecked == true);
                            TxtCodeGenSql.AppendText(tableSql);
                            TxtCodeGenSql.AppendText(Environment.NewLine);
                        }

                        CodeGenTableTreeView.ItemsSource = tables;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void BtnGenerateSql_OnClick(object sender, RoutedEventArgs e)
        {
            if (ModelDataGrid.Items.Count > 0)
            {
                if (string.IsNullOrEmpty(TxtModelFirstTableName.Text))
                {
                    return;
                }

                var table = new TableEntity()
                {
                    TableName = TxtModelFirstTableName.Text,
                    TableDescription = TxtModelFirstTableDesc.Text,
                };
                foreach (var item in ModelDataGrid.Items)
                {
                    if (item is ColumnEntity column && !string.IsNullOrEmpty(column.ColumnName))
                    {
                        table.Columns.Add(column);
                    }
                }
                var dbProvider = _dbProviderFactory.GetDbProvider(_settings.DefaultDbType);
                var sql = dbProvider.GenerateSqlStatement(table, ModelFirstGenDesc.IsChecked == true);
                TxtModelFirstGeneratedSql.Text = sql;
                Clipboard.SetText(sql);
                MessageBox.Show(_localizer["SqlCopiedToClipboard"], _localizer["Tip"]);
            }
        }

        private void BtnImportModelExcel_OnClick(object sender, RoutedEventArgs e)
        {
            var ofg = new OpenFileDialog
            {
                Multiselect = false,
                CheckFileExists = true,
                Filter = "Excel file(*.xlsx)|*.xlsx|Excel97-2003(*.xls)|*.xls"
            };
            if (ofg.ShowDialog() == true)
            {
                try
                {
                    var workbook = ExcelHelper.LoadExcel(ofg.FileName);
                    if (0 == workbook.NumberOfSheets)
                    {
                        return;
                    }
                    var tableCount = workbook.NumberOfSheets;
                    var sql = string.Empty;
                    var dbProvider = _dbProviderFactory.GetDbProvider(_settings.DefaultDbType);
                    if (tableCount == 1)
                    {
                        var sheet = workbook.GetSheetAt(0);
                        var table = ExactTableFromExcel(sheet, dbProvider);

                        TxtModelFirstTableName.Text = table.TableName;
                        TxtModelFirstTableDesc.Text = table.TableDescription;
                        ModelDataGrid.ItemsSource = table.Columns;

                        sql = dbProvider.GenerateSqlStatement(table, ModelFirstGenDesc.IsChecked == true);
                    }
                    else
                    {
                        var sbSqlText = new StringBuilder();
                        for (var i = 0; i < tableCount; i++)
                        {
                            var sheet = workbook.GetSheetAt(i);
                            var table = ExactTableFromExcel(sheet, dbProvider);
                            if (i > 0)
                            {
                                sbSqlText.AppendLine();
                            }
                            else
                            {
                                TxtModelFirstTableName.Text = table.TableName;
                                TxtModelFirstTableDesc.Text = table.TableDescription;
                                ModelDataGrid.ItemsSource = table.Columns;
                            }
                            sbSqlText.AppendLine(dbProvider.GenerateSqlStatement(table, ModelFirstGenDesc.IsChecked == true));
                        }
                        sql = sbSqlText.ToString();
                    }
                    TxtModelFirstGeneratedSql.Text = sql;
                    Clipboard.SetText(sql);
                    MessageBox.Show(_localizer["SqlCopiedToClipboard"], _localizer["Tip"]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");
                }
            }
        }

        private TableEntity ExactTableFromExcel(ISheet sheet, IDbProvider dbProvider)
        {
            if (sheet == null)
                return null;

            var table = new TableEntity
            {
                TableName = sheet.SheetName.Trim()
            };

            foreach (var row in sheet.GetRowCollection())
            {
                if (null == row)
                {
                    continue;
                }
                if (row.RowNum == 0)
                {
                    table.TableDescription = row.Cells[0].StringCellValue;
                    continue;
                }
                if (row.RowNum > 1)
                {
                    var column = new ColumnEntity
                    {
                        ColumnName = row.GetCell(0)?.StringCellValue
                    };
                    if (string.IsNullOrWhiteSpace(column.ColumnName))
                    {
                        continue;
                    }
                    column.ColumnDescription = row.GetCell(1).StringCellValue;
                    column.IsPrimaryKey = row.GetCell(2).StringCellValue.Equals("Y");
                    column.IsNullable = row.GetCell(3).StringCellValue.Equals("Y");
                    column.DataType = row.GetCell(4).StringCellValue;

                    column.Size = string.IsNullOrEmpty(row.GetCell(5).ToString()) ? dbProvider.GetDefaultSizeForDbType(column.DataType) : Convert.ToUInt32(row.GetCell(5).ToString());

                    if (!string.IsNullOrWhiteSpace(row.GetCell(6)?.ToString()))
                    {
                        column.DefaultValue = row.GetCell(6).ToString();
                    }
                    table.Columns.Add(column);
                }
            }

            return table;
        }

        private void DownloadExcelTemplateLink_OnClick(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/59716856/net-core-3-1-process-startwww-website-com-not-working-in-wpf
            Process.Start(new ProcessStartInfo(_settings.ExcelTemplateDownloadLink)
            {
                UseShellExecute = true
            });
        }

        private DbHelper _dbHelper;

        private void BtnConnectDb_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtConnectionString.Text))
            {
                MessageBox.Show(_localizer["ConnectionStringCannotBeEmpty"]);
                return;
            }
            try
            {
                _dbHelper?.Dispose();

                _dbHelper = new DbHelper(TxtConnectionString.Text, _settings.DefaultDbType);

                var tables = _dbHelper.GetTablesInfo();
                CheckedTables.ItemsSource = tables
                    .OrderBy(x => x.TableName)
                    .ToArray();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error");
            }
        }

        private void BtnExportModel_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckedTables.SelectedItems.Count == 0)
            {
                MessageBox.Show(_localizer["ChooseTables"]);
                return;
            }
            var options = new ModelCodeGenerateOptions()
            {
                Namespace = TxtNamespace.Text.GetValueOrDefault("Models"),
                Prefix = TxtPrefix.Text,
                Suffix = TxtSuffix.Text,
                GenerateDataAnnotation = CbGenDataAnnotation.IsChecked == true,
                GeneratePrivateFields = CbGenPrivateFields.IsChecked == true,
            };
            var dir = ChooseFolder();
            if (string.IsNullOrEmpty(dir))
            {
                return;
            }
            foreach (var item in CheckedTables.SelectedItems)
            {
                if (item is TableEntity table)
                {
                    var modelCode = _modelCodeGenerator.GenerateModelCode(table, options, _settings.DefaultDbType);
                    var path = Path.Combine(dir, $"{table.TableName.TrimTableName()}.cs");
                    File.WriteAllText(path, modelCode, Encoding.UTF8);
                }
            }
            // open dir
            Process.Start("Explorer.exe", dir);
        }

        private string ChooseFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = _localizer["ChooseDirTip"],
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return null;
        }

        protected override void OnClosed(EventArgs e)
        {
            _dbHelper?.Dispose();
            base.OnClosed(e);
        }

        private void CheckTableToggled(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is TableEntity table)
            {
                if (checkBox.IsChecked == true)
                {
                    if (CheckedTables.SelectedItems.Contains(table) == false)
                    {
                        CheckedTables.SelectedItems.Add(table);
                    }

                    if (table.TableName != CurrentCheckedTableName.Text)
                    {
                        CurrentCheckedTableName.Text = table.TableName;
                        if (table.Columns.Count == 0)
                        {
                            table.Columns = _dbHelper.GetColumnsInfo(table.TableName);
                        }
                        ColumnListView.ItemsSource = table.Columns;
                    }
                }
                else
                {
                    if (CheckedTables.SelectedItems.Contains(table))
                    {
                        CheckedTables.SelectedItems.Remove(table);
                    }
                }
            }
        }
    }
}
