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

        public ActionResult Details(string id)
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
        [ValidateAntiForgeryToken]
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
                        VALUE = couponObj.VALUE
                    };

                if (ModelState.IsValid)
                {
                    cRepo.addCoupon(coupon, couponObj.count_coupons);
                    return Redirect("/Coupon/Details?id="+coupon.PROMO_TEXT);
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
                if (flag)
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
    }
}
