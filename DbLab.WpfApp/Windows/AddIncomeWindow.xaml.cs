using System.Collections.ObjectModel;
using System.Windows;
using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;

namespace DbLab.WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        private readonly AddIncomeWindowModel _model;
        public AddIncomeWindow(AddIncomeWindowModel model)
        {
            model.Parent = this;
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }
    }

    public class AddIncomeWindowModel : NotifyPropertyChangedItem
    {
        public AddIncomeWindow Parent;

        public ObservableCollection<IncomeTypeEntity> IncomeTypeList { get; set; } =
            new ObservableCollection<IncomeTypeEntity>(new IncomeTypeManager().ReadAll());

        #region Private
        
        private DateTime? _date = DateTime.Now;
        private decimal? _sum;
        private IncomeTypeEntity? _selectedType;
        private string? _comment;

        #endregion

        #region Properties
        
        public DateTime? Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public decimal? Sum
        {
            get => _sum;
            set { _sum = value; OnPropertyChanged(); }
        }

        public IncomeTypeEntity? SelectedType
        {
            get => _selectedType;
            set { _selectedType = value; OnPropertyChanged(); }
        }

        public string? Comment
        {
            get => _comment;
            set { _comment = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands

        public UiCommand OkButtonPressedCommand => new UiCommand(OkButtonPressed);
        public UiCommand CancelButtonPressedCommand => new UiCommand(CancelButtonPressed);

        private void OkButtonPressed(object? obj)
        {
            if (Validate())
            {
                Parent.DialogResult = true;
                return;
            }

            MessageBox.Show("Говно, переделывай");
        }

        private void CancelButtonPressed(object? obj)
        {
            Parent.DialogResult = false;
        }

        #endregion

        private bool Validate()
        {
            return _date.HasValue &&
                   _sum.HasValue &&
                   _selectedType != null;
        }

        public IncomeEntity GetBusinessEntity()
        {
            if (!Validate()) return null;

            return new IncomeEntity
            {
                Id = null,
                Date = _date.Value,
                Summ = _sum.Value,
                Comment = _comment,
                IdIncomeType = _selectedType.Id,
                IncomeTypeName = _selectedType.TypeName
            };
        }
    }
}
