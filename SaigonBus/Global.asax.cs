using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;
using System.Data.Entity;
using SaigonBus.Models;
using SaigonBus.Migrations;

namespace SaigonBus
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Web.Helpers.AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            // Đảm bảo các cột mới tồn tại trong Database (Fix lỗi SqlException)
            using (var db = new SaigonRideContext())
            {
                db.Database.ExecuteSqlCommand("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Vehicles') AND name = 'ReservedUntil') ALTER TABLE dbo.Vehicles ADD ReservedUntil DateTime");
                db.Database.ExecuteSqlCommand("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Vehicles') AND name = 'ReservedByUserId') ALTER TABLE dbo.Vehicles ADD ReservedByUserId Int");
                db.Database.ExecuteSqlCommand("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.RentalTransactions') AND name = 'GroupRentalId') ALTER TABLE dbo.RentalTransactions ADD GroupRentalId Int");
            }

            // Tự động cập nhật Database lên phiên bản mới nhất khi khởi động ứng dụng
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SaigonRideContext, Configuration>());
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        string[] roles = new string[1];
                        // Nếu UserData bắt đầu bằng "User|", thì Role là "User"
                        if (!string.IsNullOrEmpty(authTicket.UserData))
                        {
                            if (authTicket.UserData.StartsWith("User|"))
                            {
                                roles[0] = "User";
                            }
                            else if (authTicket.UserData == "Admin")
                            {
                                roles[0] = "Admin";
                            }
                            else
                            {
                                roles[0] = "";
                            }
                        }

                        GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);
                        Context.User = userPrincipal;
                    }
                }
                catch (Exception)
                {
                    // Ignore decryption errors
                }
            }
        }
    }
}
