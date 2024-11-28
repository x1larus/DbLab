using System.Collections.ObjectModel;
using System.Windows;
using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;

namespace DbLab.WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for AddAccrualWindow.xaml
    /// </summary>
    public partial class AddAccrualWindow : Window
    {
        private readonly AddAccrualWindowModel _model;
        public AddAccrualWindow()
        {
            _model = new AddAccrualWindowModel();
            DataContext = _model;
            GetDataAsync();
            InitializeComponent();
        }

        public async void GetDataAsync()
        {
            var types = await new AccrualTypeManager().ReadAll();
            _model.AllAccrualCategoriesList = await new AccrualCategoryManager().ReadAll();

            foreach (var type in types)
            {
                _model.AccrualTypesList.Add(type);
            }

            _model.SelectedType = _model.AccrualTypesList.FirstOrDefault();
        }

        public AccrualEntity BusinessEntity => new AccrualEntity
        {
            Id = null,
            IdAccrualCategory = _model.SelectedCategory.Id ?? 0,
            Amount = _model.Amount,
            Comment = _model.Comment,
            AccrualDate = DateOnly.FromDateTime(_model.AccrualDate)
        };

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

    public class AddAccrualWindowModel : NotifyPropertyChangedItem
    {
        public List<AccrualCategoryEntity> AllAccrualCategoriesList{ get; set; } = [];
        public ObservableCollection<AccrualTypeEntity> AccrualTypesList { get; set; } = [];
        public ObservableCollection<AccrualCategoryEntity> AccrualCategoriesList { get; set; } = [];

        private AccrualTypeEntity? _selectedType;
        private AccrualCategoryEntity? _selectedCategory;

        private decimal _amount;
        private string? _comment;
        private DateTime _accrualDate = DateTime.Now;

        public AccrualTypeEntity? SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                AccrualCategoriesList.Clear();
                foreach (var cat in AllAccrualCategoriesList.Where(el => el.IdAccrualType == _selectedType?.Id))
                {
                    AccrualCategoriesList.Add(cat);
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCategorySelectEnable));
            }
        }

        public AccrualCategoryEntity? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public bool IsCategorySelectEnable => SelectedType != null;

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public string? Comment
        {
            get => _comment;
            set
            {
                _comment = value; 
                OnPropertyChanged();
            }
        }

        public DateTime AccrualDate
        {
            get => _accrualDate;
            set
            {
                _accrualDate = value;
                OnPropertyChanged();
            }
        }
    }
}
