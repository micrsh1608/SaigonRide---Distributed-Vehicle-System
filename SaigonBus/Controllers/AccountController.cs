using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SaigonBus.Models;

namespace SaigonBus.Controllers
{
    public class AccountController : Controller
    {
        private SaigonRideContext db = new SaigonRideContext();

        // GET: Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // Nếu đã đăng nhập VÀ CÓ QUYỀN ADMIN thì mới tự động chuyển vào Index
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin model, string returnUrl)
        {
            ModelState.Remove("FullName"); // Bỏ qua việc check tên khi đăng nhập
            if (ModelState.IsValid)
            {
                // Tìm kiếm Admin trong Database theo Username và Password
                // LƯU Ý: Đồ án mẫu đang dùng mật khẩu dạng text thuần. Nếu có thời gian, nên mã hóa (ví dụ: MD5, BCrypt)
                var admin = db.Admins.FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);

                if (admin != null)
                {
                    // Đăng nhập thành công, lưu Cookie với Role Admin
                    var ticket = new FormsAuthenticationTicket(1, admin.Username, DateTime.Now, DateTime.Now.AddDays(1), false, "Admin");
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }

            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // ======================== USER LOGIN / REGISTER ========================

        // GET: Account/UserLogin
        [AllowAnonymous]
        public ActionResult UserLogin(string returnUrl)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                return RedirectToAction("UserDashboard", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/UserLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(User model, string returnUrl)
        {
            // Chúng ta chỉ cần Username và Password để validate ở bước này, có thể bỏ qua các trường khác
            ModelState.Remove("FullName");
            ModelState.Remove("Phone");
            ModelState.Remove("Email");

            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Đăng nhập thành công, lưu Cookie kèm Role User và UserId
                    var ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddDays(1), false, "User|" + user.UserId);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("UserDashboard", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return View(model);
        }

        // GET: Account/UserRegister
        [AllowAnonymous]
        public ActionResult UserRegister()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                return RedirectToAction("UserDashboard", "Home");
            }
            return View();
        }

        // POST: Account/UserRegister
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserRegister(User model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Username đã tồn tại chưa
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng.");
                    return View(model);
                }

                // Lưu user mới vào CSDL
                db.Users.Add(model);
                db.SaveChanges();

                // Tự động đăng nhập luôn sau khi đăng ký
                var ticket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now, DateTime.Now.AddDays(1), false, "User|" + model.UserId);
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);

                return RedirectToAction("UserDashboard", "Home");
            }

            return View(model);
        }

        // ======================== USER LOGIN / REGISTER VIA PORTAL ========================

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PortalAuth(string AuthMode, string FullName, string Phone, string CCCD, string Email, string Passport, string Password)
        {
            bool isLocal = !string.IsNullOrEmpty(Phone);

            if (string.IsNullOrEmpty(Password))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập mật khẩu.";
                return RedirectToAction("Portal", "Home");
            }

            if (AuthMode == "register")
            {
                // Validate required fields
                if (isLocal && string.IsNullOrEmpty(CCCD))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập số CCCD/CMND để đăng ký.";
                    return RedirectToAction("Portal", "Home");
                }
                if (!isLocal && string.IsNullOrEmpty(Passport))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập số Hộ chiếu để đăng ký.";
                    return RedirectToAction("Portal", "Home");
                }

                // Chọn Username dựa trên những gì người dùng đã nhập
                string newUsername = isLocal ? Phone : Email;

                // Kiểm tra xem Phone hoặc CCCD đã tồn tại chưa
                var existingUser = isLocal
                    ? db.Users.FirstOrDefault(u => u.Phone == Phone || u.CCCD == CCCD)
                    : db.Users.FirstOrDefault(u => u.Email == Email || u.Passport == Passport);

                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Thông tin đăng ký (SĐT, CCCD hoặc Email) này đã tồn tại trong hệ thống.";
                    return RedirectToAction("Portal", "Home");
                }

                // Đăng ký mới
                var newUser = new User
                {
                    Username = newUsername,
                    Password = Password,
                    FullName = string.IsNullOrEmpty(FullName) ? "Khách hàng" : FullName,
                    Phone = string.IsNullOrEmpty(Phone) ? null : Phone,
                    Email = string.IsNullOrEmpty(Email) ? null : Email,
                    CCCD = string.IsNullOrEmpty(CCCD) ? null : CCCD,
                    Passport = string.IsNullOrEmpty(Passport) ? null : Passport,
                    Balance = 0
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                // Đăng nhập luôn
                var ticket = new FormsAuthenticationTicket(1, newUser.Username, DateTime.Now, DateTime.Now.AddDays(1), false, "User|" + newUser.UserId);
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);

                return RedirectToAction("UserDashboard", "Home");
            }
            else
            {
                // Đăng nhập
                User user = null;
                if (isLocal)
                {
                    user = db.Users.FirstOrDefault(u => u.Phone == Phone && u.Password == Password);
                }
                else
                {
                    user = db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
                }
                
                if (user != null)
                {
                    var ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddDays(1), false, "User|" + user.UserId);
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("UserDashboard", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Thông tin đăng nhập không đúng.";
                    return RedirectToAction("Portal", "Home");
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public JsonResult UpdateProfile(string FullName, string Phone, string Email)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                if (user != null)
                {
                    user.FullName = FullName;
                    user.Phone = Phone;
                    user.Email = Email;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public JsonResult TopUpBalance(decimal amount)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                if (user != null)
                {
                    user.Balance += amount;
                    db.SaveChanges();
                    return Json(new { success = true, newBalance = user.Balance });
                }
            }
            return Json(new { success = false, message = "Không tìm thấy tài khoản" });
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetLinkedAccounts()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                if (user != null)
                {
                    return Json(new {
                        success = true,
                        momo = user.LinkedMoMo,
                        zalo = user.LinkedZaloPay,
                        visa = user.LinkedVisa
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LinkAccount(string method, string accountInfo)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name);
                if (user != null)
                {
                    if (method == "momo") user.LinkedMoMo = accountInfo;
                    else if (method == "zalo") user.LinkedZaloPay = accountInfo;
                    else if (method == "visa") user.LinkedVisa = accountInfo;
                    
                    db.SaveChanges();
                    return Json(new { success = true, method = method, info = accountInfo });
                }
            }
            return Json(new { success = false, message = "Bạn cần đăng nhập để liên kết tài khoản." });
        }
    }
}
