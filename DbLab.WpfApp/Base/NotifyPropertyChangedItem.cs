using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DbLab.WpfApp.Base
{
    public class NotifyPropertyChangedItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
