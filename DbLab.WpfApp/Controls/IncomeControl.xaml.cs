using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;
using DbLab.WpfApp.Windows;
using System.Collections.ObjectModel;

namespace DbLab.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for AnotherControl.xaml
    /// </summary>
    public partial class IncomeControl : UserControlBase
    {
        private readonly IncomeControlModel _model;

        public IncomeControl()
        {
            _model = new IncomeControlModel();
            DataContext = _model;
            InitializeComponent();
            GetIncomeDataAsync();
        }

        private async void GetIncomeDataAsync()
        {
            var lst = await new IncomeManager().ReadAll();
            foreach (var ent in lst)
                _model.IncomeRows.Add(new IncomeRowModel(ent));
        }
    }

    public class IncomeControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<IncomeRowModel> IncomeRows { get; set; } = [];

        public UiCommand AddChargeCommand => new(AddCharge);

        private void AddCharge(object? obj)
        {
            var window = new AddIncomeWindow();
            window.ShowDialog();
            if (!(window.DialogResult ?? false)) return;

            var business = window.GetBusinessEntity();
            IncomeRows.Add(new IncomeRowModel(business));
        }
    }

    public class IncomeRowModel : NotifyPropertyChangedItem
    {
        private readonly IncomeEntity _entity;

        public IncomeRowModel(IncomeEntity entity) => _entity = entity;

        #region Properties

        public decimal Sum
        {
            get => _entity.Summ;
            set
            {
                _entity.Summ = value;
                OnPropertyChanged();
            }
        }

        public string? IncomeTypeName
        {
            get => _entity.IncomeTypeName;
            set
            {
                _entity.IncomeTypeName = value;
                OnPropertyChanged();
            }
        }

        public DateTime Date
        {
            get => _entity.Date;
            set
            {
                _entity.Date = value;
                OnPropertyChanged();
            }
        }

        public string? Comment
        {
            get => _entity.Comment;
            set
            {
                _entity.Comment = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
