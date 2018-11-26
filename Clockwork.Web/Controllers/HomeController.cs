using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Clockwork.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
            var timeZones = new List<string>();
            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
                timeZones.Add(zone.Id);
            ViewData["TimeZones"] = timeZones;
            return View();
        }
    }
}
