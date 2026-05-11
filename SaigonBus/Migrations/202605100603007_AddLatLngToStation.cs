namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLatLngToStation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stations", "Latitude", c => c.Double());
            AddColumn("dbo.Stations", "Longitude", c => c.Double());
        }

        public override void Down()
        {
            DropColumn("dbo.Stations", "Longitude");
            DropColumn("dbo.Stations", "Latitude");
        }
    }
}
