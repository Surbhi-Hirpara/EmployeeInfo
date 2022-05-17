using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmployeeSearch
{
    internal class DB
    {
        const string CONNECTION_STRING = @"Server=(localdb)\ProjectsV13;Database=SIS;Trusted_Connection=True;";
        const string EMPLOYEES_SELECT_COMMAND = "SELECT id, name, position, hourlyPayRate FROM Employees";
        const string INSERT_COMMAND = "INSERT INTO Employees(name, position, hourlyPayRate) " +
            "VALUES (@NAME, @POSITION, @HOURLYPAYRATE)";
        const string UPDATE_COMMAND = "UPDATE Employees " +
            "SET name = @NAME, position = @POSITION, hourlyPayRate = @HOURLYPAYRATE " +
            "WHERE id = @ID";
        const string DELETE_COMMAND = "DELETE FROM Employees WHERE id = @ID";
        const string EMP_LAST_RECORD = "SELECT TOP 1 id FROM Employees ORDER BY id ASC;";
        const string ID_SELECT_COMMAND = "SELECT id FROM Employees "; 

        private readonly SqlConnection conn;
        private static DB db;
        public static DB Instance { get { db ??= new DB(); return db; } }
        private DB()
        {
            conn = new SqlConnection(CONNECTION_STRING);
            conn.Open();
        }
        public bool Insert(Employee employee)
        {
            SqlCommand cmd = new SqlCommand(INSERT_COMMAND, conn);
            cmd.Parameters.AddWithValue("@NAME", employee.Name);
            cmd.Parameters.AddWithValue("@POSITION", employee.Position);
            cmd.Parameters.AddWithValue("@HOURLYPAYRATE", employee.HourlyPayRate);
            if (cmd.ExecuteNonQuery() > 0)
            {
                cmd = new SqlCommand(EMP_LAST_RECORD, conn);
                using SqlDataReader dr = cmd.ExecuteReader();
                int Id = 0;

                while (dr.Read())
                    Id = dr.GetInt32(dr.GetOrdinal("id"));
                dr.Close();
                //cmd = new SqlCommand(ID_SELECT_COMMAND, conn);
                cmd.Parameters.AddWithValue("@ID", employee.Id);
                return cmd.ExecuteNonQuery() > 0;
            }
            else
            {
                return false;
            }
        }
        public bool Update(Employee employee)
        {
            using SqlCommand cmd = new SqlCommand(UPDATE_COMMAND, conn);
            cmd.Parameters.AddWithValue("@ID", employee.Id);
            cmd.Parameters.AddWithValue("@NAME", employee.Name);
            cmd.Parameters.AddWithValue("@POSITION", employee.Position);
            cmd.Parameters.AddWithValue("@HOURLYPAYRATE", employee.HourlyPayRate);
            return cmd.ExecuteNonQuery() > 0;
        }
        public bool Delete(Employee employee)
        {
            using SqlCommand cmd = new SqlCommand(DELETE_COMMAND, conn);
            cmd.Parameters.AddWithValue("@ID", employee.Id);
            return cmd.ExecuteNonQuery() > 0;
        }
        public List<Employee> Get()
        {
            List<Employee> employees = new();
            using SqlCommand cmd = new(EMPLOYEES_SELECT_COMMAND, conn);
            using SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                employees.Add(new Employee
                {
                    Id = (int)dr.GetInt32(dr.GetOrdinal("id")),
                    Name= dr.GetString(dr.GetOrdinal("name")),
                    Position = dr.GetString(dr.GetOrdinal("position")),
                    HourlyPayRate = (decimal)dr.GetSqlMoney(dr.GetOrdinal("hourlyPayRate"))
                });
            }
            return employees;
        }
    }
}
