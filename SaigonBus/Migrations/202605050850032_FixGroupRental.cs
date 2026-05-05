namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixGroupRental : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroupRentals", "EndStationId", "dbo.Stations");
            DropIndex("dbo.GroupRentals", new[] { "EndStationId" });
            AlterColumn("dbo.GroupRentals", "EndStationId", c => c.Int());
            CreateIndex("dbo.GroupRentals", "EndStationId");
            AddForeignKey("dbo.GroupRentals", "EndStationId", "dbo.Stations", "StationId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.GroupRentals", "EndStationId", "dbo.Stations");
            DropIndex("dbo.GroupRentals", new[] { "EndStationId" });
            AlterColumn("dbo.GroupRentals", "EndStationId", c => c.Int(nullable: false));
            CreateIndex("dbo.GroupRentals", "EndStationId");
            AddForeignKey("dbo.GroupRentals", "EndStationId", "dbo.Stations", "StationId", cascadeDelete: true);
        }
    }
}
