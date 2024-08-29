using Assessment1_1.Models;
using MySql.Data.MySqlClient;  // Use MySql.Data namespace

namespace Assessment1_1.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionStrings;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionStrings = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (MySqlConnection conn = new MySqlConnection(_connectionStrings))
            {
                string sql = "SELECT * FROM employees";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var emp = new Employee
                        {
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Salary = (Decimal)reader["Salary"]
                        };
                        employees.Add(emp);
                    }
                }
            }

            return employees;
        }

        public Employee GetEmployeeById(Guid id)
        {
            Employee emp = null;

            using (MySqlConnection conn = new MySqlConnection(_connectionStrings))
            {
                string sql = "SELECT * FROM employees WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        emp = new Employee
                        {
                            Id = (Guid)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Salary = (Decimal)reader["Salary"]
                        };
                    }
                }
            }

            return emp;
        }

        public void AddEmployee(Employee emp)
        {
            emp.Id = Guid.NewGuid();  // Generate a new Guid for Id
            using (MySqlConnection conn = new MySqlConnection(_connectionStrings))
            {
                string sql = "INSERT INTO employees (Id, Name, Phone, Email, Salary) VALUES (@Id, @Name, @Phone, @Email, @Salary)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", emp.Id);
                cmd.Parameters.AddWithValue("@Name", emp.Name);
                cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateEmployee(Employee emp)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStrings))
            {
                string sql = "UPDATE employees SET Name=@Name, Phone=@Phone, Email=@Email, Salary=@Salary WHERE Id=@Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", emp.Id);
                cmd.Parameters.AddWithValue("@Name", emp.Name);
                cmd.Parameters.AddWithValue("@Phone", emp.Phone);
                cmd.Parameters.AddWithValue("@Email", emp.Email);
                cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(Guid empId)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionStrings))
            {
                string sql = "DELETE FROM employees WHERE Id=@Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", empId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
