namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddGroupRentalTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroupRentals",
                c => new
                {
                    GroupRentalId = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    StartStationId = c.Int(nullable: false),
                    EndStationId = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                    VehicleCategory = c.String(maxLength: 50),
                    StartTime = c.DateTime(nullable: false),
                    EndTime = c.DateTime(),
                    TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Status = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.GroupRentalId)
                .ForeignKey("dbo.Stations", t => t.EndStationId, cascadeDelete: false)
                .ForeignKey("dbo.Stations", t => t.StartStationId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.StartStationId)
                .Index(t => t.EndStationId);

            CreateTable(
                "dbo.SupportRequests",
                c => new
                {
                    RequestId = c.Int(nullable: false, identity: true),
                    UserId = c.Int(),
                    Subject = c.String(nullable: false, maxLength: 200),
                    Content = c.String(nullable: false),
                    CreatedAt = c.DateTime(nullable: false),
                    Status = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);

            // AddColumn("dbo.Users", "CCCD", c => c.String(maxLength: 20));
            // AddColumn("dbo.Users", "Passport", c => c.String(maxLength: 50));
        }

        public override void Down()
        {
            DropForeignKey("dbo.SupportRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.GroupRentals", "UserId", "dbo.Users");
            DropForeignKey("dbo.GroupRentals", "StartStationId", "dbo.Stations");
            DropForeignKey("dbo.GroupRentals", "EndStationId", "dbo.Stations");
            DropIndex("dbo.SupportRequests", new[] { "UserId" });
            DropIndex("dbo.GroupRentals", new[] { "EndStationId" });
            DropIndex("dbo.GroupRentals", new[] { "StartStationId" });
            DropIndex("dbo.GroupRentals", new[] { "UserId" });
            // DropColumn("dbo.Users", "Passport");
            // DropColumn("dbo.Users", "CCCD");
            DropTable("dbo.SupportRequests");
            DropTable("dbo.GroupRentals");
        }
    }
}
