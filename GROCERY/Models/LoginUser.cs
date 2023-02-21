using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.Models
{
    public class LoginUser
    {
        GROCERYEntities ent = new GROCERYEntities();
        public string RoleName = "";
        private static LoginUser _instance;
        private static object objlock = new object();
        public static LoginUser Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (objlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LoginUser();
                        }
                    }
                }
                return _instance;
            }
        }

        public void GetRoleName(USER user)
        {
            USER_TYPES userType = ent.USER_TYPES.FirstOrDefault(x => x.USER_TYPE_ID == user.USER_TYPE);

            if (userType != null)
            {
                RoleName = userType.DESCRIPTION;
            }
        }
    }
}