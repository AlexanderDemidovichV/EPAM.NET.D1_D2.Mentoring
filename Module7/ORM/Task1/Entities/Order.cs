using System;
using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Column("OrderID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("EmployeeID")]
        public int EmployeeId { get; set; }

        [Column("CustomerID")]
        public string CustomerId { get; set; }

        [Column]
        public int ShipVia { get; set; }

        [Column]
        public DateTime ShippedDate { get; set; }

        [Association(ThisKey = "EmployeeId", OtherKey = "Id")]
        public Employee Employee;

        [Association(ThisKey = "CustomerId", OtherKey = "Id")]
        public Customer Customer;

        [Association(ThisKey = "ShipVia", OtherKey = "Id")]
        public Shipper Shipper;
    }
}