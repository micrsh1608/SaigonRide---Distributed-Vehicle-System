namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPointToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Point", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Point");
        }
    }
}
