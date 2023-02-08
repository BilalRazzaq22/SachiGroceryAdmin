using GROCERY.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Managers
{
    public class StoreManager : DABase
    {
        public DataSet getBranches()
        {
            string q = "SELECT BRANCH_ID, BRANCH_NAME FROM BRANCHES where IS_ACTIVE =1";
            return ExecuteDataSet(q);
        }
        public DataSet getPaymentModes()
        {
            return ExecuteDataSet("select * from Payment_modes");
        }
        public DataSet getPrevAddress(int userId)
        {
            return ExecuteDataSet(string.Format("select * from user_Addresses where user_id = {0}",userId));
        }
        public DataSet getCouponInfo(string code)
        {
            return ExecuteDataSet(string.Format("select * from coupons where PROMO = '{0}' and IS_ACTIVE =1 AND IS_USED = 0 and EXPIRY_DATE > '{1}'", code,DateTime.Now.ToString()));
        }
    }
}