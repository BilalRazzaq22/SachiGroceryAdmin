using GROCERY.DAL.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Managers
{
    public class RiderManager : DABase
    {
        public DataSet getRiders()
        {
            //var q = "SELECT u.[USER_ID], u.[USERNAME], ut.[USER_TYPE_ID], ut.[DESCRIPTION] USER_TYPE_DESCRIPTION, u.[IS_ACTIVE]"
            //        + "FROM [dbo].[USERS] u LEFT OUTER JOIN [dbo].[USER_TYPES] ut " 
            //        + "ON u.[USER_TYPE] = ut.[USER_TYPE_ID]" 
            //        + "WHERE u.[IS_ACTIVE] = 1";
            var q = @"SELECT u.[USER_ID], u.[USERNAME], ut.[USER_TYPE_ID], ut.[DESCRIPTION] USER_TYPE_DESCRIPTION, u.[IS_ACTIVE]
                    FROM[dbo].[USERS] u LEFT OUTER JOIN[dbo].[USER_TYPES] ut
                    ON u.[USER_TYPE] = ut.[USER_TYPE_ID]
                    WHERE ut.[DESCRIPTION] = 'Rider' AND u.[IS_ACTIVE] = 1";
            return ExecuteDataSet(q);
        }
        
        public DataSet getRiderOrderJobs(int riderId)
        {
            //var q = "SELECT u.[USER_ID], u.[USERNAME], ut.[USER_TYPE_ID], ut.[DESCRIPTION] USER_TYPE_DESCRIPTION, u.[IS_ACTIVE]"
            //        + "FROM [dbo].[USERS] u LEFT OUTER JOIN [dbo].[USER_TYPES] ut " 
            //        + "ON u.[USER_TYPE] = ut.[USER_TYPE_ID]" 
            //        + "WHERE u.[IS_ACTIVE] = 1";
            var q = @"SELECT U.USER_ID,U.USERNAME,UT.DESCRIPTION AS USER_TYPE_DESCRIPTION,U.IS_ACTIVE AS USER_STATUS,RO.RIDER_TIME_IN,RO.RIDER_TIME_OUT,RO.IS_RIDER_BACK FROM USERS U
                    INNER JOIN USER_TYPES UT ON U.USER_TYPE = UT.USER_TYPE_ID
                    INNER JOIN RIDER_ORDER RO ON U.USER_ID = RO.RIDER_ID
                    WHERE U.USER_TYPE = 5 AND U.IS_ACTIVE = 1 AND RO.RIDER_ID = "+ riderId + "";
            return ExecuteDataSet(q);
        }
    }
}