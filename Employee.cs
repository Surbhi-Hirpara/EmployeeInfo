namespace EmployeeSearch
{
    internal class Employee
    {
        public bool IsNew { get; set; } = true;
        private int id;
        public int Id { get { return id; } set { id = value; } }
        private string name;
        public string Name { get { return name; } set { name = value; } }

        private string position;
        public string Position { get { return position; } set { position = value; } }

        private decimal hourlyPayRate;
        public decimal HourlyPayRate { get { return hourlyPayRate; } set { hourlyPayRate = value; } }
    }
}
