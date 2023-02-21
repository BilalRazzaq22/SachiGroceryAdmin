using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class CouponsRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();


        public bool addCoupon(COUPON coupon, int noOfCoupons)
        {
            try
            {
                coupon.CREATED_ON = System.DateTime.Now;
                coupon.CREATED_BY = 1;

                List<int> listNumbers = new List<int>();
                int number;


                for (int i = 0; i < noOfCoupons; i++)
                {
                    do
                    {
                        number = (new Random()).Next(1000, 9999);
                    } while (listNumbers.Contains(number));
                    listNumbers.Add(number);
                    coupon.CODE = number;
                    coupon.PROMO = coupon.PROMO_TEXT.ToUpper() + coupon.CODE;
                    gEnt.COUPONS.Add(coupon);
                    if (gEnt.SaveChanges() < 1)
                        return false;
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<COUPON> getAllCoupons()
        {
            try
            {
                var q = from c in gEnt.COUPONS
                        where c.IS_ACTIVE == true
                        select c;
                //if (q.Any())
                //{
                //    return q.ToList();
                //}
                return q.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<COUPON> getAllCouponsByID(int ID)
        {
            try
            {
                var q = from c in gEnt.COUPONS
                        where c.COUPON_ID == ID
                        && c.IS_ACTIVE == true
                        && c.IS_USED == false
                        //&& c.CREATED_ON.Value.Day == DateTime.Now.Day
                        //&& c.CREATED_ON.Value.Month == DateTime.Now.Month
                        //&& c.CREATED_ON.Value.Year == DateTime.Now.Year
                        select c;
                if (q.Any())
                {
                    return q.ToList();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public COUPON getCouponByCode(string code)
        {
            try
            {
                var q = from c in gEnt.COUPONS
                        where c.PROMO == code.ToUpper()
                        && c.IS_ACTIVE == true && c.IS_USED == false
                        && c.EXPIRY_DATE >= DateTime.Now && c.START_DATE <= DateTime.Now
                        select c;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public COUPON getCouponById(int cID)
        {
            try
            {
                var q = from c in gEnt.COUPONS
                        where c.COUPON_ID == cID
                        && c.IS_ACTIVE == true
                        select c;
                if (q.Any())
                {
                    return q.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool updateCouponStatus(int ID)
        {
            try
            {
                var c = gEnt.COUPONS.FirstOrDefault(x => x.COUPON_ID == ID);
                c.IS_USED = true;
                c.IS_ACTIVE = false;
                c.updated_by = 1;
                c.updated_on = DateTime.Now;
                int res = gEnt.SaveChanges();
                return res > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool updateCoupon(COUPON coupon)
        {
            try
            {
                COUPON cpn = getCouponById(coupon.COUPON_ID);
                if (cpn == null)
                    return false;
                cpn.VALUE = coupon.VALUE;
                cpn.UNLOCK_AMOUNT = coupon.UNLOCK_AMOUNT;
                cpn.IS_ACTIVE = coupon.IS_ACTIVE;
                cpn.IS_USED = coupon.IS_USED;
                cpn.updated_by = 1;
                cpn.updated_on = DateTime.Now;
                cpn.COUPONTYPE = coupon.COUPONTYPE;
                cpn.CATEGORYID = (coupon.CATEGORYID == -1 || coupon.CATEGORYID == null) ? null : coupon.CATEGORYID;
                cpn.SUBCATEGORYID = (coupon.SUBCATEGORYID == -1 || coupon.SUBCATEGORYID == null) ? null : coupon.SUBCATEGORYID;
                cpn.PRODUCTID = (coupon.PRODUCTID == -1 || coupon.PRODUCTID == null) ? null : coupon.PRODUCTID;
                //cpn.IsCartBased = (coupon.COUPONTYPE == "No Type Selected") ? true : false;
                int res = gEnt.SaveChanges();
                return res > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}