using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using DbTool.Core;
using DbTool.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Win32;

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

        public MainWindow(
            IStringLocalizer<MainWindow> localizer,
            DbProviderFactory dbProviderFactory,
            SettingsViewModel settings)
        {
            InitializeComponent();

            _localizer = localizer;
            _settings = settings;
            _dbProviderFactory = dbProviderFactory;

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
                    MessageBox.Show("不支持所选文件类型，只可以选择C#文件(*.cs)");
                    return;
                }

                try
                {
                    var dbProvider = _dbProviderFactory.GetDbProvider(_settings.DefaultDbType);
                    var tables = dbProvider.GetTableEntityFromSourceCode(ofg.FileNames);
                    if (tables == null)
                    {
                        MessageBox.Show("没有找到 Model");
                    }
                    else
                    {
                        TxtCodeGenSql.Clear();
                        foreach (var table in tables)
                        {
                            var tableSql = dbProvider.GenerateSqlStatement(table, CodeGenDbDescCheckBox.IsChecked == true);
                            TxtCodeGenSql.AppendText(tableSql);
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
}
