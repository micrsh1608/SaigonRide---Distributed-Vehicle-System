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
    public class StationsController : Controller
    {
        private SaigonRideContext db = new SaigonRideContext();

        public ActionResult Index()
        {
            return View(db.Stations.ToList());
        }

        [AllowAnonymous]
        public JsonResult GetStationsData()
        {
            var stations = db.Stations.ToList();
            var vehicles = db.Vehicles.ToList();

            var result = stations.Select(s => new {
                id = s.StationId,
                name = s.LocationName,
                dist = "Gần bạn",
                capacity = s.CapacityLimit,
                currentCount = vehicles.Count(v => v.CurrentStationId == s.StationId && v.Status == Models.VehicleStatus.Available && (v.ReservedUntil == null || v.ReservedUntil < DateTime.Now)),
                vehicles = vehicles.Where(v => v.CurrentStationId == s.StationId && v.Status == Models.VehicleStatus.Available).Select(v => new {
                    id = v.VehicleId,
                    type = v.Category.ToString().ToLower().Contains("bike") ? (v.Category == Models.VehicleCategory.EBike ? "ebike" : "bike") : "scooter",
                    name = v.Category == Models.VehicleCategory.StandardBike ? "Xe đạp chuẩn" :
                           (v.Category == Models.VehicleCategory.EBike ? "Xe điện (E-bike)" : "Scooter điện"),
                    bat = v.Category == Models.VehicleCategory.StandardBike ? (int?)null : 85,
                    price = v.Category == Models.VehicleCategory.StandardBike ? 500 :
                            (v.Category == Models.VehicleCategory.EBike ? 1500 : 1000)
                }).ToList()
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Stations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // GET: Stations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StationId,LocationName,CapacityLimit,CurrentInventoryCount")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Stations.Add(station);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(station);
        }

        // GET: Stations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StationId,LocationName,CapacityLimit,CurrentInventoryCount")] Station station)
        {
            if (ModelState.IsValid)
            {
                db.Entry(station).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(station);
        }

        // GET: Stations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Station station = db.Stations.Find(id);
            if (station == null)
            {
                return HttpNotFound();
            }
            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Station station = db.Stations.Find(id);
                db.Stations.Remove(station);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                TempData["ErrorMessage"] = "Không thể xóa trạm đỗ này vì hiện tại đang có Xe hoặc Giao dịch liên quan. Vui lòng di chuyển toàn bộ xe khỏi trạm trước khi xóa.";
            }
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
