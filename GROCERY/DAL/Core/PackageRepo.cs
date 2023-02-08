using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class PackageRepo:DABase
    {
        GROCERYEntities gEnt = new GROCERYEntities();
        const string QRY_GET_PACKAGE_DETAIL = @"select PP.PACKAGE_ID,PP.PRODUCT_ID,PP.QUANTITY,p.NAME
                                                ,bc.UNIT_PRICE,bc.DISC,(bc.UNIT_PRICE-bc.DISC)* PP.QUANTITY as Total,bc.BAR_CODE
                                                from PACKAGE_PRODUCTS PP 
                                                inner join PACKAGES pg on PP.PACKAGE_ID = pg.PACKAGE_ID
                                                inner join BARCODES bc ON PP.BAR_CODE = bc.BAR_CODE and pg.BRANCH_ID = bc.LOCNO
                                                inner join PRODUCTS p on bc.ITEM_CODE = p.OLD_PRODUCT_ID
                                                where pg.BRANCH_ID = {0} and PP.PACKAGE_ID = {1} and bc.bDEFAULT = 1 and bc.IsActive=1";
        public int addPackage(PACKAGE pkg)
        {
            try
            {
                pkg.IS_ACTIVE = true;
                pkg.CREATED_ON = DateTime.Now;
                
                PACKAGE obj = gEnt.PACKAGES.Add(pkg);
                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;
                return obj.PACKAGE_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool generatePackageProduct(List<CallOrders> items, int packageId)
        {
            for (int i = 0; i < items.Count; i++)
            {
                gEnt.PACKAGE_PRODUCTS.Add(new PACKAGE_PRODUCTS
                {
                    PACKAGE_ID = packageId,
                    PRODUCT_ID = items[i].product_id,
                    QUANTITY = items[i].amountOrdered,
                    BAR_CODE = items[i].barcode,
                    IS_ACTIVE = 1
                });
            }

            int result = gEnt.SaveChanges();
            return result > 0;
        }


        public List<PACKAGE> getAllPackages()
        {
            try
            {
                var q = from c in gEnt.PACKAGES
                       
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
        public PACKAGE getPackage(int pID)
        {
            try
            {
                var q = from p in gEnt.PACKAGES
                        where p.PACKAGE_ID == pID
                        select p;
                return q.Any() ? q.FirstOrDefault() : null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CallOrders> getPackageDetails(int pID, int bID)
        {
            try
            {
                DataTable dtTable = ExecuteDataSet(string.Format(QRY_GET_PACKAGE_DETAIL, bID, pID)).Tables[0];
                List<CallOrders> list = new List<CallOrders>();
                foreach (DataRow row in dtTable.Rows)
                {
                    CallOrders co = new CallOrders();

                    co.product_id = int.Parse(row[1].ToString());
                    co.amountOrdered = int.Parse(row[2].ToString());
                    co.product_name = row[3].ToString();
                    //co.availableQty = decimal.Parse(row[4].ToString());
                    co.price = decimal.Parse(row[4].ToString());
                    co.disc = decimal.Parse(row[5].ToString());
                    co.total = decimal.Parse(row[6].ToString());
                    co.barcode = row[7].ToString();
                    list.Add(co);
                }
                if (list.Count == 0)
                    return null;
                else
                    return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool updatePackage(PACKAGE p, List<CallOrders> listPackageProducts)
        {
            try
            {
                var orderObj = getPackage(p.PACKAGE_ID);

                orderObj.NAME = p.NAME;
                orderObj.DESCRIPTION = p.DESCRIPTION;

                orderObj.UPDATED_BY = p.UPDATED_BY;
                orderObj.UPDATED_ON = DateTime.Now;

                int result = gEnt.SaveChanges();
                if (result < 1)
                    return false;

                var qOP = from oP in gEnt.PACKAGE_PRODUCTS
                          where oP.PACKAGE_ID == p.PACKAGE_ID
                          select oP;
                //if (!qOP.Any())
                //    return false;

                foreach (var item in qOP.ToList())
                {
                    gEnt.PACKAGE_PRODUCTS.Remove(item);
                }
                gEnt.SaveChanges();
                return generatePackageProduct(listPackageProducts, p.PACKAGE_ID);

                
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}