using DbLab.DalPg.Entities;
using DbLab.WpfApp.Base;
using System.Collections.ObjectModel;
using System.Windows;
using DbLab.DalPg.Managers;

namespace DbLab.WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        private readonly AddIncomeWindowModel _model;

        public AddIncomeWindow()
        {
            InitializeComponent();
            
            _model = new AddIncomeWindowModel();
            DataContext = _model;
            GetTypesAsync();
        }

        public async void GetTypesAsync()
        {
            var types = await new IncomeTypeManager().ReadAll();
            foreach (var type in types)
            {
                _model.IncomeTypeList.Add(type);
            }
            _model.SelectedType = _model.IncomeTypeList.FirstOrDefault();
        }

        public IncomeEntity GetBusinessEntity() => _model.GetBusinessEntity();

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_model.ValidateModel())
                return;

            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

    public class AddIncomeWindowModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<IncomeTypeEntity> IncomeTypeList { get; set; } = [];

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

        public bool ValidateModel()
        {
            return _date != null &&
                   _sum != null &&
                   _selectedType != null;
        }

        public IncomeEntity GetBusinessEntity()
        {
            return new IncomeEntity
            {
                Id = null,
                Date = _date!.Value,
                Summ = _sum!.Value,
                Comment = _comment,
                IdIncomeType = _selectedType!.Id,
                IncomeTypeName = _selectedType!.TypeName
            };
        }
    }
}
