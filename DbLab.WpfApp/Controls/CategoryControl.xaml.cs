using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;
using System.Collections.ObjectModel;
using System.Windows;
using DbLab.WpfApp.Controls;

namespace DbLab.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for CategoryControl.xaml
    /// </summary>
    public partial class CategoryControl : UserControlBase
    {
        private readonly CategoryControlModel _model;
        public CategoryControl()
        {
            _model = new CategoryControlModel();
            DataContext = _model;
            InitializeComponent();
            GetDataAsync();
        }

        public async void GetDataAsync()
        {
            _model.Categories.Clear();
            foreach (var el in await new CategoryManager().ReadAll())
            {
                _model.Categories.Add(new CategoryModel() { CategoryEntity = el, ViewModel = _model });
            }
        }
    }


    public class CategoryControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<CategoryModel> Categories { get; set; } =
            new ObservableCollection<CategoryModel>();

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

        public UiCommand SaveCommand => new UiCommand(async (obj) =>
        {
            var persistQueue = Categories.Where(el => el.NeedPersist).ToList();
            try
            {
                var mng = new CategoryManager();
                foreach (var ent in persistQueue)
                {
                    await mng.Write(ent.CategoryEntity);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(Application.Current.MainWindow, $"Возникла ошибка при сохранении {e.Message}");
                return;
            }

            IsSaveBtnEnabled = false;
            foreach (var model in persistQueue)
            {
                model.NeedPersist = false;
            }
        });

        public UiCommand AddNewCommand => new UiCommand((obj) =>
        {
            Categories.Add(new CategoryModel() { ViewModel = this, CategoryEntity = new CategoryEntity(), NeedPersist = true });
            IsSaveBtnEnabled = true;
        });
    }


    public class CategoryModel : NotifyPropertyChangedItem
    {
        public CategoryEntity CategoryEntity { get; set; }

        public bool? IsIncome
        {
            get => CategoryEntity.IsIncome;
            set
            {
                CategoryEntity.IsIncome = value;
                AddToPersist();
                OnPropertyChanged();
            }
        }

        public string? Name
        {
            get => CategoryEntity.Name;
            set
            {
                CategoryEntity.Name = value;
                AddToPersist();
                OnPropertyChanged();
            }
        }

        public bool NeedPersist { get; set; }

        public CategoryControlModel ViewModel { get; set; }

        private void AddToPersist()
        {
            NeedPersist = true;
            ViewModel.IsSaveBtnEnabled = true;
        }
    }
}

