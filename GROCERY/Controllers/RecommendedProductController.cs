using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class RecommendedProductController : Controller
    {
        GROCERYEntities GROCERYEntities = new GROCERYEntities();
        RecommendedProductRepo rpRepo = new RecommendedProductRepo();
        private static RECOMMENDED_PRODUCTS rp = new RECOMMENDED_PRODUCTS();
        // GET: RecommendedProduct
        public ActionResult Index()
        {
            return View(rpRepo.GetRecommendedProducts());
        }

        [HttpPost]
        public ActionResult Create(int productId)
        {
            rp.TYPE = "Manual";
            rp.CREATED_ON = DateTime.Now;
            rp.PRODUCT_ID = productId;
            rpRepo.AddRecommendedProduct(rp);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            rpRepo.DeleteRecommendedProduct(id);
            return RedirectToAction("Index");
        }

        public JsonResult GetProduct(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            int idd = Convert.ToInt32(id);

            PRODUCT p = GROCERYEntities.PRODUCTS.FirstOrDefault(x => x.PRODUCT_ID == idd);
            if (p != null)
            {
                rp.PRODUCT_ID = p.PRODUCT_ID;
                rp.PRODUCT_NAME = p.NAME;
                rp.PRICE = p.PRICE;
                rp.PACKING = p.PACKING;
            }


            //(from slist in GROCERYEntities.PRODUCTS
            //            where (slist.PRODUCT_ID == idd)
            //            select new { slist.PRODUCT_ID, slist.DESCRIPTION }).FirstOrDefault();
            //ViewBag.Products = new SelectList(prod, "PRODUCT_ID", "DESCRIPTION");
            return Json(p);
        }
    }
}