//Code Written by Surbhi Hirpara
using System.Windows;

namespace EmployeeSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly VM vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = VM.Instance;
            DataContext = vm;
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            vm.Search();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(false) { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            ew.ShowDialog();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedEmployee != null)
            {
                EditWindow ew = new EditWindow(true) { Owner = this };
                ew.ShowDialog();
            }
            else
                MessageBox.Show("Please select a Employee to edit");
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedEmployee != null)
            {
                vm.Delete();
            }
            else
            {
                MessageBox.Show("Please select a Employee to Delete");
            }
        }
    }
}
