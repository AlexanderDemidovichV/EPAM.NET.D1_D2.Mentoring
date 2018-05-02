namespace Task2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Version_1_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        CreditCardID = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        ExpiryDate = c.DateTime(),
                        CardHolderName = c.String(),
                        Holder_EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.CreditCardID)
                .ForeignKey("dbo.Employees", t => t.Holder_EmployeeID)
                .Index(t => t.Holder_EmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditCards", "Holder_EmployeeID", "dbo.Employees");
            DropIndex("dbo.CreditCards", new[] { "Holder_EmployeeID" });
            DropTable("dbo.CreditCards");
        }
    }
}
