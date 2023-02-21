using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using GROCERY.Models;
using System;
using GROCERY.DAL.Managers;
using System.Collections.Generic;

namespace GROCERY.DAL.Core
{
    public class UsersRepo
    {

        GROCERYEntities gEnt = new GROCERYEntities();
        RiderManager riderManager = new RiderManager();
        public int addUser(USER u)
        {
            try
            {
                //u.IS_ACTIVE = 1;
                u.CREATED_BY = 1;
                u.CREATED_ON = DateTime.Now;
                u.UPDATED_ON = null;
                if (u.USER_TYPE == 5)
                    u.IS_AVAILABLE = 1;

                USER obj = gEnt.USERS.Add(u);
                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;
                return obj.USER_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int updateUser(USER u)
        {
            try
            {
                USER user = gEnt.USERS.Find(u.USER_ID);
                if (user == null)
                    return -1;
                user.USERNAME = u.USERNAME;
                user.USER_TYPE = u.USER_TYPE;
                user.MOBILE_NO = u.MOBILE_NO;
                user.ADDRESS = u.ADDRESS;
                user.IS_ACTIVE = u.IS_ACTIVE;
                user.BRANCH_ID = u.BRANCH_ID;
                //user.ADDRESS = u.ADDRESS;

                if (user.USER_TYPE == 5)
                {
                    user.VEHICLE_DESCRIPTION = u.VEHICLE_DESCRIPTION;
                    user.VEHICLE_NUMBER = u.VEHICLE_NUMBER;
                    user.IS_AVAILABLE = 1;
                }
                user.UPDATED_ON = DateTime.Now;
                user.UPDATED_BY = 1;


                int result = gEnt.SaveChanges();
                if (result < 1)
                    return -1;
                return user.USER_ID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public bool deleteUser(int userID)
        //{
        //    try
        //    {
        //        USER obj = gEnt.USERS.Find(userID);
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

        public USER getUser(int userID)
        {
            try
            {
                USER obj = gEnt.USERS.Find(userID);
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteUser(int userId)
        {
            try
            {
                USER obj = getUser(userId);
                if (obj != null)
                {
                    gEnt.USERS.Remove(obj); 
                    gEnt.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public USER getUserByMobileNumber(string mobNo)
        {
            try
            {
                var query = from u in gEnt.USERS
                            where u.MOBILE_NO.ToUpper() == mobNo.ToUpper()
                            select u;
                if (query.Any())
                    return query.First();
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void saveAddress(string address, int userId)
        {
            try
            {
                var query = from u in gEnt.USER_ADDRESSES
                            where u.ADDRESS.ToUpper() == address.ToUpper() && u.USER_ID == userId
                            select u;
                if (query.Any())
                    return;
                else
                {
                    USER_ADDRESSES u_add = new USER_ADDRESSES();
                    u_add.USER_ID = userId;
                    u_add.ADDRESS = address;
                    u_add.IS_ACTIVE = true;
                    u_add.CREATED_ON = DateTime.Now;
                    u_add.CREATED_BY = 1;

                    gEnt.USER_ADDRESSES.Add(u_add);
                    gEnt.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public USER getUser(string uName, string uPassword)
        {
            try
            {
                var query = (from u in gEnt.USERS
                             where u.USERNAME.ToUpper() == uName.ToUpper()
                             && u.PASSWORD.ToUpper() == uPassword.ToUpper()
                             && u.IS_ACTIVE == true
                             select u);
                if (query.Any())
                    return query.FirstOrDefault();
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<RiderOrderModel> GetRiderOrderJobs(int riderId)
        {
            try
            {
                List<RiderOrderModel> riderOrdersList = new List<RiderOrderModel>();
                DataSet ds = riderManager.getRiderOrderJobs(riderId);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        RiderOrderModel riderOrderModel = new RiderOrderModel()
                        {
                            USER_ID = Convert.ToInt32(dr["USER_ID"]?.ToString()),
                            IS_RIDER_BACK = Convert.ToBoolean(dr["IS_RIDER_BACK"]?.ToString()),
                            RIDER_TIME_IN = dr["RIDER_TIME_IN"]?.ToString(),
                            RIDER_TIME_OUT = dr["RIDER_TIME_OUT"]?.ToString(),
                            USERNAME = dr["USERNAME"]?.ToString(),
                            USER_STATUS = Convert.ToBoolean(dr["USER_STATUS"]?.ToString()),
                            USER_TYPE_DESCRIPTION = dr["USER_TYPE_DESCRIPTION"]?.ToString()
                        };
                        riderOrdersList.Add(riderOrderModel);   
                    }
                }

                return riderOrdersList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}