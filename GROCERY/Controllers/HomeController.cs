using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class HomeController : Controller
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        private USER user = null;
        public ActionResult Index()
        {

            //Session["UserLoggedIn"] = gEnt.USERS.Find(1); 
            user = (USER)Session["UserLoggedIn"];
            if(user==null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.Title = "Home";


            ViewBag.ProductsCount = gEnt.PRODUCTS.Count();
            ViewBag.UsersCount = gEnt.USERS.Count();
            ViewBag.VendorsCount = gEnt.VENDORS.Count();
            ViewBag.CategoriesCount = gEnt.CATEGORIES.Count();

            return View();
            //return Redirect("/Item/List");
        }

        public ActionResult Login()
        {
            Session["UserLoggedIn"] = gEnt.USERS.Find(1);
            ViewBag.Title = "Login";
            return View();
            //return Redirect("/Item/List");
        }

       
    }
}
