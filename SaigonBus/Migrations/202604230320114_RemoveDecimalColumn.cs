namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDecimalColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalTransactions", "TotalAmount", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.RentalTransactions", "EndStationId", c => c.Int());
            AlterColumn("dbo.RentalTransactions", "CalculatedFare", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.RentalTransactions", "AppliedDiscount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RentalTransactions", "AppliedDiscount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RentalTransactions", "CalculatedFare", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.RentalTransactions", "EndStationId", c => c.Int(nullable: false));
            DropColumn("dbo.RentalTransactions", "TotalAmount");
        }
    }
}
