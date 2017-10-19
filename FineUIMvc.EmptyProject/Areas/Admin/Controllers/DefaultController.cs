using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FineUIMvc.PumpMVC.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Admin/Default/
        public ActionResult Index()
        {
            return View();
        }
	}
}