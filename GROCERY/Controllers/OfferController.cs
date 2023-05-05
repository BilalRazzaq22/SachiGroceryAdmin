using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class OfferController : Controller
    {
        OfferRepo offerRepo = new OfferRepo();
        GROCERYEntities GROCERYEntities = new GROCERYEntities();
        private static int CategoryID = 0;
        private static string CategoryName = "";
        private static int SubCategoryID = 0;
        private static string SubCategoryName = "";
        private static int ProductID = 0;
        private static string ProductName = "";
        private USER user;
        // GET: Offer
        public ActionResult Index()
        {
            return View(offerRepo.GetOffers());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(OFFER_MANAGEMENT offer)
        {
            user = (USER)Session["UserLoggedIn"];
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            offer.USER_ID = user.USER_ID;
            offer.CREATED_BY = user.USERNAME;
            offer.CATEGORY_NAME = offer.CATEGORY_NAME;
            offer.SUB_CATEGORY_NAME = offer.SUB_CATEGORY_NAME;
            offer.PRODUCT_NAME = offer.PRODUCT_NAME;
            offer.CREATED_ON = DateTime.Now;
            offerRepo.AddOffer(offer);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            OFFER_MANAGEMENT offer = offerRepo.getOfferById(id);
            if (offer != null)
            {
                ViewBag.CATEGORYID = offer.CATEGORY_ID;
                ViewBag.SUBCATEGORYID = offer.SUB_CATEGORY_ID;
                ViewBag.PRODUCTID = offer.PRODUCT_ID;
                return View(offer);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult Edit(OFFER_MANAGEMENT offer)
        {
            user = (USER)Session["UserLoggedIn"];
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            offer.USER_ID = user.USER_ID;
            offer.UPDATED_BY = user.USERNAME;
            offer.CATEGORY_NAME = offer.CATEGORY_NAME;
            offer.SUB_CATEGORY_NAME = offer.SUB_CATEGORY_NAME;
            offer.PRODUCT_NAME = offer.PRODUCT_NAME;
            offerRepo.UpdateOffer(offer);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            offerRepo.DeleteOffer(id);
            return RedirectToAction("Index");
        }

        public JsonResult SetCategoryName(string name)
        {
            CategoryName = name;
            return Json("Success");
        }

        public JsonResult SetSubCategoryName(string name)
        {
            SubCategoryName = name;
            return Json("Success");
        }

        public JsonResult SetProductName(string name)
        {
            ProductName = name;
            return Json("Success");
        }

        public JsonResult GetProductID(string productID, string name)
        {
            if (productID == null)
            {
                productID = "0";
            }

            ProductID = Convert.ToInt32(productID);
            ProductName = name;
            return Json(ProductID);
        }
    }
}