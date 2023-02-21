using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GROCERY.Models;
using GROCERY.DAL.Core;

namespace GROCERY.Controllers
{
    public class CouponController : Controller
    {
        private CouponsRepo cRepo = new CouponsRepo();
        private ProductsRepo productsRepo = new ProductsRepo();
        GROCERYEntities GROCERYEntities = new GROCERYEntities();
        private static int CategoryID = 0;
        private static int SubCategoryID = 0;
        private static int ProductID = 0;
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                return View(cRepo.getAllCoupons());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(cRepo.getAllCouponsByID(id));
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: /Coupon/Details/5
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COUPON coupon = db.COUPONS.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }*/

        // GET: /Coupon/Create
        public ActionResult Create()
        {
            try
            {
                //ViewBag.Categories = new SelectList(GROCERYEntities.CATEGORIES, "CATEGORY_ID", "DESCRIPTION");
                //ViewBag.SubCategories = new SelectList(GROCERYEntities.SUB_CATEGORIES, "SUB_CATEGORY_ID", "DESCRIPTION");
                //ViewBag.Products = new SelectList(productsRepo.GetAllProducts(), "PRODUCT_ID", "DESCRIPTION");
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: /Coupon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(COUPONSBO couponObj)
        {
            try
            {
                COUPON coupon = new COUPON
                {
                    COUPON_ID = couponObj.COUPON_ID,
                    PROMO_TEXT = couponObj.PROMO_TEXT,
                    START_DATE = couponObj.START_DATE,
                    EXPIRY_DATE = couponObj.EXPIRY_DATE,
                    IS_ACTIVE = true,
                    IS_USED = false,
                    UNLOCK_AMOUNT = couponObj.UNLOCK_AMOUNT,
                    VALUE = couponObj.VALUE,
                    COUPONTYPE = couponObj.COUPONTYPE,
                    CATEGORYID = (couponObj.CATEGORYID == -1 || couponObj.CATEGORYID == null) ? null : couponObj.CATEGORYID,
                    SUBCATEGORYID = (couponObj.SUBCATEGORYID == -1 || couponObj.SUBCATEGORYID == null) ? null : couponObj.SUBCATEGORYID,
                    PRODUCTID = (couponObj.PRODUCTID == -1 || couponObj.PRODUCTID == null) ? null : couponObj.PRODUCTID,
                    //IsCartBased = (couponObj.COUPONTYPE == "No Type Selected") ? true : false
                };

                if (ModelState.IsValid)
                {
                    cRepo.addCoupon(coupon, couponObj.count_coupons);
                    //return RedirectToAction("Index");
                }

                return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Update(COUPON copn)
        {
            try
            {
                bool flag = cRepo.updateCoupon(copn);
                if (!flag)
                    return Redirect("/Coupon/Index");
                return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: /Coupon/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                COUPON coupon = cRepo.getCouponById(id);
                if (coupon == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CATEGORYID = coupon.CATEGORYID;
                ViewBag.SUBCATEGORYID = coupon.SUBCATEGORYID;
                ViewBag.PRODUCTID = coupon.PRODUCTID;
                return View(coupon);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                bool flag = cRepo.updateCouponStatus(id);
                if (flag)
                    return Redirect("/Coupon/Index");
                return View("Error");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        public ActionResult Print()
        {
            return View();
        }
        /*
        // POST: /Coupon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="COUPON_ID,PROMO_TEXT,START_DATE,EXPIRY_DATE,VALUE,CREATED_ON,CREATED_BY,IS_ACTIVE,IS_USED")] COUPON coupon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coupon);
        }

        // GET: /Coupon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COUPON coupon = db.COUPONS.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: /Coupon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            COUPON coupon = db.COUPONS.Find(id);
            db.COUPONS.Remove(coupon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        public JsonResult GetSubCategory(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            CategoryID = Convert.ToInt32(id);

            var subcat = (from slist in GROCERYEntities.SUB_CATEGORIES
                          where (slist.CATEGORY_ID == CategoryID)
                          select new { slist.SUB_CATEGORY_ID, slist.DESCRIPTION }).ToList();
            ViewBag.SubCategories = new SelectList(subcat, "SUB_CATEGORY_ID", "DESCRIPTION");
            return Json(new SelectList(subcat, "SUB_CATEGORY_ID", "DESCRIPTION"));
        }

        public JsonResult GetProduct(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            SubCategoryID = Convert.ToInt32(id);

            var prod = (from slist in GROCERYEntities.PRODUCTS
                        where (slist.SUB_CATEGORY_ID == SubCategoryID)
                        select new { slist.PRODUCT_ID, slist.DESCRIPTION }).ToList();
            ViewBag.Products = new SelectList(prod, "PRODUCT_ID", "DESCRIPTION");
            return Json(new SelectList(prod, "PRODUCT_ID", "DESCRIPTION"));
        }

        public JsonResult GetProductID(string id)
        {
            if (id == null)
            {
                id = "0";
            }

            ProductID = Convert.ToInt32(id);
            return Json(ProductID);
        }
    }
}
