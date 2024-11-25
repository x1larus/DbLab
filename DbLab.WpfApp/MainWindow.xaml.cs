﻿using DbLab.WpfApp.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DbLab.WpfApp.Base;
using DbLab.DalPgBase;

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
            var connectionString = "Host=79.137.204.140;Port=5432;Username=db_lab_app;Password=db_lab_app;Database=db_lab";
            DbManager.InitializeDb(connectionString);
            InitializeComponent();
            _model = new MainWindowModel();
            _model.Tabs.Add(new TabModel { TabName = "Главная", Control = typeof(DashboardControl) });
            _model.Tabs.Add(new TabModel { TabName = "хуй", Control = typeof(IncomeControl) });
            DataContext = _model;
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
        public string TabName { get; set; }

        public Type Control { get; set; }
    }
}