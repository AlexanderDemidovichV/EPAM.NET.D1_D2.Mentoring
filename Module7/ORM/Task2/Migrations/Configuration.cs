namespace Task2.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Northwind>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Task2.Northwind context)
        {
            context.Categories.AddOrUpdate(
                c => c.CategoryID, 
                new Category { CategoryID = 1, CategoryName = "SomeCategory" },
                new Category { CategoryID = 2, CategoryName = "AnotherCategory" });

            context.Regions.AddOrUpdate(
                r => r.RegionID,
                new Region { RegionID = 1, RegionDescription = "SomeRegion" },
                new Region { RegionID = 2, RegionDescription = "AnotherRegion" });

            context.Territories.AddOrUpdate(
                t => t.TerritoryID,
                new Territory { TerritoryID = "T1", TerritoryDescription = "SomeTerritory", RegionID = 1 },
                new Territory { TerritoryID = "T2", TerritoryDescription = "AnotherTerritory", RegionID = 1 });
            context.SaveChanges();
        }
    }
}
