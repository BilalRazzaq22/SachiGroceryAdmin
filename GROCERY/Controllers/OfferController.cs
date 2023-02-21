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
            //ViewBag.Categories = new SelectList(GROCERYEntities.CATEGORIES, "CATEGORY_ID", "DESCRIPTION");
            //ViewBag.SubCategories = new SelectList(GROCERYEntities.SUB_CATEGORIES, "SUB_CATEGORY_ID", "DESCRIPTION");
            //ViewBag.Products = new SelectList(GROCERYEntities.PRODUCTS, "PRODUCT_ID", "DESCRIPTION");
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
            offer.CATEGORY_NAME = CategoryName;
            offer.SUB_CATEGORY_NAME = SubCategoryName;
            offer.PRODUCT_NAME = ProductName;
            offer.CREATED_ON = DateTime.Now;
            offerRepo.AddOffer(offer);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            //ViewBag.Categories = new SelectList(GROCERYEntities.CATEGORIES, "CATEGORY_ID", "DESCRIPTION");
            //ViewBag.SubCategories = new SelectList(GROCERYEntities.SUB_CATEGORIES, "SUB_CATEGORY_ID", "DESCRIPTION");
            //ViewBag.Products = new SelectList(GROCERYEntities.PRODUCTS, "PRODUCT_ID", "DESCRIPTION");
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
            //offer.CATEGORY_NAME = CategoryName;
            //offer.SUB_CATEGORY_NAME = SubCategoryName;
            //offer.PRODUCT_NAME = ProductName;
            offerRepo.UpdateOffer(offer);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            offerRepo.DeleteOffer(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetSubCategory(string categoryID, string name)
        {
            if (categoryID == null)
            {
                categoryID = "0";
            }

            CategoryID = Convert.ToInt32(categoryID);
            CategoryName = name;

            var subcat = (from slist in GROCERYEntities.SUB_CATEGORIES
                          where (slist.CATEGORY_ID == CategoryID)
                          select new { slist.SUB_CATEGORY_ID, slist.DESCRIPTION }).ToList();
            ViewBag.SubCategories = new SelectList(subcat, "SUB_CATEGORY_ID", "DESCRIPTION");
            return Json(new SelectList(subcat, "SUB_CATEGORY_ID", "DESCRIPTION"));
        }

        public JsonResult GetProduct(string subCategoryID, string name)
        {
            if (subCategoryID == null)
            {
                subCategoryID = "0";
            }

            SubCategoryID = Convert.ToInt32(subCategoryID);
            SubCategoryName = name;

            var prod = (from slist in GROCERYEntities.PRODUCTS
                        where (slist.SUB_CATEGORY_ID == SubCategoryID)
                        select new { slist.PRODUCT_ID, slist.DESCRIPTION }).ToList();
            ViewBag.Products = new SelectList(prod, "PRODUCT_ID", "DESCRIPTION");
            return Json(new SelectList(prod, "PRODUCT_ID", "DESCRIPTION"));
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