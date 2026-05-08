using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public ActionResult Weather()
        {
            return View();
        }
        [Authorize]
        public ActionResult MemberRank()
        {
            using (var db = new Models.SaigonRideContext())
            {
                string username = User.Identity.Name;

                var user = db.Users.FirstOrDefault(u => u.Username == username);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                int points = user.Point;

                string rankName = "HẠNG ĐỒNG";
                string nextRankName = "HẠNG BẠC";
                int nextRankPoint = 1000;

                if (points >= 5000)
                {
                    rankName = "KIM CƯƠNG";
                    nextRankName = "VIP";
                    nextRankPoint = 10000;
                }
                else if (points >= 2500)
                {
                    rankName = "HẠNG VÀNG";
                    nextRankName = "KIM CƯƠNG";
                    nextRankPoint = 5000;
                }
                else if (points >= 1000)
                {
                    rankName = "HẠNG BẠC";
                    nextRankName = "HẠNG VÀNG";
                    nextRankPoint = 2500;
                }

                var topVips = db.Users
                    .OrderByDescending(u => u.Point)
                    .Take(5)
                    .ToList();

                bool isTop5 = topVips.Any(u => u.UserId == user.UserId);

                bool isVIP = points >= 5000 && isTop5;

                if (isVIP)
                {
                    rankName = "VIP";
                }

                int progress = 0;

                if (isVIP)
                {
                    progress = 100;
                }
                else
                {
                    progress = (points * 100) / nextRankPoint;

                    if (progress > 100)
                    {
                        progress = 100;
                    }
                }

                var allUsers = db.Users.OrderByDescending(u => u.Point).ToList();

                int currentRankPosition = 1;

                for (int i = 0; i < allUsers.Count; i++)
                {
                    if (allUsers[i].UserId == user.UserId)
                    {
                        currentRankPosition = i + 1;
                        break;
                    }
                }

                ViewBag.IsVIP = isVIP;
                ViewBag.CurrentRankName = rankName;
                ViewBag.CurrentPoints = points;
                ViewBag.NextRankName = nextRankName;
                ViewBag.ProgressPercentage = progress;
                ViewBag.PointsToNextRank = Math.Max(0, nextRankPoint - points);
                ViewBag.CurrentRankPosition = currentRankPosition;
                ViewBag.TotalUsers = allUsers.Count;

                ViewBag.TopVips = topVips;

                return View();
            }
        }
        [HttpGet]
        public JsonResult GetStationsData()
        {
            using (var db = new Models.SaigonRideContext())
            {
                var stations = db.Stations.Select(s => new {
                    s.StationId,
                    StationName = s.LocationName,
                    CurrentBikes = s.CurrentInventoryCount,
                    Capacity = s.CapacityLimit,
                    Latitude = (double?)null,
                    Longitude = (double?)null
                }).ToList();
                return Json(stations, JsonRequestBehavior.AllowGet);
            }
        }
    }
}