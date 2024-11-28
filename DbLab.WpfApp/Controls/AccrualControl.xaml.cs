using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;
using DbLab.WpfApp.Windows;
using System.Collections.ObjectModel;
using DbLab.DalPg.Entities.View;

namespace DbLab.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for AnotherControl.xaml
    /// </summary>
    public partial class AccrualControl : UserControlBase
    {
        private readonly AccrualControlModel _model;

        public AccrualControl()
        {
            _model = new AccrualControlModel();
            DataContext = _model;
            InitializeComponent();
            GetDataAsync();
        }

        private async void GetDataAsync()
        {
            var lst = await new ViewManager().ReadViewEntity<VAccrualEntity>();
            foreach (var ent in lst)
                _model.AccrualList.Add(ent);
        }
    }

    public class AccrualControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<VAccrualEntity> AccrualList { get; set; } = new ObservableCollection<VAccrualEntity>();

        public UiCommand AddChargeCommand => new(AddCharge);

        private async void AddCharge(object? obj)
        {
            var window = new AddAccrualWindow();
            window.ShowDialog();
            if (window.DialogResult ?? false)
            {
                var business = window.BusinessEntity;
                var id = await new AccrualManager().Write(business);
                RefreshView();
            }
        }

        private async void RefreshView()
        {
            var lst = await new ViewManager().ReadViewEntity<VAccrualEntity>();
            AccrualList.Clear();
            foreach (var ent in lst)
                AccrualList.Add(ent);
        }
    }
}
