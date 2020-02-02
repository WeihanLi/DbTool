namespace DbTool.ViewModels
{
    public class SettingsViewModel
    {
        private string _defaultDbType;
        private string _defaultConnectionString;
        private bool _generatePrivateField;
        private bool _generateDataAnnotation;
        private string _excelTemplateDownloadLink;
        private string _defaultCulture;

        public string DefaultDbType
        {
            get => _defaultDbType;
            set
            {
                _defaultDbType = value;
                ConfigurationHelper.UpdateAppSetting(ConfigurationConstants.DbType, value);
            }
        }

        public string DefaultConnectionString
        {
            get => _defaultConnectionString;
            set
            {
                _defaultConnectionString = value;
                ConfigurationHelper.UpdateAppSetting(ConfigurationConstants.DefaultConnectionString, value);
            }
        }

        public string ConnectionString { get; set; }

        public bool GeneratePrivateField
        {
            get => _generatePrivateField;
            set
            {
                _generatePrivateField = value;
                ConfigurationHelper.UpdateAppSetting(ConfigurationConstants.GeneratePrivateField, value);
            }
        }

        public bool GenerateDataAnnotation
        {
            get => _generateDataAnnotation;
            set
            {
                _generateDataAnnotation = value;
                ConfigurationHelper.UpdateAppSetting(ConfigurationConstants.GenerateDataAnnotation, value);
            }
        }

        public string ExcelTemplateDownloadLink
        {
            get => _excelTemplateDownloadLink;
            set
            {
                _excelTemplateDownloadLink = value;
                ConfigurationHelper.UpdateAppSetting(ConfigurationConstants.ExcelTemplateDownloadLink, value);
            }
        }

        public string DefaultCulture
        {
            get => _defaultCulture;
            set
            {
                _defaultCulture = value;
                ConfigurationHelper.UpdateAppSetting(nameof(DefaultCulture), value);
            }
        }

        public string[] SupportedCultures { get; set; }
    }
}
