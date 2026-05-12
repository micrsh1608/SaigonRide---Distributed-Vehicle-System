using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaigonBus.Models;
using SaigonBus.Services;

namespace SaigonBus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RentalTransactionsController : Controller
    {
        private SaigonRideContext db = new SaigonRideContext();

        // GET: RentalTransactions
        public ActionResult Index()
        {
            // Lấy danh sách giao dịch, sắp xếp giảm dần theo thời gian bắt đầu
            var transactions = db.RentalTransactions
                                 .Include(t => t.Vehicle) // Nạp thêm thông tin xe để hiển thị ở Index
                                 .OrderByDescending(t => t.StartTime)
                                 .ToList();
            return View(transactions);
        }

        // GET: RentalTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalTransaction rentalTransaction = db.RentalTransactions.Find(id);
            if (rentalTransaction == null)
            {
                return HttpNotFound();
            }
            return View(rentalTransaction);
        }

        // GET: RentalTransactions/Create
        public ActionResult Create()
        {
            ViewBag.VehicleId = new SelectList(db.Vehicles, "VehicleId", "VehicleId");
            ViewBag.StartStationId = new SelectList(db.Stations, "StationId", "LocationName");
            return View();
        }

        // POST: RentalTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionId,UserId,VehicleId,StartStationId,EndStationId,StartTime,EndTime,CalculatedFare,AppliedDiscount")] RentalTransaction rentalTransaction)
        {
            if (ModelState.IsValid)
            {
                db.RentalTransactions.Add(rentalTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VehicleId = new SelectList(db.Vehicles, "VehicleId", "VehicleId", rentalTransaction.VehicleId);
            ViewBag.StartStationId = new SelectList(db.Stations, "StationId", "LocationName", rentalTransaction.StartStationId);
            return View(rentalTransaction);
        }

        // GET: RentalTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalTransaction rentalTransaction = db.RentalTransactions.Find(id);
            if (rentalTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleId = new SelectList(db.Vehicles, "VehicleId", "VehicleId", rentalTransaction.VehicleId);
            ViewBag.EndStationId = new SelectList(db.Stations, "StationId", "LocationName", rentalTransaction.EndStationId);

            // Fetch Vehicle Category to determine Unit Price
            var vehicle = db.Vehicles.Find(rentalTransaction.VehicleId);
            decimal unitPrice = 500; // default (StandardBike)
            if (vehicle != null)
            {
                switch (vehicle.Category)
                {
                    case VehicleCategory.EBike:
                        unitPrice = 1500;
                        break;
                    case VehicleCategory.Scooter:
                        unitPrice = 1000;
                        break;
                    case VehicleCategory.StandardBike:
                    default:
                        unitPrice = 500;
                        break;
                }
            }
            ViewBag.UnitPrice = unitPrice;

            return View(rentalTransaction);
        }

        // POST: RentalTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,UserId,VehicleId,StartStationId,EndStationId,StartTime,EndTime,CalculatedFare,AppliedDiscount,TotalAmount")] RentalTransaction rentalTransaction)
        {
            if (ModelState.IsValid)
            {
                // --- THÊM MỚI BƯỚC NÀY: CHẶN LỖI NHẬP SAI THỜI GIAN ÂM ---
                if (rentalTransaction.EndTime.HasValue && rentalTransaction.EndTime.Value < rentalTransaction.StartTime)
                {
                    ModelState.AddModelError("EndTime", "Lỗi: Giờ trả xe không thể diễn ra trước giờ nhận. Hãy lưu ý 12h trưa là 12:xx (00:xx là 12h đêm).");

                    // Trả lại form để Admin nhập lại
                    ViewBag.EndStationId = new SelectList(db.Stations, "StationId", "LocationName", rentalTransaction.EndStationId);
                    ViewBag.VehicleId = new SelectList(db.Vehicles, "VehicleId", "VehicleId", rentalTransaction.VehicleId);
                    return View(rentalTransaction);
                }

                // 1. Tìm thông tin Xe và Trạm trả để lấy dữ liệu tính toán
                var vehicle = db.Vehicles.Find(rentalTransaction.VehicleId);
                var endStation = db.Stations.Find(rentalTransaction.EndStationId);

                if (rentalTransaction.EndTime.HasValue && vehicle != null && endStation != null)
                {
                    // --- BƯỚC A: TÍNH THỜI GIAN SỬ DỤNG ---
                    TimeSpan duration = rentalTransaction.EndTime.Value - rentalTransaction.StartTime;
                    int totalMinutes = (int)Math.Ceiling(duration.TotalMinutes);
                    if (totalMinutes <= 0) totalMinutes = 1; // Tối thiểu tính 1 phút

                    // --- BƯỚC B: ÁP ĐƠN GIÁ THEO LOẠI XE ---
                    decimal unitPrice = 0;
                    switch (vehicle.Category)
                    {
                        case VehicleCategory.StandardBike:
                            unitPrice = 500;
                            break;
                        case VehicleCategory.EBike:
                            unitPrice = 1500;
                            break;
                        case VehicleCategory.Scooter:
                            unitPrice = 1000;
                            break;
                        default:
                            unitPrice = 500;
                            break;
                    }

                    // --- BƯỚC C: TÍNH CƯỚC PHÍ GỐC ---
                    rentalTransaction.CalculatedFare = totalMinutes * unitPrice;

                    // --- BƯỚC D: KIỂM TRA ĐIỀU KIỆN GIẢM GIÁ 15% ---
                    // Nếu trạm trả đang có số xe thấp hơn 20% sức chứa (Inventory < 20% Capacity)
                    double stationFullness = (double)endStation.CurrentInventoryCount / endStation.CapacityLimit;

                    if (stationFullness < 0.2)
                    {
                        // Kích hoạt giảm giá 15% cho khách hàng vì có công hỗ trợ điều phối xe
                        rentalTransaction.AppliedDiscount = rentalTransaction.CalculatedFare * 0.15m;
                    }
                    else
                    {
                        rentalTransaction.AppliedDiscount = 0;
                    }

                    // --- BƯỚC E: TỔNG THÀNH TIỀN ---
                    rentalTransaction.TotalAmount = rentalTransaction.CalculatedFare - rentalTransaction.AppliedDiscount;

                    // --- BƯỚC F: CẬP NHẬT TRẠNG THÁI XE & TRẠM ---
                    // Đưa xe về trạm mới và chuyển trạng thái sang Sẵn sàng (Available)
                    vehicle.CurrentStationId = rentalTransaction.EndStationId;
                    vehicle.Status = VehicleStatus.Available;
                    db.Entry(vehicle).State = EntityState.Modified;

                    // Tăng số lượng xe hiện tại của trạm trả thêm 1
                    endStation.CurrentInventoryCount += 1;
                    db.Entry(endStation).State = EntityState.Modified;
                }

                db.Entry(rentalTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EndStationId = new SelectList(db.Stations, "StationId", "LocationName", rentalTransaction.EndStationId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "VehicleId", "VehicleId", rentalTransaction.VehicleId);
            return View(rentalTransaction);
        }

        // GET: RentalTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalTransaction rentalTransaction = db.RentalTransactions.Find(id);
            if (rentalTransaction == null)
            {
                return HttpNotFound();
            }
            return View(rentalTransaction);
        }

        // POST: RentalTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RentalTransaction rentalTransaction = db.RentalTransactions.Find(id);
            db.RentalTransactions.Remove(rentalTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SaveTrip(string vehicleType, int duration, decimal totalCost, int startStationId, int endStationId, decimal discountAmount = 0, string paymentMethod = "wallet")
        {
            try
            {
                // Kiểm tra xem đã đăng nhập chưa
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { success = false, message = "Bạn cần đăng nhập để thực hiện thuê xe." });
                }

                // Lấy UserId từ Cookie đăng nhập
                int currentUserId = 1;
                var authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var authTicket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && authTicket.UserData.StartsWith("User|"))
                    {
                        var parts = authTicket.UserData.Split('|');
                        if (parts.Length > 1)
                        {
                            int.TryParse(parts[1], out currentUserId);
                        }
                    }
                }

                using (var db = new SaigonRideContext())
                {
                    var startStation = db.Stations.Find(startStationId);
                    var endStation = db.Stations.Find(endStationId);

                    // Xác định loại xe dựa trên tên gửi từ Client
                    VehicleCategory targetCategory = VehicleCategory.StandardBike;
                    if (vehicleType.Contains("Scooter"))
                        targetCategory = VehicleCategory.Scooter;
                    else if (vehicleType.Contains("điện") || vehicleType.Contains("E-bike") || vehicleType.Contains("EBike"))
                        targetCategory = VehicleCategory.EBike;

                    // Tìm xe phù hợp tại trạm đó
                    var vehicle = db.Vehicles.FirstOrDefault(v => v.CurrentStationId == startStationId && v.Category == targetCategory);

                    // Nếu không tìm thấy xe đúng loại tại trạm đó (trường hợp hiếm), lấy xe bất kỳ đúng loại hoặc xe đầu tiên
                    if (vehicle == null)
                    {
                        vehicle = db.Vehicles.FirstOrDefault(v => v.Category == targetCategory)
                                  ?? db.Vehicles.FirstOrDefault()
                                  ?? new Vehicle { Category = targetCategory, Status = VehicleStatus.Available };

                        if (vehicle.VehicleId == 0) { db.Vehicles.Add(vehicle); db.SaveChanges(); }
                    }

                    // Không tự tính lại discount ở Backend nữa vì Client đã gửi lên (bao gồm cả Promo Code và Giảm giá 15% trạm)
                    // để tránh lỗi Double Discount.
                    decimal finalAmount = totalCost - discountAmount;
                    decimal discount = discountAmount;

                    // 3. LƯU GIAO DỊCH THUÊ XE
                    var newTransaction = new RentalTransaction
                    {
                        UserId = currentUserId,
                        VehicleId = vehicle.VehicleId,
                        StartStationId = startStationId,
                        EndStationId = endStationId,
                        StartTime = DateTime.Now.AddMinutes(-duration),
                        EndTime = DateTime.Now,
                        CalculatedFare = totalCost,
                        AppliedDiscount = discount,
                        TotalAmount = finalAmount
                    };

                    db.RentalTransactions.Add(newTransaction);

                    // CẬP NHẬT TRẠM & XE
                    if (startStation != null) startStation.CurrentInventoryCount--;
                    if (endStation != null) endStation.CurrentInventoryCount++;

                    vehicle.CurrentStationId = endStationId;
                    vehicle.Status = VehicleStatus.Available;

                    if (paymentMethod == "wallet")
                    {
                        var user = db.Users.Find(currentUserId);
                        if (user != null)
                        {
                            user.Balance -= finalAmount;

                            var pointResult = PointService.Calculate(
                                finalAmount: finalAmount,
                                discountAmount: discount,
                                vehicleCategory: vehicleType,
                                tripStartTime: DateTime.Now,
                                quantity: 1
                            );

                            PointService.ApplyPoints(user, pointResult, db);
                        }
                    }
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Thanh toán thành công! Hệ thống đã cập nhật dữ liệu trạm." });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                if (ex.InnerException != null) 
                {
                    error += " | " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null) 
                    {
                        error += " | " + ex.InnerException.InnerException.Message;
                    }
                }
                return Json(new { success = false, message = error });
            }
        }
    }
}
