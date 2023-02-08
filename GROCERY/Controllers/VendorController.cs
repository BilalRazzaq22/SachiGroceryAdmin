using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class VendorController : Controller
    {
        //
        // GET: /Vendor/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            try
            {
                ViewBag.Title = "Add Vendor";
                return View();
            }
            catch (System.Exception)
            {
                return View("Error");
            }
        }

        public ActionResult View(int pID)
        {
            return View();
            //try
            //{
            //    ViewBag.Title = "View Product";
            //    var obj = rp.getProdut(pID);
            //    if (obj == null)
            //        return View("Error");
            //    return View(obj);
            //}
            //catch (System.Exception)
            //{
            //    return View("Error");
            //}
        }
	}
}