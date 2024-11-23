using DbLab.WpfApp.Controls;
using DbLab.WpfApp.Extensions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DbLab.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowModel _model;
        public MainWindow()
        {
            InitializeComponent();
            _model = new MainWindowModel();
            _model.Tabs.Add(new TabModel { TabName = "Главная", Control = typeof(DashboardControl) });
            _model.Tabs.Add(new TabModel { TabName = "хуй", Control = typeof(AnotherControl) });
            DataContext = _model;
        }

        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            if (_model.SelectedTab == null) return;

            _model.CurrentControl = Activator.CreateInstance(_model.SelectedTab.Control) as UserControl;
        }
    }

    public class MainWindowModel : PropertyChangedModel
    {
        private UserControl? _currentControl;
        public ObservableCollection<TabModel> Tabs { get; set; } = new ObservableCollection<TabModel>();

        public TabModel? SelectedTab { get; set; }

        public UserControl? CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged();
            }
        }
    }

    public class TabModel : PropertyChangedModel
    {
        public string TabName { get; set; }

        public Type Control { get; set; }
    }
}