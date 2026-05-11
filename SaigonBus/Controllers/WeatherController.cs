using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SaigonBus.Controllers
{
    public class WeatherController : Controller
    {
        public async Task<JsonResult> IsRaining(double lat, double lon)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";

                var json = await client.GetStringAsync(url);
                var data = JObject.Parse(json);

                int weatherCode = (int)data["current_weather"]["weathercode"];

                bool isRaining = weatherCode == 61 || weatherCode == 63 || weatherCode == 65
                              || weatherCode == 80 || weatherCode == 81 || weatherCode == 82
                              || weatherCode == 95 || weatherCode == 96 || weatherCode == 99;

                return Json(new
                {
                    isRaining = isRaining,
                    code = weatherCode
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}