namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdminsAndUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.AdminId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Phone = c.String(maxLength: 20),
                        Email = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateIndex("dbo.RentalTransactions", "UserId");
            
            // Tạo dữ liệu User tự động cho các giao dịch thuê xe đã có sẵn trong DB để tránh lỗi Foreign Key
            Sql(@"
                SET IDENTITY_INSERT dbo.Users ON;
                INSERT INTO dbo.Users (UserId, Username, Password, FullName, Balance)
                SELECT DISTINCT UserId, 'user_' + CAST(UserId AS VARCHAR), '123456', N'Khách hàng ' + CAST(UserId AS VARCHAR), 0
                FROM dbo.RentalTransactions
                WHERE UserId NOT IN (SELECT UserId FROM dbo.Users);
                SET IDENTITY_INSERT dbo.Users OFF;
            ");

            AddForeignKey("dbo.RentalTransactions", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalTransactions", "UserId", "dbo.Users");
            DropIndex("dbo.RentalTransactions", new[] { "UserId" });
            DropTable("dbo.Users");
            DropTable("dbo.Admins");
        }
    }
}
