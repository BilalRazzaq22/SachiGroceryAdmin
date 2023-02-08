using GROCERY.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Managers
{
    public class OrderManger : DABase
    {
        public DataSet getAllOrderStatuses()
        {
            return ExecuteDataSet("select * from ORDER_STATUSES where ORDER_STATUS_ID IN (1,2,3,4,5)");
        }

        public DataSet getOrders(int oStID, string oDateFrom, string oDateTo)
        {
            string query = "";
            if (oStID == 0)
            {
                query = @"SELECT o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE, o.ADDRESS, o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,o.ADDED_BY,
                        case when STATUS = 1 then 'Pending' when STATUS = 2 then 'In Process' when STATUS = 3 then 'Dispatched' when STATUS = 4 then 'Delivered' when STATUS = 5 then 'Bounced' when STATUS = 6 then 'Rejected By Manager' when STATUS = 7 then 'Rejected by Rider'  ELSE 'none' end as STATUS_DESCRIPTION,
                        case when o.IS_PACKAGE = 1 then 'Yes' else 'No' end as IS_PACKAGE,       SUM(op.QUANTITY) TOTAL_ITEMS,
                        CEILING(SUM((b.UNIT_PRICE - b.DISC)  * op.QUANTITY)) - isnull(o.MANUAL_DISCOUNT,0) - isnull(o.COUPON_DISCOUNT,0) as TOTAL_AMOUNT      
                        FROM ORDER_PRODUCTS OP 
                        INNER JOIN PRODUCTS P on op.PRODUCT_ID = P.OLD_PRODUCT_ID
                        inner join BARCODES b on OP.PRODUCT_ID = B.ITEM_CODE
                        INNER JOIN ORDERS O ON OP.ORDER_ID = O.ORDER_ID
                        where (o.created_on > DATEADD(day,-60,getdate())) and ( B.bDefault =1 and  B.IsActive =1 and b.LOCNO = o.BRANCH_ID and  OP.IS_ACTIVE =1 )
                        GROUP BY o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE,o.MANUAL_DISCOUNT, o.ADDRESS,o.Status,o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,o.IS_PACKAGE,o.ADDED_BY";
                //query = "SELECT " +
                //       " case when STATUS = 1 then 'Pending' when STATUS = 2 then 'In Process' when STATUS = 3 then 'Dispatched' when STATUS = 4 then 'Delivered' when STATUS = 5 then 'Bounced' when STATUS = 6 then 'Rejected By Manager' when STATUS = 7 then 'Rejected by Rider'  ELSE 'none' end as STATUS_DESCRIPTION, " +
                //       "o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE, o.ADDRESS, " +
                //       "o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,case when o.IS_PACKAGE = 1 then 'Yes' else 'No' end as IS_PACKAGE," +
                //        "       SUM(op.QUANTITY) TOTAL_ITEMS, " +
                //         "      CEILING(SUM((b.UNIT_PRICE - b.DISC)  * op.QUANTITY)) - isnull(o.MANUAL_DISCOUNT,0) - isnull(o.COUPON_DISCOUNT,0) as TOTAL_AMOUNT " +
                //          "     FROM " +
                //           "    ORDERS o, ORDER_PRODUCTS op, PRODUCTS p, BARCODES b " +
                //            "   WHERE  " +
                //             "  o.ORDER_ID = op.ORDER_ID  " +
                //              " AND op.BAR_CODE = b.bar_code " +
                //               " and b.ITEM_CODE = p.OLD_PRODUCT_ID AND O.BRANCH_ID = B.LOCNO" +
                //             " AND o.IS_ACTIVE = 1 " +
                //              " GROUP BY " +
                //              "o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE,o.MANUAL_DISCOUNT, o.ADDRESS,o.Status,o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,o.IS_PACKAGE order by o.ORDER_ID desc";
            }
            else
                query = "SELECT " +
                    " case when STATUS = 1 then 'Pending' when STATUS = 2 then 'In Process' when STATUS = 3 then 'Dispatched' when STATUS = 4 then 'Delivered' when STATUS = 5 then 'Bounced' when STATUS = 6 then 'Rejected By Manager' when STATUS = 7 then 'Rejected by Rider'  ELSE 'none' end as STATUS_DESCRIPTION, " +
                           "o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE, o.ADDRESS, " +
                           "o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,case when o.IS_PACKAGE = 1 then 'Yes' else 'No' end as IS_PACKAGE," +
                     "       SUM(op.QUANTITY) TOTAL_ITEMS, " +
                      "      CEILING(SUM((b.UNIT_PRICE - b.DISC)  * op.QUANTITY)) - isnull(o.MANUAL_DISCOUNT,0) - isnull(o.COUPON_DISCOUNT,0) as TOTAL_AMOUNT " +
                       "     FROM " +
                        "    ORDERS o, ORDER_PRODUCTS op, PRODUCTS p, BARCODES b " +
                         "   WHERE  " +
                          "  o.ORDER_ID = op.ORDER_ID  " +
                           " AND op.BAR_CODE = b.bar_code " +
                            " and b.ITEM_CODE = p.OLD_PRODUCT_ID AND O.BRANCH_ID = B.LOCNO " +
                           " AND o.STATUS =  " + oStID + " " +
                           " AND CONVERT(date,o.CREATED_ON) between '" + oDateFrom + "' AND '" + oDateTo + "' " + " AND o.IS_ACTIVE = 1 " +
                           " GROUP BY " +
                           "o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE,o.MANUAL_DISCOUNT, o.ADDRESS,o.Status,o.COUPON_DISCOUNT,o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,o.IS_PACKAGE  order by o.ORDER_ID desc";
            return ExecuteDataSet(query);
        }
        public DataSet getCustomerOrders(int uid)
        {
            string query = "";
            query = @"SELECT o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE, o.ADDRESS, o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,
                        case when STATUS = 1 then 'Pending' when STATUS = 2 then 'In Process' when STATUS = 3 then 'Dispatched' when STATUS = 4 then 'Delivered' when STATUS = 5 then 'Bounced' when STATUS = 6 then 'Rejected By Manager' when STATUS = 7 then 'Rejected by Rider'  ELSE 'none' end as STATUS_DESCRIPTION,
                        case when o.IS_PACKAGE = 1 then 'Yes' else 'No' end as IS_PACKAGE,       SUM(op.QUANTITY) TOTAL_ITEMS,
                        CEILING(SUM((b.UNIT_PRICE - b.DISC)  * op.QUANTITY)) - isnull(o.MANUAL_DISCOUNT,0) - isnull(o.COUPON_DISCOUNT,0) as TOTAL_AMOUNT      
                        FROM ORDER_PRODUCTS OP 
                        INNER JOIN PRODUCTS P on op.PRODUCT_ID = P.OLD_PRODUCT_ID
                        inner join BARCODES b on OP.PRODUCT_ID = B.ITEM_CODE
                        INNER JOIN ORDERS O ON OP.ORDER_ID = O.ORDER_ID
                        where B.bDefault =1 and  B.IsActive =1 and b.LOCNO = o.BRANCH_ID and  OP.IS_ACTIVE =1 and o.CUSTOMER_ID = " + uid + 
                        " GROUP BY o.ORDER_ID, o.CUSTOMER_ID, o.NAME, o.MOBILE,o.MANUAL_DISCOUNT, o.ADDRESS,o.Status,o.DELIVERY_DESCRIPTION,o.DELIVERY_TIME,o.MANUAL_DISCOUNT,o.COUPON_DISCOUNT,o.IS_PACKAGE";
            return ExecuteDataSet(query);
        }


        public DataSet allocateOrder(int oID)
        {
            string query = "UPDATE ORDERS SET STATUS = 2, ASSIGNED_ON = '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ASSIGNED_BY = " + 1 +
                            "WHERE " +
                           " ORDER_ID = " + oID + ";";
            return ExecuteDataSet(query);
        }

        public DataSet dispatchOrder(int oID, int rider)
        {
            string query = "UPDATE ORDERS SET STATUS = 3, SENT_TO_RIDER_ON = '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', SENT_TO_RIDER_BY = " + 1 + ", RIDER_ID = " + rider + 
                            " WHERE " +
                           " ORDER_ID =  " + oID + ";";
            return ExecuteDataSet(query);
        }

        public DataSet updateStock(int pID, int orderedAmount, int bID)
        {
            string query = "UPDATE STOCK SET QTY = (QTY-" + orderedAmount + ") WHERE PRODUCT_ID = " + pID + "and branch_id = " + bID + ";";

            return ExecuteDataSet(query);
        }

        public DataSet getAvailableRiders()
        {
            string query = "SELECT USER_ID, USERNAME, VEHICLE_NUMBER, VEHICLE_DESCRIPTION, IS_ACTIVE  FROM USERS  WHERE USER_TYPE = 5 AND IS_AVAILABLE = 1  AND IS_ACTIVE = 1;";
            return ExecuteDataSet(query);
        }
        
    }
}