using System.Collections.Generic;
using System.Web.Mvc;
using SaigonBus.Models;

namespace SaigonBus.Controllers
{
    public class FeedbacksController
        : Controller
    {
        public ActionResult Index()
        {
            List<Feedback> feedbacks =
                new List<Feedback>()
            {
                new Feedback
                {
                    Username = "Quan",
                    Note =
                    "Ứng dụng đặt xe rất tiện",

                    Rating = 5
                },

                new Feedback
                {
                    Username = "An",
                    Note =
                    "Giao diện đẹp và dễ dùng",

                    Rating = 4
                }
            };

            return View(feedbacks);
        }
    }
}
