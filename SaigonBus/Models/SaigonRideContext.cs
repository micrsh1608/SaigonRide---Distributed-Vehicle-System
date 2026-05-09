using SaigonBus.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SaigonBus.Models
{
    public class SaigonRideContext : DbContext
    {
        // Tên chuỗi kết nối phải khớp với name trong file Web.config
        public SaigonRideContext() : base("name=SaigonRideConnectionString")
        {
            Database.SetInitializer<SaigonRideContext>(null);
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RentalTransaction> RentalTransactions { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupRental> GroupRentals { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
    }
}   