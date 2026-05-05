using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaigonBus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupportsController : Controller
    {
        private Models.SaigonRideContext db = new Models.SaigonRideContext();

        // GET: Supports
        public ActionResult Index()
        {
            var requests = db.SupportRequests.Include("User").OrderByDescending(r => r.CreatedAt).ToList();
            return View(requests);
        }

        [HttpPost]
        public JsonResult ResolveRequest(int id)
        {
            var req = db.SupportRequests.Find(id);
            if (req != null)
            {
                req.Status = "Resolved";
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: Supports/Details/5
        public ActionResult Details(int id)
        {
            var req = db.SupportRequests.Include("User").FirstOrDefault(r => r.RequestId == id);
            if (req == null) return HttpNotFound();
            return View(req);
        }

        // GET: Supports/Delete/5
        public ActionResult Delete(int id)
        {
            var req = db.SupportRequests.Find(id);
            if (req == null) return HttpNotFound();
            return View(req);
        }

        // POST: Supports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var req = db.SupportRequests.Find(id);
            db.SupportRequests.Remove(req);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}