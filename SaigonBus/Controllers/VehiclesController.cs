using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SaigonBus.Models;

namespace SaigonBus.Controllers
{
    [Authorize(Roles = "User,Admin")]
    public class VehiclesController : Controller
    {
        private SaigonRideContext db = new SaigonRideContext();

        // GET: Vehicles
        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.CurrentStation);
            return View(vehicles.ToList());
        }

        // GET: Vehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public ActionResult Create()
        {
            ViewBag.CurrentStationId = new SelectList(db.Stations, "StationId", "LocationName");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VehicleId,Category,Status,CurrentStationId")] Vehicle vehicle, int quantity = 1)
        {
            if (ModelState.IsValid)
            {
                if (quantity < 1) quantity = 1;

                for (int i = 0; i < quantity; i++)
                {
                    var newVehicle = new Vehicle
                    {
                        Category = vehicle.Category,
                        Status = vehicle.Status,
                        CurrentStationId = vehicle.CurrentStationId
                    };
                    db.Vehicles.Add(newVehicle);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CurrentStationId = new SelectList(db.Stations, "StationId", "LocationName", vehicle.CurrentStationId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.CurrentStationId = new SelectList(db.Stations, "StationId", "LocationName", vehicle.CurrentStationId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VehicleId,Category,Status,CurrentStationId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CurrentStationId = new SelectList(db.Stations, "StationId", "LocationName", vehicle.CurrentStationId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost]
        public JsonResult Reserve(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) return Json(new { success = false, message = "Vui lòng đăng nhập." });

            var vehicle = db.Vehicles.Find(id);
            if (vehicle == null) return Json(new { success = false, message = "Không tìm thấy xe." });

            // Kiểm tra xe có đang rảnh hoặc đã hết hạn giữ chỗ chưa
            bool isAvailable = vehicle.Status == VehicleStatus.Available &&
                              (vehicle.ReservedUntil == null || vehicle.ReservedUntil < DateTime.Now);

            if (!isAvailable)
            {
                return Json(new { success = false, message = "Xe này đang được người khác sử dụng hoặc giữ chỗ." });
            }

            // Thực hiện giữ chỗ 15p
            vehicle.ReservedUntil = DateTime.Now.AddMinutes(15);
            vehicle.ReservedByUserId = user.UserId;
            db.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Đã giữ chỗ thành công! Bạn có 15 phút để đến nhận xe.",
                expiry = vehicle.ReservedUntil.Value.ToString("HH:mm:ss")
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
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
    }
}
