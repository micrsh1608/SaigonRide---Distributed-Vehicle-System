namespace SaigonBus.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SaigonBus.Models; // Khai báo dòng này để gọi được Model Station

    internal sealed class Configuration : DbMigrationsConfiguration<SaigonBus.Models.SaigonRideContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SaigonBus.Models.SaigonRideContext context)
        {
            // Hàm AddOrUpdate giúp hệ thống tự động kiểm tra: 
            // Nếu chưa có tên trạm này thì Thêm mới, nếu có rồi thì Cập nhật
            context.Stations.AddOrUpdate(s => s.LocationName,
                new Station { LocationName = "Trạm Metro Bến Thành (Quận 1)", CapacityLimit = 50, CurrentInventoryCount = 5 },
                new Station { LocationName = "Trạm Phố đi bộ Nguyễn Huệ (Quận 1)", CapacityLimit = 40, CurrentInventoryCount = 15 },
                new Station { LocationName = "Trạm Bưu điện Trung tâm (Quận 1)", CapacityLimit = 35, CurrentInventoryCount = 10 },
                new Station { LocationName = "Trạm Công viên Lê Văn Tám (Quận 1)", CapacityLimit = 20, CurrentInventoryCount = 18 },
                new Station { LocationName = "Trạm Hồ Con Rùa (Quận 3)", CapacityLimit = 25, CurrentInventoryCount = 2 },

                new Station { LocationName = "Trạm Ký túc xá TDTU (Quận 7)", CapacityLimit = 100, CurrentInventoryCount = 50 },
                new Station { LocationName = "Trạm Crescent Mall (Quận 7)", CapacityLimit = 45, CurrentInventoryCount = 40 },
                new Station { LocationName = "Trạm SC VivoCity (Quận 7)", CapacityLimit = 30, CurrentInventoryCount = 20 },
                new Station { LocationName = "Trạm Landmark 81 (Bình Thạnh)", CapacityLimit = 60, CurrentInventoryCount = 55 },

                new Station { LocationName = "Trạm Vincom Thảo Điền (TP. Thủ Đức)", CapacityLimit = 30, CurrentInventoryCount = 28 },
                new Station { LocationName = "Trạm Làng Đại học (TP. Thủ Đức)", CapacityLimit = 120, CurrentInventoryCount = 15 },
                new Station { LocationName = "Trạm Bến xe Miền Đông mới (TP. Thủ Đức)", CapacityLimit = 150, CurrentInventoryCount = 100 },
                new Station { LocationName = "Trạm Sân bay Tân Sơn Nhất (Tân Bình)", CapacityLimit = 80, CurrentInventoryCount = 40 },
                new Station { LocationName = "Trạm Chợ Lớn (Quận 5)", CapacityLimit = 30, CurrentInventoryCount = 4 }
             );
        }
    }
}