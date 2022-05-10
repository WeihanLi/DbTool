﻿// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using System;

namespace DbTool.ViewModels;

public class SettingsViewModel
{
    private string _defaultDbType;
    private string _defaultConnectionString;
    private bool _generatePrivateField;
    private bool _generateDataAnnotation;
    private string _excelTemplateDownloadLink;
    private string _defaultCulture;
    private bool _generateDbDescription;
    private bool _globalUsingEnabled;
    private bool _nullableReferenceTypesEnabled;
    private bool _fileScopedNamespaceEnabled;
    private bool _isLoad;

    public SettingsViewModel()
    {
        _excelTemplateDownloadLink =
            ConfigurationHelper.AppSetting(ConfigurationConstants.ExcelTemplateDownloadLink);
        _defaultConnectionString = ConfigurationHelper.AppSetting(ConfigurationConstants.DefaultConnectionString);
        _defaultDbType = ConfigurationHelper.AppSetting(ConfigurationConstants.DbType);
        _generateDataAnnotation =
            ConfigurationHelper.AppSetting<bool>(ConfigurationConstants.GenerateDataAnnotation);
        _generatePrivateField = ConfigurationHelper.AppSetting<bool>(ConfigurationConstants.GeneratePrivateField);
        _generateDbDescription = ConfigurationHelper.AppSetting<bool>(ConfigurationConstants.GenerateDbDescription);
        _globalUsingEnabled = ConfigurationHelper.AppSetting<bool>(nameof(GlobalUsingEnabled));
        _nullableReferenceTypesEnabled = ConfigurationHelper.AppSetting<bool>(nameof(NullableReferenceTypesEnabled));
        _fileScopedNamespaceEnabled = ConfigurationHelper.AppSetting<bool>(nameof(FileScopedNamespaceEnabled));
        _defaultCulture = ConfigurationHelper.AppSetting(nameof(DefaultCulture));
        SupportedCultures = ConfigurationHelper.AppSetting(nameof(SupportedCultures))
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        IsLoad = false;//TODO: 合适的时机开启
        ConnectionString = _defaultConnectionString;
    }

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

    public bool GenerateDbDescription
    {
        get => _generateDbDescription;
        set
        {
            _generateDbDescription = value;
            ConfigurationHelper.UpdateAppSetting(nameof(GenerateDbDescription), value);
        }
    }

    public bool GlobalUsingEnabled
    {
        get => _globalUsingEnabled;
        set
        {
            _globalUsingEnabled = value;
            ConfigurationHelper.UpdateAppSetting(nameof(GlobalUsingEnabled), value);
        }
    }

    public bool NullableReferenceTypesEnabled
    {
        get => _nullableReferenceTypesEnabled;
        set
        {
            _nullableReferenceTypesEnabled = value;
            ConfigurationHelper.UpdateAppSetting(nameof(NullableReferenceTypesEnabled), value);
        }
    }

    public bool FileScopedNamespaceEnabled
    {
        get => _fileScopedNamespaceEnabled;
        set
        {
            _fileScopedNamespaceEnabled = value;
            ConfigurationHelper.UpdateAppSetting(nameof(FileScopedNamespaceEnabled), value);
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
    public bool IsLoad
    { 
        get => _isLoad;
        set 
        {
            _isLoad = value;
            ConfigurationHelper.UpdateAppSetting(nameof(IsLoad), value);
        }
    }
}
