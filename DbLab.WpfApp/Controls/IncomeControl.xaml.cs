using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;
using System.Collections.ObjectModel;
using DbLab.WpfApp.Windows;

namespace DbLab.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for AnotherControl.xaml
    /// </summary>
    public partial class IncomeControl : UserControlBase
    {
        public readonly IncomeControlModel _model;

        public IncomeControl()
        {
            _model = new IncomeControlModel();
            DataContext = _model;
            InitializeComponent();

            ExecuteAsync(GetIncomeData);
        }

        private void GetIncomeData()
        {
            var lst = new IncomeManager().ReadAll();
            ExecuteInUiThread(() =>
            {
                foreach (var ent in lst)
                    _model.IncomeRows.Add(new IncomeRowModel(ent));
            });
        }
    }

    public class IncomeControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<IncomeRowModel> IncomeRows { get; set; } = new ObservableCollection<IncomeRowModel>();

        public UiCommand AddChargeCommand => new UiCommand(AddCharge);

        private void AddCharge(object? obj)
        {
            var model = new AddIncomeWindowModel();
            var window = new AddIncomeWindow(model);
            window.ShowDialog();
            if (window.DialogResult ?? false)
            {
                var business = model.GetBusinessEntity();
                if (business != null)
                {
                    IncomeRows.Add(new IncomeRowModel(business));
                }
            }
        }
    }

    public class IncomeRowModel : NotifyPropertyChangedItem
    {
        private readonly IncomeEntity _entity;

        public IncomeRowModel(IncomeEntity entity)
        {
            _entity = entity;
        }

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
