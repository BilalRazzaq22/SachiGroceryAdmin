using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GROCERY.Models;
namespace GROCERY.DAL.Core
{
    public class AdRepo:DABase
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public int saveAd(AD ad)
        {
            gEnt.ADS.Add(ad);
            gEnt.SaveChanges();
            return ad.AD_ID;
        }
        public void updateStatus(int id,int mode)
        {
            if(mode ==0)
                ExecuteNonQuery(string.Format("update ADS set IS_ACTIVE = 'Inactive' where AD_ID = {0}", id));
            else
                ExecuteNonQuery(string.Format("update ADS set IS_ACTIVE = 'Active' where AD_ID = {0}", id));
        }
        public List<AD> getAllAds()
        {
            try
            {
                var q = from c in gEnt.ADS
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
    }
}