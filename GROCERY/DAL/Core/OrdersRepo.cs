using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
namespace GROCERY.DAL.Core
{
    public class OrdersRepo : DABase
    {
        GROCERYEntities gEnt = new GROCERYEntities();
        CouponsRepo cRepo = new CouponsRepo();
        public DateTime currentdate;
        public OrdersRepo()
        {
            DateTime date1 = DateTime.UtcNow;
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            currentdate = TimeZoneInfo.ConvertTime(date1, tz);

        }
        const string QRY_GET_ORDER_DETAIL = @"select op.ORDER_ID,op.PRODUCT_ID,op.QUANTITY,p.NAME
                                            ,bc.UNIT_PRICE,bc.DISC,(bc.UNIT_PRICE-bc.DISC)* op.QUANTITY as Total,bc.BAR_CODE
                                            from ORDER_PRODUCTS op 
                                            inner join ORDERS o on op.ORDER_ID = o.ORDER_ID
                                            inner join BARCODES bc ON op.BAR_CODE = bc.BAR_CODE and o.BRANCH_ID = bc.LOCNO
                                            inner join PRODUCTS p on bc.ITEM_CODE = p.OLD_PRODUCT_ID
                                            where o.BRANCH_ID = {0} and op.ORDER_ID = {1} and bc.bDEFAULT = 1 and bc.IsActive=1";
        //        select op.ORDER_ID,op.PRODUCT_ID,op.QUANTITY,p.NAME,s.QTY,bc.UNIT_PRICE,bc.DISC,(bc.UNIT_PRICE-bc.DISC)* op.QUANTITY as Total,bc.BAR_CODE
        //from ORDER_PRODUCTS op 
        //inner join ORDERS o on op.ORDER_ID = o.ORDER_ID
        //inner join BARCODES bc ON op.BAR_CODE = bc.BAR_CODE and o.BRANCH_ID = bc.LOCNO
        //inner join PRODUCTS p on bc.ITEM_CODE = p.PRODUCT_ID
        //inner join STOCK S on o.BRANCH_ID =  s.BRANCH_ID and p.PRODUCT_ID = s.PRODUCT_ID


