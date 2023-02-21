using GROCERY.DAL.Core;
using System.Data;
namespace GROCERY.DAL.Managers
{
    public class UserManager : DABase
    {
        public DataSet getUserTypes()
        {
            var q = "SELECT * FROM USER_TYPES;";
            return ExecuteDataSet(q);
        }

        public DataSet getUsers()
        {
            //var q = "SELECT u.[USER_ID], u.[USERNAME], ut.[USER_TYPE_ID], ut.[DESCRIPTION] USER_TYPE_DESCRIPTION, u.[IS_ACTIVE]"
            //        + "FROM [dbo].[USERS] u LEFT OUTER JOIN [dbo].[USER_TYPES] ut " 
            //        + "ON u.[USER_TYPE] = ut.[USER_TYPE_ID]" 
            //        + "WHERE u.[IS_ACTIVE] = 1";
            var q = "SELECT u.[USER_ID], u.[USERNAME], ut.[USER_TYPE_ID], ut.[DESCRIPTION] USER_TYPE_DESCRIPTION, u.[IS_ACTIVE] "
                    + "FROM[dbo].[USERS] u LEFT OUTER JOIN[dbo].[USER_TYPES] ut "
                    + "ON u.[USER_TYPE] = ut.[USER_TYPE_ID] "
                    + "WHERE u.[IS_ACTIVE] = 1 AND (ut.[DESCRIPTION] != 'Rider' OR ut.[DESCRIPTION] IS NULL) AND(ut.USER_TYPE_ID != 5 OR ut.USER_TYPE_ID IS NULL)";
            return ExecuteDataSet(q);
        }

        public DataSet getUserscContacts()
        {
            var q = "SELECT * FROM USERS WHERE IS_ACTIVE =1";
            return ExecuteDataSet(q);
        }

        public int deleteUser(int uID)
        {
            string queryString = "UPDATE USERS SET IS_ACTIVE = 0, UPDATED_ON = '" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', UPDATED_BY = " + 1 + "  where USER_ID = " + uID;
            return ExecuteNonQuery(queryString);
        }

        public DataSet checkExistence(string mobNum)
        {
            string queryString = "Select * from USERS where MOBILE_NO = '" + mobNum + "';";
            return ExecuteDataSet(queryString);
        }

    }
}