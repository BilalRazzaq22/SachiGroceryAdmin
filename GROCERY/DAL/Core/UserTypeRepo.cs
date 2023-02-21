using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class UserTypeRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public List<USER_TYPES> UserTypeList()
        {
            return gEnt.USER_TYPES.ToList();
        }

        public void AddUserType(USER_TYPES userType)
        {
            gEnt.USER_TYPES.Add(userType);
            gEnt.SaveChanges();
        }

        public USER_TYPES getUserTypeById(int userTypeId)
        {
            try
            {
                var q = from c in gEnt.USER_TYPES
                        where c.USER_TYPE_ID == userTypeId
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

        public void UpdateUserType(USER_TYPES userType)
        {
            try
            {
                USER_TYPES utype = getUserTypeById(userType.USER_TYPE_ID);
                if (utype != null)
                {
                    utype.DESCRIPTION = userType.DESCRIPTION;
                    gEnt.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void DeleteUserType(int id)
        {
            USER_TYPES utype = getUserTypeById(id);
            if (utype != null)
            {
                gEnt.USER_TYPES.Remove(utype);
                gEnt.SaveChanges();
            }
        }
    }
}