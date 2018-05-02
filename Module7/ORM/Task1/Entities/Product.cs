using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Products")]
    public class Product
    {
        [Column("ProductID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("ProductName")]
        public string Name { get; set; }

        [Column("SupplierID")]
        public int SupplierId { get; set; }

        [Column("CategoryID")]
        public int CategoryId { get; set; }

        [Column]
        public decimal UnitPrice { get; set; }

        [Column]
        public int UnitsInStock { get; set; }

        [Association(ThisKey = "SupplierId", OtherKey = "Id")]
        public Supplier Supplier;

        [Association(ThisKey = "CategoryId", OtherKey = "Id")]
        public Category Category;
    }
}