using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaigonBus.Services;

namespace SaigonBus.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class RentalTransactionsGroupController : Controller
    {
        private Models.SaigonRideContext db = new Models.SaigonRideContext();

        // GET: RentalTransactionGroup
        public ActionResult Index()
        {
            var groups = db.GroupRentals.Include("User").Include("StartStation").OrderByDescending(g => g.StartTime).ToList();
            return View(groups);
        }

        [HttpPost]
        public JsonResult ProcessGroupRental(int stationId, int endStationId, string category, int quantity)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return Json(new { success = false, message = "Vui lòng đăng nhập." });

            // 1. Kiểm tra số lượng xe thực tế tại trạm
            var catEnum = (Models.VehicleCategory)Enum.Parse(typeof(Models.VehicleCategory), category);
            var availableVehicles = db.Vehicles
                .Where(v => v.CurrentStationId == stationId && v.Category == catEnum && v.Status == Models.VehicleStatus.Available)
                .Where(v => v.ReservedUntil == null || v.ReservedUntil < DateTime.Now)
                .ToList();

            if (availableVehicles.Count < quantity)
            {
                return Json(new
                {
                    success = false,
                    message = $"Rất tiếc, trạm này chỉ còn {availableVehicles.Count} xe loại này. Bạn có muốn thuê số lượng này không?",
                    maxAvailable = availableVehicles.Count
                });
            }

            // 2. Tính tiền theo đơn giá mới
            // Xe đạp thường: 500, Xe điện: 1500, Scooter: 1000
            decimal unitPrice = 1000; // Mặc định cho Scooter
            if (category == "EBike") unitPrice = 1500;
            else if (category == "StandardBike") unitPrice = 500;

            decimal totalAmount = unitPrice * quantity;
            decimal totalDiscount = 0;

            // 3. Kiểm tra điều kiện giảm giá 15% (Nếu trạm trả có < 20% sức chứa)
            var endStation = db.Stations.Find(endStationId);
            if (endStation != null)
            {
                double fullness = (double)endStation.CurrentInventoryCount / endStation.CapacityLimit;
                if (fullness < 0.2)
                {
                    totalDiscount = totalAmount * 0.15m;
                    totalAmount -= totalDiscount;
                }
            }

            if (user.Balance < totalAmount)
            {
                return Json(new { success = false, message = $"Số dư ví không đủ. Cần {totalAmount:N0} VNĐ (Đã giảm: {totalDiscount:N0})." });
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 4. Tạo GroupRental
                    var groupRental = new Models.GroupRental
                    {
                        UserId = user.UserId,
                        StartStationId = stationId,
                        EndStationId = endStationId,
                        Quantity = quantity,
                        VehicleCategory = category,
                        StartTime = DateTime.Now,
                        TotalAmount = totalAmount,
                        Status = "Active"
                    };
                    db.GroupRentals.Add(groupRental);
                    db.SaveChanges(); // Lấy ID

                    decimal discountPerVehicle = totalDiscount / quantity;

                    // 4. Tạo các giao dịch cá nhân và cập nhật trạng thái xe
                    for (int i = 0; i < quantity; i++)
                    {
                        var vehicle = availableVehicles[i];
                        vehicle.Status = Models.VehicleStatus.InTransit;
                        vehicle.CurrentStationId = null;

                        var rental = new Models.RentalTransaction
                        {
                            UserId = user.UserId,
                            VehicleId = vehicle.VehicleId,
                            StartStationId = stationId,
                            StartTime = DateTime.Now,
                            GroupRentalId = groupRental.GroupRentalId,
                            CalculatedFare = unitPrice,
                            AppliedDiscount = discountPerVehicle,
                            TotalAmount = unitPrice - discountPerVehicle
                        };
                        db.RentalTransactions.Add(rental);
                    }

                    // 5. Cập nhật số lượng xe tại trạm
                    var station = db.Stations.Find(stationId);
                    if (station != null)
                    {
                        station.CurrentInventoryCount -= quantity;
                    }

                    // 6. Trừ tiền người dùng
                    user.Balance -= totalAmount;

                    db.SaveChanges();
                    transaction.Commit();

                    return Json(new { success = true, message = $"Thuê nhóm thành công {quantity} xe. Tổng phí: {totalAmount:N0} VNĐ." });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Lỗi xử lý giao dịch: " + ex.Message });
                }
            }
        }

        [HttpGet]
        public JsonResult GetActiveGroupRentals()
        {
            var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return Json(new { success = false, message = "Vui lòng đăng nhập." }, JsonRequestBehavior.AllowGet);

            var activeGroupsQuery = db.GroupRentals
                .Where(g => g.UserId == user.UserId && g.Status == "Active")
                .ToList(); // Chạy SQL trước

            var activeGroups = activeGroupsQuery.Select(g => new
            {
                id = g.GroupRentalId,
                quantity = g.Quantity,
                category = g.VehicleCategory,
                startTime = g.StartTime.ToString("HH:mm dd/MM/yyyy"),
                rawStartTime = g.StartTime.ToString("o"), // ISO format cho JS timer
                startStation = g.StartStation?.LocationName ?? "N/A",
                endStationId = g.EndStationId
            }).ToList();

            return Json(new { success = true, groups = activeGroups }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ReturnGroupRental(int groupRentalId, int endStationId)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return Json(new { success = false, message = "Vui lòng đăng nhập." });

            var groupRental = db.GroupRentals.Find(groupRentalId);
            if (groupRental == null || groupRental.UserId != user.UserId)
                return Json(new { success = false, message = "Giao dịch không hợp lệ." });

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    groupRental.Status = "Completed";
                    groupRental.EndTime = DateTime.Now;
                    // Tính toán thời gian và tiền
                    var duration = (groupRental.EndTime.Value - groupRental.StartTime).TotalMinutes;
                    int mins = (int)Math.Max(1, Math.Ceiling(duration));

                    decimal rate = (groupRental.VehicleCategory == "EBike") ? 1500 :
                                   (groupRental.VehicleCategory == "Scooter") ? 1000 : 500;

                    decimal baseAmount = mins * rate * groupRental.Quantity;
                    decimal finalAmount = baseAmount;
                    decimal discountAmount = 0;

                    // Giảm giá 15% nếu trả về trạm đang thiếu xe (<20%)
                    var endStation = db.Stations.Find(endStationId);
                    if (endStation != null)
                    {
                        double fullness = (double)endStation.CurrentInventoryCount / endStation.CapacityLimit;
                        if (fullness < 0.2)
                        {
                            discountAmount = baseAmount * 0.15m;
                            finalAmount = baseAmount - discountAmount;
                        }
                        endStation.CurrentInventoryCount += groupRental.Quantity;
                    }

                    groupRental.TotalAmount = finalAmount;
                    user.Balance += groupRental.TotalAmount;
                    user.Balance -= finalAmount;

                    var rentals = db.RentalTransactions.Where(rt => rt.GroupRentalId == groupRentalId).ToList();
                    foreach (var rt in rentals)
                    {
                        rt.EndTime = groupRental.EndTime;
                        rt.EndStationId = endStationId;
                        rt.TotalAmount = finalAmount / groupRental.Quantity; // Chia đều cho từng xe
                        rt.AppliedDiscount = discountAmount / groupRental.Quantity;

                        var vehicle = db.Vehicles.Find(rt.VehicleId);
                        if (vehicle != null)
                        {
                            vehicle.Status = Models.VehicleStatus.Available;
                            vehicle.CurrentStationId = endStationId;
                        }
                    }

                    if (user != null)
                    {
                        var pointResult = PointService.Calculate(
                            finalAmount: finalAmount,
                            discountAmount: discountAmount,
                            vehicleCategory: groupRental.VehicleCategory,
                            tripStartTime: groupRental.StartTime,
                            quantity: groupRental.Quantity
                        );

                        PointService.ApplyPoints(user, pointResult, db);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    return Json(new
                    {
                        success = true,
                        message = "Đã trả toàn bộ xe nhóm thành công!",
                        finalBill = new
                        {
                            mins = mins,
                            baseAmount = baseAmount,
                            discount = discountAmount,
                            total = finalAmount,
                            rate = rate,
                            quantity = groupRental.Quantity
                        }
                    });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Lỗi khi trả xe: " + ex.Message });
                }
            }
        }

        // GET: RentalTransactionsGroup/Details/5
        public ActionResult Details(int id)
        {
            var group = db.GroupRentals.Include("User").Include("StartStation").FirstOrDefault(g => g.GroupRentalId == id);
            if (group == null) return HttpNotFound();
            return View(group);
        }

        // GET: RentalTransactionsGroup/Delete/5
        public ActionResult Delete(int id)
        {
            var group = db.GroupRentals.Find(id);
            if (group == null) return HttpNotFound();
            return View(group);
        }

        // POST: RentalTransactionsGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var group = db.GroupRentals.Find(id);
            db.GroupRentals.Remove(group);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}