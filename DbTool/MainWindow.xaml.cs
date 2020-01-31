using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using DbTool.Core;
using DbTool.ViewModels;
using Microsoft.Extensions.Localization;
using Microsoft.Win32;
using WeihanLi.Extensions;

namespace DbTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SettingsViewModel _settings;
        private readonly IStringLocalizer<MainWindow> _localizer;

        public MainWindow(IStringLocalizer<MainWindow> localizer, SettingsViewModel settings, IEnumerable<IDbProvider> dbProviders)
        {
            InitializeComponent();

            _localizer = localizer;
            _settings = settings;

            InitDataBinding(dbProviders.Select(p => p.DbType));
        }

        private void InitDataBinding(IEnumerable<string> dbProviders)
        {
            DataContext = _settings;

            DefaultDbType.ItemsSource = dbProviders;
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
                    var tables = Utils.GetTableEntityFromSourceCode(ofg.FileNames);
                    if (tables == null)
                    {
                        MessageBox.Show("没有找到 Model");
                    }
                    else
                    {
                        TxtCodeGenSql.Clear();
                        foreach (var table in tables)
                        {
                            TxtCodeGenSql.AppendText(table.GenerateSqlStatement(CodeGenDbDescCheckBox.IsChecked == true, _settings.DefaultDbType));
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
