using DbLab.WpfApp.Base;
using DbLab.WpfApp.Controls;
using System.Collections.ObjectModel;
using System.Windows;

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
            _model = new MainWindowModel();
            DataContext = _model;

            RegisterTabs();

            InitializeComponent();
        }

        public void RegisterTabs()
        {
            _model.Tabs.Add(new TabModel("Доходы", typeof(AccrualControl)));
        }

        private void Tab_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_model.SelectedTab == null) return;

            _model.CurrentControl = Activator.CreateInstance(_model.SelectedTab.Control) as UserControlBase;
        }
    }

    public class MainWindowModel : NotifyPropertyChangedItem
    {
        private UserControlBase? _currentControl;
        public ObservableCollection<TabModel> Tabs { get; set; } = new ObservableCollection<TabModel>();

        public TabModel? SelectedTab { get; set; }

        public UserControlBase? CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged();
            }
        }
    }

    public class TabModel : NotifyPropertyChangedItem
    {
        public TabModel(string tabName, Type control)
        {
            TabName = tabName;
            Control = control;
        }

        public string TabName { get; set; }

        public Type Control { get; set; }
    }
}