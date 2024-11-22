using DbLab.WpfApp.Extensions;
using System.Windows;

namespace DbLab.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class MainWindowModel : PropertyChangedModel
    {

    }
}