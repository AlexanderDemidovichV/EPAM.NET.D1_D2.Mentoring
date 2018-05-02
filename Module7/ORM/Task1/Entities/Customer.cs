using LinqToDB.Mapping;

namespace Task1.Entities
{
    public class Customer
    {
        [Column("CustomerID"), PrimaryKey]
        public string Id { get; set; }
    }
}
