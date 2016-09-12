namespace FundRaising.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Distributor = c.String(),
                        Name = c.String(nullable: false),
                        ContactName = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Postal = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                        WelcomeMessage = c.String(maxLength: 1000),
                        Catalog = c.String(),
                        ShipToSchool = c.Boolean(nullable: false),
                        ShioToSchoolOnly = c.Boolean(nullable: false),
                        ShipToSchoolCatalog = c.String(),
                        LoginID = c.String(),
                        Password = c.String(),
                        ParticipantOption = c.String(),
                        PricingLevel = c.String(),
                        FreeShippingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AutoAssignParticipantID = c.Boolean(nullable: false),
                        CollectTeacherGrade = c.Boolean(nullable: false),
                        CollectCellPhone = c.Boolean(nullable: false),
                        CellPhoneRequired = c.Boolean(nullable: false),
                        GoalType = c.String(),
                        DefaultGoal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Campaign",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrganizatonID = c.Int(nullable: false),
                        CampaignName = c.String(nullable: false),
                        CampaignStartDate = c.DateTime(nullable: false),
                        CampaignEndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Organizations", t => t.OrganizatonID, cascadeDelete: true)
                .Index(t => t.OrganizatonID);
            
            CreateTable(
                "dbo.Brochures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BrochureID = c.String(),
                        Description = c.String(),
                        BrochureName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryID = c.String(),
                        CategoryName = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemNumber = c.Int(nullable: false),
                        productType = c.String(),
                        Description = c.String(),
                        CustomerRetailPrice = c.Double(nullable: false),
                        CustomerRetailPriceA = c.Double(nullable: false),
                        CustomerRetailPriceB = c.Double(nullable: false),
                        FundTrackerPrice = c.Double(nullable: false),
                        FundTrackerPriceA = c.Double(nullable: false),
                        FundTrackerPriceB = c.Double(nullable: false),
                        ItemWeight = c.Double(nullable: false),
                        ChargeSalesTax = c.Boolean(nullable: false),
                        ChargeShipping = c.Boolean(nullable: false),
                        ItemOverSize = c.Boolean(nullable: false),
                        ItemActive = c.Boolean(nullable: false),
                        PicsInventory = c.Boolean(nullable: false),
                        InventoryAmount = c.Double(nullable: false),
                        ItemExtraTitle = c.String(),
                        ItemExtraFileName = c.String(),
                        DetailDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MapCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BrochureID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Campaign", new[] { "OrganizatonID" });
            DropForeignKey("dbo.Campaign", "OrganizatonID", "dbo.Organizations");
            DropTable("dbo.MapCategory");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
            DropTable("dbo.Brochures");
            DropTable("dbo.Campaign");
            DropTable("dbo.Organizations");
        }
    }
}
