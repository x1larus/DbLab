using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Windows;
using DbLab.DalPg.Entities;
using DbLab.DalPg.Managers;
using DbLab.WpfApp.Base;

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
            var a = GetData();
        }

        public async Task GetData()
        {
            var lst = new IncomeManager().ReadAll();
            await Task.Delay(5000);
            foreach (var ent in lst)
            {
                Application.Current.Dispatcher.Invoke(() => { _model.IncomeRows.Add(new IncomeRowModel(ent)); });
            }
            //var a = () =>
            //{
                

            //    return Task.CompletedTask;
            //};
        }
    }

    public class IncomeControlModel : NotifyPropertyChangedItem
    {
        public ObservableCollection<IncomeRowModel> IncomeRows { get; set; } = new ObservableCollection<IncomeRowModel>();
    }

    public class IncomeRowModel : NotifyPropertyChangedItem
    {
        private readonly IncomeEntity _entity;
        
        public IncomeRowModel(IncomeEntity entity)
        {
            _entity = entity;
        }

        public decimal Sum => _entity.Summ;

        public string? IncomeTypeName => _entity.IncomeTypeName;

        public DateTime Date => _entity.Date;

        public string? Comment => _entity.Comment;
    }
}
