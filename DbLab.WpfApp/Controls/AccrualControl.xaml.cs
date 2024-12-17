using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;
using System.Collections.ObjectModel;
using System.Windows;

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
                _model.AccrualModelList.Add(new AccrualModel(ent, _model.ParticipantList, _model.CategoryList, _model));
        }
    }

    public class AccrualControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<AccrualModel> AccrualModelList { get; set; } = new ObservableCollection<AccrualModel>();
        public ObservableCollection<ParticipantEntity> ParticipantList { get; set; } = new ObservableCollection<ParticipantEntity>();
        public ObservableCollection<CategoryEntity> CategoryList { get; set; } = new ObservableCollection<CategoryEntity>();

        private bool _isSaveBtnEnabled = false;

        public bool IsSaveBtnEnabled
        {
            get => _isSaveBtnEnabled;
            set
            {
                _isSaveBtnEnabled = value;
                OnPropertyChanged();
            }
        }

        public UiCommand SaveCommand => new UiCommand((obj) =>
        {
            var persistQueue = AccrualModelList.Where(el => el.NeedPersist).ToList();
            try
            {
                var mng = new AccrualManager();
                foreach (var ent in persistQueue)
                {
                    mng.Write(ent._accrualEntity);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Application.Current.MainWindow, $"Возникла ошибка при сохранении {e.Message}");
            }
            finally
            {
                IsSaveBtnEnabled = false;
                foreach (var model in persistQueue)
                {
                    model.NeedPersist = false;
                }
            }
        });
    }

    public class AccrualModel : NotifyPropertyChangedItem
    {
        public readonly AccrualEntity _accrualEntity;

        public bool NeedPersist { get; set; } = false;

        public ObservableCollection<ParticipantEntity> ParticipantList { get; set; }
        public ObservableCollection<CategoryEntity> CategoryList { get; set; }

        private AccrualControlModel _viewModel;

        public AccrualModel(AccrualEntity accrualEntity, ObservableCollection<ParticipantEntity> participantList, ObservableCollection<CategoryEntity> categoryList, AccrualControlModel viewModel)
        {
            _accrualEntity = accrualEntity;
            ParticipantList = participantList;
            CategoryList = categoryList;
            _selectedParticipant = ParticipantList.FirstOrDefault(el => el.Id == _accrualEntity.ParticipantId);
            _selectedCategory = CategoryList.FirstOrDefault(el => el.Id == _accrualEntity.CategoryId);
            _viewModel = viewModel;
        }

        private ParticipantEntity? _selectedParticipant;
        private CategoryEntity? _selectedCategory;

        public DateTime? Date
        {
            get => _accrualEntity.Date;
            set
            {
                _accrualEntity.Date = value;
                AddToPersist();
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
                AddToPersist();
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
                AddToPersist();
                OnPropertyChanged();
            }
        }

        public decimal? Amount
        {
            get => _accrualEntity.Amount;
            set
            {
                _accrualEntity.Amount = value;
                AddToPersist();
                OnPropertyChanged();
            }
        }

        public string? Comment
        {
            get => _accrualEntity.Comment;
            set
            {
                _accrualEntity.Comment = value;
                AddToPersist();
                OnPropertyChanged();
            }
        }

        public void AddToPersist()
        {
            NeedPersist = true;
            _viewModel.IsSaveBtnEnabled = true;
        }
    }
}
