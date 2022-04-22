// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core;
using Xunit;

namespace DbTool.Test;

public class NameConverterTest
{
    private readonly IModelNameConverter _converter;

    public NameConverterTest()
    {
        _converter = new ModelNameConverter();
    }

    [Theory]
    [InlineData("tabNotice", "Notice")]
    [InlineData("tab_Notice", "Notice")]
    [InlineData("tblNotice", "Notice")]
    [InlineData("tbl_Notice", "Notice")]
    [InlineData("tab_notice", "Notice")]
    [InlineData("tab-notice", "Notice")]
    [InlineData("project", "Project")]
    [InlineData("user-profile", "UserProfile")]
    [InlineData("userProfile", "UserProfile")]
    [InlineData("user_profile", "UserProfile")]
    public void TableToModelTest(string tableName, string expectedModelName)
    {
        Assert.Equal(expectedModelName, _converter.ConvertTableToModel(tableName));
    }

    [Theory]
    [InlineData("NoticeModel", "Notice")]
    [InlineData("NoticeEntity", "Notice")]
    public void ModelToTableTest(string modelName, string expectedTableName)
    {
        Assert.Equal(expectedTableName, _converter.ConvertModelToTable(modelName));
    }
}
