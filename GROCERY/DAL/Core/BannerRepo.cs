using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class BannerRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public void AddBanner(BANNER banner)
        {
            gEnt.BANNERS.Add(banner);
            gEnt.SaveChanges();
        }

        public BANNER getBannerById(int bannerId)
        {
            try
            {
                var q = from c in gEnt.BANNERS
                        where c.BannerId == bannerId
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

        public void UpdateBanner(BANNER banner)
        {
            try
            {
                BANNER ban = getBannerById(banner.BannerId);
                if (ban != null)
                {
                    ban.BannerTitle = banner.BannerTitle;
                    ban.BannerType = banner.BannerType;
                    ban.Description = banner.Description;
                    ban.ImageUrl = banner.ImageUrl;
                    ban.AdminImagePath = banner.AdminImagePath;
                    ban.IsActive = banner.IsActive;
                    ban.ModifiedDate = DateTime.Now;
                    ban.RecordStatus = banner.RecordStatus;
                    gEnt.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void DeleteBanner(int id)
        {
            BANNER ban = getBannerById(id);
            if (ban != null)
            {
                ban.IsActive = false;
                gEnt.SaveChanges();
            }
        }
    }
}