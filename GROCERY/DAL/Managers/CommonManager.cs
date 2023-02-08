using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GROCERY.DAL.Core;
using System.Data;

namespace GROCERY.DAL.Managers
{
    public class CommonManager:DABase
    {
        const string QRY_GET_PACKAGES = "select P.NAME,P.DESCRIPTION,(CASE WHEN P.IS_ACTIVE = 0 THEN 'Inactive' else 'Active' end) as STATUS,P.PACKAGE_ID FROM PACKAGES P";
        const string QRY_GET_CUSTOMERS = @"select u.USER_ID ,u.USERNAME,u.MOBILE_NO,u.ADDRESS,
                                        (case when u.IS_ACTIVE =0 then 'Inactive' else 'Active' end) as STATUS ,
                                        (select count(*) from ORDERS where CUSTOMER_ID = u.USER_ID) as NO_OF_ORDERS
                                        from USERS U
                                        order by NO_OF_ORDERS desc"; 

        public DataSet getPackages()
        {
            return ExecuteDataSet(QRY_GET_PACKAGES);
        }
        public DataSet getCustomers()
        {
            return ExecuteDataSet(QRY_GET_CUSTOMERS);
        }
    }
}