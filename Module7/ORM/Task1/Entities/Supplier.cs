using LinqToDB.Mapping;
using LinqToDB.SchemaProvider;

namespace Task1.Entities
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Column("SupplierID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column]
        public string CompanyName { get; set; }

        [Column]
        public string ContactName { get; set; }
    }
}