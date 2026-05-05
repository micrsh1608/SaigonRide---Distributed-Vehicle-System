namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RentalTransactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        StartStationId = c.Int(nullable: false),
                        EndStationId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                        CalculatedFare = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppliedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleId = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CurrentStationId = c.Int(),
                    })
                .PrimaryKey(t => t.VehicleId)
                .ForeignKey("dbo.Stations", t => t.CurrentStationId)
                .Index(t => t.CurrentStationId);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        StationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 200),
                        CapacityLimit = c.Int(nullable: false),
                        CurrentInventoryCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalTransactions", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "CurrentStationId", "dbo.Stations");
            DropIndex("dbo.Vehicles", new[] { "CurrentStationId" });
            DropIndex("dbo.RentalTransactions", new[] { "VehicleId" });
            DropTable("dbo.Stations");
            DropTable("dbo.Vehicles");
            DropTable("dbo.RentalTransactions");
        }
    }
}
