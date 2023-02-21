using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GROCERY;
using System.Data;
namespace GROCERY.DAL.Core
{
    public class ProductsRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public int addProduct(PRODUCT p)
        {
            try
            {
                p.UNIT_OF_MEASUREMENT = p.UNIT_OF_MEASUREMENT == null ? "NONE" : p.UNIT_OF_MEASUREMENT ;
                //p.CREATED_BY = 1;
                
               // p.CREATED_DATE = DateTime.Now;
                p.MODIFIED_DATE = null;
                                
                PRODUCT obj = gEnt.PRODUCTS.Add(p);
                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;

                var branches = (from b in gEnt.BRANCHES
                                select b).ToList();
                foreach (var item in branches)
                {
                    gEnt.STOCKs.Add(new STOCK
                    {
                        PRODUCT_ID = (int)obj.OLD_PRODUCT_ID,
                        BRANCH_ID = Convert.ToInt16(item.BRANCH_ID),
                        IsActive = true,
                        QTY = 0
                    });
                }

                gEnt.SaveChanges();

                return obj.PRODUCT_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BRANCH> getBranches()
        {
            try
            {
                return (from b in gEnt.BRANCHES
                                select b).ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<STOCK> getStockDetailsByProduct(int pID)
        {
            try
            {
                return (from stk in gEnt.STOCKs
                        where stk.PRODUCT_ID == pID
                                select stk).ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public int updateProduct(PRODUCT p)
        {
            try
            {
                PRODUCT obj = gEnt.PRODUCTS.Find(p.PRODUCT_ID);
                if (obj == null)
                    return -1;
                obj.UNIT_OF_MEASUREMENT = p.UNIT_OF_MEASUREMENT == null ? "NONE" : p.UNIT_OF_MEASUREMENT;
                obj.CREATED_BY = p.CREATED_BY ;
                obj.CREATED_DATE = p.CREATED_DATE;
                obj.OLD_PRODUCT_ID = p.OLD_PRODUCT_ID;
                obj.NAME = p.NAME;
                obj.DESCRIPTION = p.DESCRIPTION;
                obj.AVG_COST = p.AVG_COST;
                obj.IS_ACTIVE = p.IS_ACTIVE;
                obj.IS_FEATURED = p.IS_FEATURED;
                obj.MODIFIED_DATE = DateTime.Now;
                obj.MODIFIED_BY = p.MODIFIED_BY;
               // obj.UNIT_OF_MEASUREMENT = p.UNIT_OF_MEASUREMENT;
                obj.TAG = p.TAG;
                obj.TYPES = p.TYPES;
                obj.FLAVOR = p.FLAVOR;
                obj.BRAND = p.BRAND;
                obj.PRICE = p.PRICE;
                obj.IMPORTED = p.IMPORTED;
                obj.DISCOUNT_PERCENTAGE = p.DISCOUNT_PERCENTAGE;
                obj.COLOR = p.COLOR;
                obj.PACKING = p.PACKING;
                obj.DISCOUNT = p.DISCOUNT;
                obj.SEC_SUB_CATEGORY_A = p.SEC_SUB_CATEGORY_A;
                obj.SEC_SUB_CATEGORY_B = p.SEC_SUB_CATEGORY_B;
                obj.SUB_CATEGORY_ID = p.SUB_CATEGORY_ID;
                obj.PRICE2 = p.PRICE2;
                
                //has image check
                obj.HAS_IMAGE = p.HAS_IMAGE;
                //chaypee
                obj.HAS_THUMBNAIL_IMAGE = p.HAS_IMAGE;

                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;

                
                
                
                return p.PRODUCT_ID;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        //public bool deleteProdut(int productID)
        //{
        //    try
        //    {
        //        PRODUCT obj = gEnt.PRODUCTS.Find(productID);
        //        if (obj == null)
        //            return false;
        //        obj.IS_ACTIVE = false;
        //        obj.UPDATED_ON = DateTime.Now;

        //        int result = gEnt.SaveChanges();
        //        if (result < 1)
        //            return false;
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public PRODUCT getProduct(int productID)
        {
            try
            {
                PRODUCT obj = gEnt.PRODUCTS.Find(productID);
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PRODUCT_IMAGES getProductImages(int productID)
        {
            try
            {
                PRODUCT_IMAGES obj = gEnt.PRODUCT_IMAGES.FirstOrDefault(pi=> pi.PRODUCT_ID == productID);
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int getProductQuantity(int pID)
        {
            try
            {
                var stkQ = (from stock in gEnt.STOCKs
                            where stock.PRODUCT_ID == pID
                            select stock.QTY);
                if (stkQ.Any())
                    return (int)stkQ.Sum();
                else
                    return 0;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool updateStock(List<STOCK> stk, int oldPID)
        {
            try
            {
                var branches = (from s in gEnt.STOCKs
                                where s.PRODUCT_ID == oldPID
                                select s).ToList();
                foreach (var item in branches)
                {
                    var stkObj = (from s in gEnt.STOCKs
                                      where s.BRANCH_ID == item.BRANCH_ID
                                      && s.PRODUCT_ID == oldPID
                                      select s).First();
                    if (stkObj == null)
                        return false;
                    stkObj.QTY = (from sObj in stk 
                                 where sObj.BRANCH_ID==item.BRANCH_ID
                                     select sObj.QTY).First();
                    gEnt.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public int addMiniItem(PRODUCT p,BARCODE b)
        {
            try
            {
                p.UNIT_OF_MEASUREMENT = p.UNIT_OF_MEASUREMENT == null ? "NONE" : p.UNIT_OF_MEASUREMENT;
                
                p.MODIFIED_DATE = null;

                //FIRSTTIME
                //p.OLD_PRODUCT_ID = 2000000;

                p.OLD_PRODUCT_ID = gEnt.PRODUCTS.Max(x => x.OLD_PRODUCT_ID) + 1;

                PRODUCT obj = gEnt.PRODUCTS.Add(p);
                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;

                var branches = (from br in gEnt.BRANCHES
                                select br).ToList();
                foreach (var item in branches)
                {
                    gEnt.STOCKs.Add(new STOCK
                    {
                        PRODUCT_ID = (int)obj.OLD_PRODUCT_ID,
                        BRANCH_ID = Convert.ToInt16(item.BRANCH_ID),
                        IsActive = true,
                        QTY = 1
                    });

                }
                gEnt.SaveChanges();

                DateTime now = DateTime.Now;
                for (short i = 0; i < 7; i++)
                {

                    gEnt.BARCODES.Add(new BARCODE
                    {
                        LOCNO = i,
                        ITEM_CODE = (int)p.OLD_PRODUCT_ID,
                        PACK_CODE = b.PACK_CODE,
                        PACK_DESC = b.PACK_DESC,
                        UNIT = b.UNIT,
                        bDEFAULT = b.bDEFAULT,
                        BAR_CODE= b.BAR_CODE,
                        UNIT_PRICE = b.UNIT_PRICE,
                        DISC_TYPE = b.DISC_TYPE,
                        DISC = b.DISC,
                        CUSER = b.CUSER,
                        MUSER = b.MUSER,

                        CDATE = now,
                        MDATE = now,

                        IsActive = true
                    });

                }
                gEnt.SaveChanges();

                return obj.PRODUCT_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}