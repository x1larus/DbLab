using DbLab.WpfApp.Base;
using DbLab.WpfApp.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using DbLab.DalPg.Base;
using Npgsql;

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
            _model.Tabs.Add(new TabModel("Начисления", typeof(AccrualControl)));
            _model.Tabs.Add(new TabModel("Люди", typeof(ParticipantsControl)));
            _model.Tabs.Add(new TabModel("Категории", typeof(CategoryControl)));
        }

        private void Tab_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_model.SelectedTab == null) return;

            _model.CurrentControl = Activator.CreateInstance(_model.SelectedTab.Control) as UserControlBase;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ClearDb();
        }

        private async void ClearDb()
        {
            await using var conn = await DbHelper.CreateOpenedConnectionAsync();
            using var comd = new NpgsqlCommand("call public.p$clear_all()", conn);
            comd.ExecuteNonQuery();
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