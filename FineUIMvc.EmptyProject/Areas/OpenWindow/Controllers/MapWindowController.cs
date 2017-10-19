using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.OpenWindow.Controllers
{
    [Authorize]
    public class MapWindowController : Controller
    {
        //
        // GET: /OpenWindow/MapWindow/
        public ActionResult Index(string LngLat)
        {
            ViewBag.txtLngLat = LngLat;
            return View();
        }
	}
}