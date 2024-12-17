using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;
using System.Collections.ObjectModel;

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
            foreach (var ent in await new CategoryManager().ReadAll())
                _model.CategoryList.Add(ent);

            foreach (var ent in await new ParticipantManager().ReadAll())
                _model.ParticipantList.Add(ent);

            foreach (var ent in await new AccrualManager().ReadAll())
                _model.AccrualModelList.Add(new AccrualModel(ent, _model.ParticipantList, _model.CategoryList));
        }
    }

    public class AccrualControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<AccrualModel> AccrualModelList { get; set; } = new ObservableCollection<AccrualModel>();
        public ObservableCollection<ParticipantEntity> ParticipantList { get; set; } = new ObservableCollection<ParticipantEntity>();
        public ObservableCollection<CategoryEntity> CategoryList { get; set; } = new ObservableCollection<CategoryEntity>();
    }

    public class AccrualModel : NotifyPropertyChangedItem
    {
        private readonly AccrualEntity _accrualEntity;

        public bool NeedPersist { get; private set; } = false;

        public ObservableCollection<ParticipantEntity> ParticipantList { get; set; }
        public ObservableCollection<CategoryEntity> CategoryList { get; set; }

        public AccrualModel(AccrualEntity accrualEntity, ObservableCollection<ParticipantEntity> participantList, ObservableCollection<CategoryEntity> categoryList)
        {
            _accrualEntity = accrualEntity;
            ParticipantList = participantList;
            CategoryList = categoryList;
            _selectedParticipant = ParticipantList.FirstOrDefault(el => el.Id == _accrualEntity.ParticipantId);
            _selectedCategory = CategoryList.FirstOrDefault(el => el.Id == _accrualEntity.CategoryId);
        }

        private ParticipantEntity? _selectedParticipant;
        private CategoryEntity? _selectedCategory;

        public DateTime? Date
        {
            get => _accrualEntity.Date;
            set
            {
                _accrualEntity.Date = value;
                NeedPersist = true;
                OnPropertyChanged();
            }
        }

        public ParticipantEntity? SelectedParticipant
        {
            get => _selectedParticipant;
            set
            {
                _selectedParticipant = value;
                _accrualEntity.ParticipantId = _selectedParticipant?.Id;
                NeedPersist = true;
                OnPropertyChanged();
            }
        }

        public CategoryEntity? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                _accrualEntity.CategoryId = _selectedCategory?.Id;
                NeedPersist = true;
                OnPropertyChanged();
            }
        }

        public decimal? Amount
        {
            get => _accrualEntity.Amount;
            set
            {
                _accrualEntity.Amount = value;
                NeedPersist = true;
                OnPropertyChanged();
            }
        }

        public string? Comment
        {
            get => _accrualEntity.Comment;
            set
            {
                _accrualEntity.Comment = value;
                NeedPersist = true;
                OnPropertyChanged();
            }
        }
    }
}
