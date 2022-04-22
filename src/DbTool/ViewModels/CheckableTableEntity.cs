// Copyright (c) Weihan Li. All rights reserved.
// Licensed under the MIT license.

using DbTool.Core.Entity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DbTool.ViewModels;

public class CheckableTableEntity : TableEntity, INotifyPropertyChanged
{
    private bool _checked;

    public bool Checked
    {
        get => _checked;
        set
        {
            _checked = value;
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
