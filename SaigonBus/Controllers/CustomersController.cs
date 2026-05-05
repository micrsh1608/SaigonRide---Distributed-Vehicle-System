using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaigonBus.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private Models.SaigonRideContext db = new Models.SaigonRideContext();

        // GET: Customers
        public ActionResult Index()
        {
            var users = db.Users.OrderByDescending(u => u.UserId).ToList();
            return View(users);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Models.User user)
        {
            if (ModelState.IsValid)
            {
                var dbUser = db.Users.Find(user.UserId);
                if (dbUser != null)
                {
                    dbUser.FullName = user.FullName;
                    dbUser.Email = user.Email;
                    dbUser.Phone = user.Phone;
                    dbUser.Balance = user.Balance;
                    dbUser.CCCD = user.CCCD;
                    dbUser.Passport = user.Passport;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user == null) return HttpNotFound();
            return View(user);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}