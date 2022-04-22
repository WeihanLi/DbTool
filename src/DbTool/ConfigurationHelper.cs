// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Specialized;
using System.Configuration;
using WeihanLi.Extensions;

namespace DbTool;

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
    public static string AppSetting(string key) => _appSettings[key] ?? string.Empty;

    /// <summary>
    /// 获取配置文件中AppSetting节点的值
    /// </summary>
    /// <param name="key">设置的键值</param>
    /// <returns>键值对应的值</returns>
    public static T AppSetting<T>(string key) => AppSetting(key).StringToType<T>();

    /// <summary>
    /// 更新 AppSetting 配置
    /// </summary>
    /// <typeparam name="T">value type</typeparam>
    /// <param name="key">app setting key</param>
    /// <param name="value">app setting value</param>
    public static void UpdateAppSetting<T>(string key, T value) => UpdateAppSetting(key, value.ToJsonOrString());

    /// <summary>
    /// 更新 AppSetting 配置
    /// </summary>
    /// <param name="key">app setting key</param>
    /// <param name="value">app setting value</param>
    public static void UpdateAppSetting(string key, string value)
    {
        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings.Remove(key);
        config.AppSettings.Settings.Add(key, value);
        config.Save(ConfigurationSaveMode.Minimal);
        // Force a reload of a changed section.
        ConfigurationManager.RefreshSection("appSettings");
        _appSettings = ConfigurationManager.AppSettings;
    }
}
