using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("EmployeeTerritories")]
    public class EmployeeTerritory
    {
        [Column("EmployeeID"), PrimaryKey(1)]
        public int EmployeeId { get; set; }

        [Column("TerritoryID"), PrimaryKey(2)]
        public string TerritoryId { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id")]
        public Employee Employee;

        [Association(ThisKey = "TerritoryId", OtherKey = "Id")]
        public Territory Territory;
    }
}