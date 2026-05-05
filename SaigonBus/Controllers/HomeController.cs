using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaigonBus.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Portal()
        {
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult UserDashboard()
        {
            if (User.IsInRole("User"))
            {
                using (var db = new Models.SaigonRideContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                    if (user != null)
                    {
                        ViewBag.FullName = user.FullName ?? "Khách hàng";
                        ViewBag.Balance = user.Balance;
                        ViewBag.Phone = user.Phone ?? "N/A";
                        ViewBag.Email = user.Email ?? "N/A";
                    }
                }
            }
            else if (User.IsInRole("Admin"))
            {
                ViewBag.FullName = "Quản trị viên (Khách)";
                ViewBag.Balance = 0;
                ViewBag.Phone = "N/A";
                ViewBag.Email = "admin@saigonride.com";
            }
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult SubmitSupport(string subject, string content)
        {
            using (var db = new Models.SaigonRideContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                var request = new Models.SupportRequest
                {
                    UserId = user?.UserId,
                    Subject = subject ?? "Yêu cầu hỗ trợ",
                    Content = content,
                    CreatedAt = DateTime.Now,
                    Status = "Pending"
                };
                db.SupportRequests.Add(request);
                db.SaveChanges();
            }
            return Json(new { success = true });
        }
    }
}