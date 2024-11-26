using System.Windows;
using System.Windows.Controls;

namespace DbLab.WpfApp.Base
{
    public abstract class UserControlBase : UserControl
    {
        protected static void ExecuteInUiThread(Action func)
        {
            Application.Current.Dispatcher.BeginInvoke(func);
        }

        protected static void ExecuteAsync(Action func)
        {
            Task.Run(func);
        }
    }
}
