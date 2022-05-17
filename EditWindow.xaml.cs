using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeSearch
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        readonly VM vm;
        readonly Employee employee = new Employee();
        public EditWindow(bool isEdit)
        {
            InitializeComponent();
            vm = VM.Instance;
            if (isEdit)
            {
                employee.Id = vm.SelectedEmployee.Id;
                employee.Name = vm.SelectedEmployee.Name;
                employee.Position = vm.SelectedEmployee.Position;
                employee.HourlyPayRate = vm.SelectedEmployee.HourlyPayRate;

                employee.IsNew = false;
                txtId.IsEnabled = false;
                txtName.IsEnabled = false;
                Title = "Edit Employee";
            }
            else
            {
                txtId.IsEnabled = false;
                Title = "Add Employee";
            }

            DataContext = employee;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
  
            if(employee.Name == null || employee.Position == null)
            {
                MessageBox.Show("Please Fill all information");
            }
            else
            {
                vm.Save(employee);
                Close();
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Id_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            e.Handled = !int.TryParse(tb.Text + e.Text, out _);
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            bool isDecimal = decimal.TryParse(tb.Text + e.Text, out decimal result);
            if (isDecimal)
            {
                if (result >= 0)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            else
                e.Handled = true;
        }
    }
}
