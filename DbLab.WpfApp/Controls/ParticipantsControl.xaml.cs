using System.Collections.ObjectModel;
using System.Transactions;
using System.Windows;
using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;

namespace DbLab.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for ParticipantsControl.xaml
    /// </summary>
    public partial class ParticipantsControl : UserControlBase
    {
        private readonly ParticipantsControlModel _model;

        public ParticipantsControl()
        {
            _model = new ParticipantsControlModel();
            DataContext = _model;
            InitializeComponent();
            GetDataAsync();
        }

        private async void GetDataAsync()
        {
            _model.Participants.Clear();
            foreach (var el in await new ParticipantManager().ReadAll())
            {
                _model.Participants.Add(new ParticipantModel { Participant = el, ViewModel = _model });
            }
        }
    }

    public class ParticipantsControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<ParticipantModel> Participants { get; set; } =
            new ObservableCollection<ParticipantModel>();

        private bool _isSaveBtnEnabled;

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
            var persistQueue = Participants.Where(el => el.NeedPersist).ToList();
            try
            {
                var mng = new ParticipantManager();
                foreach (var ent in persistQueue)
                {
                    await mng.Write(ent.Participant);
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
            Participants.Add(new ParticipantModel() { ViewModel = this, Participant = new ParticipantEntity(), NeedPersist = true});
            IsSaveBtnEnabled = true;
        });
    }

    public class ParticipantModel : NotifyPropertyChangedItem
    {
        public ParticipantEntity Participant { get; set; }

        public ParticipantsControlModel ViewModel { get; set; }

        public bool NeedPersist { get; set; } = false;

        public string Name
        {
            get => Participant.Name;
            set
            {
                Participant.Name = value;
                AddToPersist();
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => Participant.PhoneNumber;
            set
            {
                Participant.PhoneNumber = value;
                AddToPersist();
                OnPropertyChanged();
            }
        }

        private void AddToPersist()
        {
            NeedPersist = true;
            ViewModel.IsSaveBtnEnabled = true;
        }
    }
}
