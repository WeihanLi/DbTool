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
using WeihanLi.Common;
using WeihanLi.Extensions;

namespace DbTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IStringLocalizer<MainWindow> _localizer;
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly IDbHelperFactory _dbHelperFactory;
        private readonly SettingsViewModel _settings;

        private readonly IModelNameConverter _modelNameConverter;

        public MainWindow(
            IStringLocalizer<MainWindow> localizer,
            DbProviderFactory dbProviderFactory,
            IDbHelperFactory dbHelperFactory,
            SettingsViewModel settings,
            IModelNameConverter modelNameConverter)
        {
            InitializeComponent();

            _localizer = localizer;
            _settings = settings;
            _dbProviderFactory = dbProviderFactory;
            _dbHelperFactory = dbHelperFactory;
            _modelNameConverter = modelNameConverter;

            InitDataBinding();
        }

        private void InitDataBinding()
        {
            DataContext = _settings;

            DbFirst_DbType.ItemsSource = _dbProviderFactory.SupportedDbTypes;
            DbFirst_DbType.SelectedItem = _settings.DefaultDbType;

            DefaultDbType.ItemsSource = _dbProviderFactory.SupportedDbTypes;
            DefaultDbType.SelectedItem = _settings.DefaultDbType;

            var supportedCultures = _settings.SupportedCultures
                .Select(c => new CultureInfo(c))
                .ToArray();
            DefaultCulture.ItemsSource = supportedCultures;
            DefaultCulture.SelectedItem = supportedCultures.FirstOrDefault(c => c.Name == _settings.DefaultCulture);

            CbGenPrivateFields.IsChecked = _settings.GeneratePrivateField;
            CbGenDataAnnotation.IsChecked = _settings.GenerateDataAnnotation;
            CbApplyNameConverter.IsChecked = _settings.ApplyNameConverter;
            CodeGenDbDescCheckBox.IsChecked = _settings.GenerateDbDescription;
            ModelFirstGenDesc.IsChecked = _settings.GenerateDbDescription;

            var codeGenerators = DependencyResolver.ResolveServices<IModelCodeGenerator>();
            foreach (var generator in codeGenerators)
            {
                var button = new Button()
                {
                    Content = $"{_localizer["Export"]} {generator.CodeType} Code",
                    Tag = generator,
                    MaxWidth = 180,
                    Margin = new Thickness(4)
                };
                button.Click += ExportModel_Click;
                ModelCodeGeneratorsPanel.Children.Add(button);
            }
            var exporters = DependencyResolver.ResolveServices<IDbDocExporter>();
            foreach (var exporter in exporters)
            {
                var exportButton = new Button()
                {
                    Content = $"{_localizer["Export"]}{exporter.ExportType}",
                    Tag = exporter,
                    MaxWidth = 180,
                    Margin = new Thickness(4)
                };
                exportButton.Click += ExportButton_Click;
                DbDocExportersPanel.Children.Add(exportButton);
            }
            var importers = DependencyResolver.ResolveServices<IDbDocImporter>();
            foreach (var importer in importers)
            {
                var importButton = new Button()
                {
                    Content = $"{_localizer["ChooseFile"]}({importer.ImportType})",
                    Tag = importer,
                    MaxWidth = 160,
                    Margin = new Thickness(4, 0, 4, 0)
                };
                importButton.Click += ImportButton_Click;
                DbDocImportersPannel.Children.Add(importButton);
            }
            var codeExtractors = DependencyResolver.ResolveServices<IModelCodeExtractor>();
            foreach (var extractor in codeExtractors)
            {
                var button = new Button()
                {
                    Content = $"{_localizer["ChooseModelFiles"]}({extractor.CodeType})",
                    Margin = new Thickness(8),
                    Tag = extractor,
                    MaxWidth = 300
                };
                button.Click += ChooseModel_Click;
                CodeExtractorsPannel.Children.Add(button);
            }
        }

        private void ExportModel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: IModelCodeGenerator codeGenerator })
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
                    ApplyNameConverter = CbApplyNameConverter.IsChecked == true,
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
                        var modelCode = codeGenerator.GenerateModelCode(table, options, _dbProviderFactory.GetDbProvider(_dbHelper?.DbType ?? _settings.DefaultDbType));
                        var path = Path.Combine(dir, $"{(_settings.ApplyNameConverter ? _modelNameConverter.ConvertTableToModel(table.TableName ?? "") : table.TableName)}{codeGenerator.FileExtension}");
                        File.WriteAllText(path, modelCode, Encoding.UTF8);
                    }
                }
                // open dir
                Process.Start("Explorer.exe", dir);
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (_dbHelper is null)
            {
                MessageBox.Show(_localizer["DbNotConnected"]);
                return;
            }
            if (CheckedTables.SelectedItems.Count == 0)
            {
                MessageBox.Show(_localizer["ChooseTables"], _localizer["Tip"]);
                return;
            }

            if (sender is Button { Tag: IDbDocExporter exporter })
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

                var dir = ChooseFolder();
                if (string.IsNullOrEmpty(dir))
                {
                    return;
                }
                try
                {
                    var exportBytes = exporter.Export(tables.ToArray(), _dbProviderFactory.GetDbProvider(_dbHelper.DbType));
                    if (exportBytes.Length > 0)
                    {
                        var fileName = tables.Count > 1
                            ? _dbHelper.DatabaseName
                            :
                                _settings.ApplyNameConverter
                                    ? _modelNameConverter.ConvertTableToModel(tables[0].TableName)
                                    : tables[0].TableName
                            ;
                        fileName = $"{fileName}.{exporter.FileExtension.TrimStart('.')}";
                        var path = Path.Combine(dir, fileName);
                        File.WriteAllBytes(path, exportBytes);
                        // open dir
                        Process.Start("Explorer.exe", dir);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Export Error");
                }
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: IDbDocImporter importer })
            {
                var ofg = new OpenFileDialog
                {
                    Multiselect = false,
                    CheckFileExists = true,
                    Filter = importer.SupportedFileExtensions.ToFileChooseFilter()
                };
                if (ofg.ShowDialog() == true)
                {
                    try
                    {
                        var dbProvider = _dbProviderFactory.GetDbProvider(_settings.DefaultDbType);
                        var tables = importer.Import(ofg.FileName, dbProvider);

                        if (tables.Length > 0)
                        {
                            TxtModelFirstTableName.Text = tables[0].TableName;
                            TxtModelFirstTableDesc.Text = tables[0].TableDescription;
                            ModelDataGrid.ItemsSource = tables[0].Columns;

                            var sbSqlText = new StringBuilder();
                            foreach (var table in tables)
                            {
                                sbSqlText.AppendLine(dbProvider.GenerateSqlStatement(table, ModelFirstGenDesc.IsChecked == true));
                            }
                            var sql = sbSqlText.ToString();
                            TxtModelFirstGeneratedSql.Text = sql;
                            Clipboard.SetText(sql);
                            MessageBox.Show(_localizer["SqlCopiedToClipboard"], _localizer["Tip"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                }
            }
        }

        private async void ChooseModel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: IModelCodeExtractor extractor })
            {
                var ofg = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Multiselect = true,
                    Filter = extractor.SupportedFileExtensions.ToFileChooseFilter()
                };
                if (ofg.ShowDialog() == true)
                {
                    try
                    {
                        var dbProvider = _dbProviderFactory.GetDbProvider(_settings.DefaultDbType);
                        var tables = await extractor.GetTablesFromSourceFiles(dbProvider, ofg.FileNames);
                        if (tables.Count == 0)
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
        }

        private void BtnSaveSettings_OnClick(object sender, RoutedEventArgs e)
        {
            if (null != DefaultDbType.SelectedItem && _settings.DefaultDbType != DefaultDbType.SelectedItem.ToString())
            {
                _settings.DefaultDbType = DefaultDbType.SelectedItem?.ToString() ?? string.Empty;
            }
            if (DefaultCulture.SelectedItem is CultureInfo culture && culture.Name != _settings.DefaultCulture)
            {
                _settings.DefaultCulture = culture.Name;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            if (_settings.DefaultConnectionString != TxtDefaultConnStr.Text.Trim())
            {
                _settings.DefaultConnectionString = TxtDefaultConnStr.Text.Trim();
            }
            MessageBox.Show(_localizer["Success"], _localizer["Tip"]);
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

        private void DownloadExcelTemplateLink_OnClick(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/59716856/net-core-3-1-process-startwww-website-com-not-working-in-wpf
            Process.Start(new ProcessStartInfo(_settings.ExcelTemplateDownloadLink)
            {
                UseShellExecute = true
            });
        }

        private IDbHelper? _dbHelper;

        private async void BtnConnectDb_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtConnectionString.Text))
            {
                MessageBox.Show(_localizer["ConnectionStringCannotBeEmpty"]);
                return;
            }
            try
            {
                var connStr = TxtConnectionString.Text;
                var currentDbProvider = _dbProviderFactory.GetDbProvider(DbFirst_DbType.SelectedItem?.ToString() ?? _settings.DefaultDbType);
                _dbHelper = _dbHelperFactory.GetDbHelper(currentDbProvider, connStr);

                var tables = await _dbHelper.GetTablesInfoAsync();
                CheckedTables.Dispatcher.Invoke(() =>
                {
                    CheckedTables.ItemsSource = tables
                        .OrderBy(x => x.GetFullTableName())
                        .ToArray();
                });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error");
            }
        }

        private string? ChooseFolder()
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
            if (_dbHelper is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnClosed(e);
        }

        private async void CheckTableToggled(object sender, RoutedEventArgs e)
        {
            if (_dbHelper is null)
            {
                MessageBox.Show(_localizer["DbNotConnected"]);
                return;
            }
            if (sender is CheckBox { DataContext: TableEntity table } checkBox)
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
                            table.Columns = await _dbHelper.GetColumnsInfoAsync(table.TableName);
                            ColumnListView.Dispatcher.Invoke(() =>
                            {
                                ColumnListView.ItemsSource = table.Columns;
                            });
                        }
                        else
                        {
                            ColumnListView.ItemsSource = table.Columns;
                        }
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
