using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Order Details")]
    public class OrderDetails
    {
        [Column("OrderID"), PrimaryKey(1)]
        public int OrderId { get; set; }

        [Column("ProductID"), PrimaryKey(2)]
        public int ProductId { get; set; }

        [Column]
        public decimal UnitPrice { get; set; }

        [Column]
        public int Quantity { get; set; }
    }
}