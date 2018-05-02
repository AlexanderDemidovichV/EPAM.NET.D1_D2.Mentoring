using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Column("EmployeeID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column]
        public string FirstName { get; set; }

        [Column]
        public string LastName { get; set; }
    }
}