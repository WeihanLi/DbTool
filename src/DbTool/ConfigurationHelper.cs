using System;
using System.Collections.Specialized;
using System.Configuration;
using WeihanLi.Extensions;

namespace DbTool
{
    /// <summary>
    /// Helper for ConfigurationManager
    /// https://github.com/WeihanLi/WeihanLi.Common/blob/dev/src/WeihanLi.Common/Helpers/ConfigurationHelper.cs
    /// </summary>
    public static class ConfigurationHelper
    {
        private static NameValueCollection _appSettings;

        static ConfigurationHelper()
        {
            _appSettings = ConfigurationManager.AppSettings;
        }

        /// <summary>
        /// 获取配置文件中AppSetting节点的值
        /// </summary>
        /// <param name="key">设置的键值</param>
        /// <returns>键值对应的值</returns>
        public static string AppSetting(string key) => _appSettings[key] ?? throw new ArgumentNullException(nameof(key));

        /// <summary>
        /// 获取配置文件中AppSetting节点的值
        /// </summary>
        /// <param name="key">设置的键值</param>
        /// <returns>键值对应的值</returns>
        public static T AppSetting<T>(string key) => _appSettings[key].StringToType<T>();

        public static T AppSetting<T>(string key, T defaultValue) => _appSettings[key].StringToType(defaultValue);

        public static T AppSetting<T>(string key, Func<T> defaultValueFactory) => _appSettings[key].StringToType(defaultValueFactory());

        public static bool AddAppSetting<T>(string key, T value) => AddAppSetting(key, value.ToJsonOrString());

        public static bool AddAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Minimal);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
            _appSettings = ConfigurationManager.AppSettings;
            return true;
        }

        public static bool UpdateAppSetting<T>(string key, T value) => UpdateAppSetting(key, value.ToJsonOrString());

        public static bool UpdateAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Minimal);
            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
            _appSettings = ConfigurationManager.AppSettings;
            return true;
        }

        /// <summary>
        /// 获取配置文件中ConnectionStrings节点的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>键值对应的连接字符串值</returns>
        public static string ConnectionString(string key) => ConfigurationManager.ConnectionStrings[key].ConnectionString;
    }
}
