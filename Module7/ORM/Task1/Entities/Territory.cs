using LinqToDB.Mapping;

namespace Task1.Entities
{
    [Table("Territories")]
    public class Territory
    {
        [Column("TerritoryID"), PrimaryKey, Identity]
        public string Id { get; set; }

        [Column("TerritoryDescription")]
        public string Description { get; set; }

        [Column("RegionID")]
        public int RegionId { get; set; }

        [Association(ThisKey = "RegionId", OtherKey = "Id")]
        public Region Region;
    }
}