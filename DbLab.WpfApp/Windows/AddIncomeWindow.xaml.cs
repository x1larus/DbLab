using System.Windows;
using DbLab.WpfApp.Base;

namespace DbLab.WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        public AddIncomeWindow(AddIncomeWindowModel model)
        {
            model.Parent = this;
            InitializeComponent();
        }
    }

    public class AddIncomeWindowModel : NotifyPropertyChangedItem
    {
        public AddIncomeWindow Parent;

        private DateTime? _date;
        private decimal? _sum;
        private long? _type;
        private string _comment;
    }
}
