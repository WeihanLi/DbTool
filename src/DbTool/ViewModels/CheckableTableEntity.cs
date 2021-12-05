using System.ComponentModel;
using System.Runtime.CompilerServices;
using DbTool.Core.Entity;

namespace DbTool.ViewModels
{
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
}