        public int addOrder(ORDER order)
        {
            try
            {
                order.IS_ACTIVE = 1; //not rejected
                order.CREATED_BY = 1;
                order.CREATED_ON = currentdate;
                ORDER obj = gEnt.ORDERS.Add(order);
                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;
                return obj.ORDER_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool updateOrder(ORDER order, List<CallOrders> listOrderProducts, string code, decimal amount)
        {
            try
            {
                var res = getOrder(order.ORDER_ID);
                if (res != null && res.item != null)
                {
                    var orderObj = res.item;
                    if (orderObj.COUPON_ID == -1)
                    {
                        var coupon = cRepo.getCouponByCode(code);

                        if (coupon == null)
                            coupon = new COUPON()
                            {
                                COUPON_ID = -1
                            };
                        else
                            if (amount < coupon.UNLOCK_AMOUNT)
                            coupon = new COUPON()
                            {
                                COUPON_ID = -1
                            };
                        else
                                if (!cRepo.updateCouponStatus(coupon.COUPON_ID))
                            return false;
                        orderObj.COUPON_ID = coupon.COUPON_ID;
                        orderObj.COUPON_DISCOUNT = coupon.VALUE;
                    }


                    orderObj.MOBILE = order.MOBILE;
                    orderObj.NAME = order.NAME;
                    orderObj.DELIVERY_DESCRIPTION = order.DELIVERY_DESCRIPTION;
                    orderObj.MANUAL_DISCOUNT = order.MANUAL_DISCOUNT;
                    orderObj.UPDATED_ON = currentdate;
                    orderObj.ADDRESS = order.ADDRESS;
                    orderObj.PAYMENT_MODE_ID = order.PAYMENT_MODE_ID;
                    orderObj.IS_PACKAGE = order.IS_PACKAGE;
                    orderObj.DELIVERY_TIME = order.DELIVERY_TIME;

                    orderObj.UPDATED_BY = order.UPDATED_BY;

                    int result = gEnt.SaveChanges();
                    if (result < 1)
                        return false;

                    var qOP = from oP in gEnt.ORDER_PRODUCTS
                              where oP.ORDER_ID == order.ORDER_ID
                              select oP;
                    if (!qOP.Any())
                        return false;
                    //var orderedProducts = qOP.ToList();

                    foreach (var item in qOP.ToList())
                    {
                        gEnt.ORDER_PRODUCTS.Remove(item);
                    }
                    gEnt.SaveChanges();
                    return generateProductOrders(listOrderProducts, order.ORDER_ID);

                    //foreach (var item in listOrderProducts)
                    //{
                    //    var query = from oPs in orderedProducts
                    //                where oPs.BAR_CODE == item.barcode && oPs.IS_ACTIVE == 1
                    //                select oPs;
                    //    if (query.Any())
                    //    {
                    //        var itemP = query.FirstOrDefault();
                    //        if (!revertStock(item.product_id, (int)itemP.QUANTITY, (int)orderObj.BRANCH_ID))
                    //            return false;

                    //        var query2 = from oP in gEnt.ORDER_PRODUCTS
                    //                     where oP.ORDER_ID == order.ORDER_ID
                    //                     && oP.PRODUCT_ID == item.product_id
                    //                     select oP;
                    //        if (query2.Any() != false)
                    //        {
                    //            var updatedOrderProduct = query2.FirstOrDefault();
                    //            if (item.product_name == "-1")      //item is removed from list
                    //            {
                    //                updatedOrderProduct.IS_ACTIVE = 0;
                    //                updatedOrderProduct.UPDATED_ON = DateTime.Now;
                    //                updatedOrderProduct.UPDATED_BY = 1;

                    //                if (gEnt.SaveChanges() < 1)
                    //                    return false;
                    //            }
                    //            else
                    //            {
                    //                updatedOrderProduct.QUANTITY = item.amountOrdered;
                    //                updatedOrderProduct.UPDATED_ON = DateTime.Now;
                    //                updatedOrderProduct.UPDATED_BY = 1;
                    //                if (!revertStock(item.product_id, -1 * item.amountOrdered, (int)orderObj.BRANCH_ID)) //using as update function to updateStock
                    //                    return false;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            return false;
                    //        }

                    //    }
                    //    else
                    //    {

                    //        //new item added in order list
                    //        if (item.product_name != "-1")
                    //        {
                    //            gEnt.ORDER_PRODUCTS.Add(new ORDER_PRODUCTS
                    //                {
                    //                    ORDER_ID = order.ORDER_ID,
                    //                    IS_ACTIVE = 1,
                    //                    QUANTITY = item.amountOrdered,
                    //                    PRODUCT_ID = item.product_id,
                    //                    BAR_CODE = item.barcode
                    //                });
                    //            if (gEnt.SaveChanges() < 1)
                    //                return false;
                    //            if (!revertStock(item.product_id, -1 * item.amountOrdered, (int)orderObj.BRANCH_ID)) //using as update function to updateStock
                    //                return false;
                    //        }
                    //    }
                    //}
                    //return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool rejectOrder(ORDER order)
        {
            try
            {
                var tempOrd = (from o in gEnt.ORDERS
                               where o.ORDER_ID == order.ORDER_ID
                               select o).FirstOrDefault();
                tempOrd.IS_ACTIVE = 0; // rejected
                tempOrd.REJECTED_BY = 1;
                tempOrd.REJECTED_REASON = order.REJECTED_REASON;

                tempOrd.UPDATED_ON = currentdate;
                tempOrd.UPDATED_BY = 1;

                var oQ = from oP in gEnt.ORDER_PRODUCTS
                         where oP.ORDER_ID == order.ORDER_ID
                         select oP;
                if (oQ.Any())
                {
                    var list = oQ.ToList();
                    foreach (var item in list)
                    {
                        item.IS_ACTIVE = 0;
                        item.UPDATED_ON = currentdate;
                        item.UPDATED_BY = 1;

                        var oldProductID = from p in gEnt.PRODUCTS
                                           where p.PRODUCT_ID == item.PRODUCT_ID
                                           select p.OLD_PRODUCT_ID;
                        if (!oldProductID.Any())
                            return false;
                        if (!revertStock((int)oldProductID.FirstOrDefault(), (int)item.QUANTITY, (int)tempOrd.BRANCH_ID))
                            return false;
                    }
                }

                int result = gEnt.SaveChanges();
                if (result < 1)
                    return false;
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool generateProductOrders(List<CallOrders> orders, int orderID)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                gEnt.ORDER_PRODUCTS.Add(new ORDER_PRODUCTS
                {
                    ORDER_ID = orderID,
                    PRODUCT_ID = orders[i].product_id,
                    QUANTITY = orders[i].amountOrdered,
                    BAR_CODE = orders[i].barcode,
                    IS_ACTIVE = 1
                });
            }

            int result = gEnt.SaveChanges();
            return result > 0;
        }



        public OrderViewModel getOrder(int oID)
        {
            try
            {
                var q = (from o in gEnt.ORDERS
                         from u in gEnt.USERS.Where(x => x.USER_ID == o.CUSTOMER_ID).DefaultIfEmpty()
                         where o.ORDER_ID == oID
                         select new OrderViewModel()
                         {
                             item = o,
                             Email = u == null ? "" : (u.EMAIL ?? "")
                         }
                       ).FirstOrDefault();
                return q;

                //var q = from o in gEnt.ORDERS
                //        where o.ORDER_ID == oID
                //        select o;
                //return q.Any() ? q.FirstOrDefault() : null;

                //ORDER o = gEnt.ORDERS.SingleOrDefault(m => m.ORDER_ID == oID);

                //ORDER o = gEnt.ORDERS.Find(oID);

                //ORDER o = gEnt.ORDERS.SqlQuery("Select * from Orders where ORDER_ID=@id", new SqlParameter("@id", oID))
                //    .FirstOrDefault();
                //return o;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CallOrders> getOrderDetails(int oID, int bID)
        {
            try
            {
                DataTable dtTable = ExecuteDataSet(string.Format(QRY_GET_ORDER_DETAIL, bID, oID)).Tables[0];
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


        public bool revertStock(int oPID, int stockAmount, int branchID)
        {
            try
            {
                var stockQ = from stk in gEnt.STOCKs
                             where stk.BRANCH_ID == branchID &&
                             stk.PRODUCT_ID == oPID
                             select stk;
                if (!stockQ.Any())
                    return false;

                var stockEntity = stockQ.FirstOrDefault();
                stockEntity.QTY += stockAmount;
                //stockEntity.u

                return gEnt.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<USER> getRiders()
        {
            try
            {
                var q = from r in gEnt.USERS
                        where r.USER_TYPE == 5
                        && r.IS_AVAILABLE == 1
                        && r.IS_ACTIVE == true
                        select r;
                return q.Any() ? q.ToList() : new List<USER>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ORDER> getAllOrders()
        {
            try
            {
                var ordersQ = from o in gEnt.ORDERS
                              select o;
                return ordersQ.Any() ? ordersQ.OrderByDescending(x => x.CREATED_ON).Take(0).ToList() : null;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public List<ORDER> getAllCustomerOrders(int uId)
        {
            try
            {
                var ordersQ = from o in gEnt.ORDERS
                              where o.CUSTOMER_ID == uId
                              select o;
                return ordersQ.Any() ? ordersQ.OrderByDescending(x => x.CREATED_ON).ToList() : null;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void SaveTransactionNumber(string paymentModeId, string transactionNumber)
        {
            ONLINE_TRANSACTION onlineTrans = new ONLINE_TRANSACTION()
            {
                PAYMENT_MODE_ID = Convert.ToInt32(paymentModeId),
                TRANSACTION_NUMBER = transactionNumber
            };

            gEnt.ONLINE_TRANSACTION.Add(onlineTrans);
            gEnt.SaveChanges();
        }
    }
}