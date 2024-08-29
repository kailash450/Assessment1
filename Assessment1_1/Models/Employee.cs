namespace Assessment1_1.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public decimal Salary { get; set; }
    }
}
