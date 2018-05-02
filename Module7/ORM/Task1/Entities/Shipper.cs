using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Shippers")]
    public class Shipper
    {
        [Column("ShipperID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("CompanyName")]
        public string Name;

        [Column]
        public string Phone;
    }
}