using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Region")]
    public class Region
    {
        [Column("RegionID"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("RegionDescription")]
        public string Description { get; set; }
    }
}
