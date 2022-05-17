using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace EmployeeSearch
{
    internal class VM : INotifyPropertyChanged
    {
        readonly DB db = DB.Instance;
        internal List<Employee> employees;
       
        #region signleton
        private static VM vm;
        public static VM Instance { get { vm ??= new VM(); return vm; } }
       
        #endregion
        public BindingList<Employee> Employees { get; set; } = new BindingList<Employee>();
        private string userSearch;
        public string UserSearch
        {
            get { return userSearch; }
            set { userSearch = value; }
        }
        private Employee selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; propertyChanged(); }
        }
        #region methods
        private VM()
        {
            employees = db.Get();
            updateEmployeesList();
        }
        public void Save(Employee employee)
        {
            bool success = false;

            if (employee.IsNew)
            {
                success = db.Insert(employee);
                employees.Add(employee);
                employees = db.Get();
                updateEmployeesList();
            }
            else
            {
                success = db.Update(employee);
                if (success)
                {
                    employees.Remove(SelectedEmployee);
                    SelectedEmployee = null;
                }
            }

            if (success)
            {
                employees.Add(employee);
                updateEmployeesList();
            }
        }
        public void Delete()
        {
            if (db.Delete(SelectedEmployee))
            {
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
            }
        }
        private void updateEmployeesList()
        {
            employees = employees.OrderBy(e => e.Id).ToList();
            Employees.Clear();
            foreach (Employee e in employees)
                Employees.Add(e);
        }
        public void Search()
        {
            Employees.Clear();
            List<Employee> foundRecords = employees;
            string user_search = UserSearch.ToUpper();
            if (!string.IsNullOrEmpty(userSearch))
                foundRecords = employees.Where(e => e.Name.ToUpper().Contains(user_search)).ToList();
            foreach (Employee e in foundRecords)
                Employees.Add(e);
        }
        #endregion
        #region
        public event PropertyChangedEventHandler PropertyChanged;
        private void propertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
