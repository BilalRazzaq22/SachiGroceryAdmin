using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class BannerController : Controller
    {
        GROCERYEntities GROCERYEntities = new GROCERYEntities();
        private USER user;
        private BannerRepo bannerRepo = new BannerRepo();

        // GET: Branch
        public ActionResult List()
        {
            return View(GROCERYEntities.BANNERS.Where(x => x.IsActive == true).ToList());
        }

        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BANNER banner, HttpPostedFileBase thumbnail)
        {
            try
            {
                string dirPath = "", path = "", fileName = "";
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                if (thumbnail.ContentLength > 0)
                {

                    if (thumbnail != null && thumbnail.ContentLength > 0)
                    {
                        fileName = user.USER_ID + "_" + Path.GetFileName(thumbnail.FileName);
                        if (banner.BannerType == "Web")
                        {
                            path = Path.Combine(Server.MapPath("../Banner_Images/Web_Banner/"), fileName);
                            //WebImage img = new WebImage(imageSize.InputStream);
                            //int width = img.Width;
                            //int height = img.Height;
                            //if (width > 1440 && height > 520)
                            //{
                            //    ViewBag.ImageError = "Image size is too big for Web";
                            //    return View();
                            //}
                            dirPath = @"/Banner_Images/Web_Banner/";
                        }
                        else
                        {
                            path = Path.Combine(Server.MapPath("../Banner_Images/Mobile_Banner/"), fileName);
                            //WebImage img = new WebImage(imageSize.InputStream);
                            //int width = img.Width;
                            //int height = img.Height;
                            //if (width > 1920 && height > 1275)
                            //{
                            //    ViewBag.ImageError = "Image size is too big for Mobile";
                            //    return View();
                            //}
                            dirPath = @"/Banner_Images/Mobile_Banner/";
                        }
                        string extension = Path.GetExtension(thumbnail.FileName);
                        thumbnail.SaveAs(path);
                    }
                }

                //BANNER ban = new BANNER();
                //ban.BannerTitle = banner.BannerTitle;
                //ban.BannerType = banner.BannerType;
                //ban.Description = banner.Description;
                //ban.ImageUrl = "http://admin-stg.chaarsu.pk" + dirPath + fileName;
                banner.ImageUrl = "https://admin.sachigrocery.pk" + dirPath + fileName;
                banner.AdminImagePath = ".." + dirPath + fileName;
                //ban.IsActive = banner.IsActive;
                banner.CreatedDate = DateTime.Now;
                //ban.RecordStatus = banner.RecordStatus;

                bannerRepo.AddBanner(banner);
                return Redirect("/Banner/List");

            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                BANNER ban = bannerRepo.getBannerById(id);
                if (ban != null)
                    return View(ban);
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BANNER banner, HttpPostedFileBase thumbnail)
        {
            try
            {
                string dirPath = "", path = "", fileName = "";
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                if (thumbnail.ContentLength > 0)
                {

                    if (thumbnail != null && thumbnail.ContentLength > 0)
                    {
                        fileName = user.USER_ID + "_" + Path.GetFileName(thumbnail.FileName);
                        if (banner.BannerType == "Web")
                        {
                            path = Path.Combine(Server.MapPath("/Banner_Images/Web_Banner/"), fileName);
                            //WebImage img = new WebImage(thumbnail.InputStream);
                            //int width = img.Width;
                            //int height = img.Height;
                            //if (width > 1440 && height > 520)
                            //{
                            //    ViewBag.ImageError = "Image size is too big for Web";
                            //    return View();
                            //}
                            dirPath = @"/Banner_Images/Web_Banner/";
                        }
                        else
                        {
                            path = Path.Combine(Server.MapPath("/Banner_Images/Mobile_Banner/"), fileName);
                            //WebImage img = new WebImage(thumbnail.InputStream);
                            //int width = img.Width;
                            //int height = img.Height;
                            //if (width > 1920 && height > 1275)
                            //{
                            //    ViewBag.ImageError = "Image size is too big for Mobile";
                            //    return View();
                            //}
                            dirPath = @"/Banner_Images/Mobile_Banner/";
                        }
                        string extension = Path.GetExtension(thumbnail.FileName);
                        thumbnail.SaveAs(path);
                    }
                }

                //BANNER ban = new BANNER();
                //ban.BannerTitle = banner.BannerTitle;
                //ban.BannerType = banner.BannerType;
                //ban.Description = banner.Description;
                //ban.IsActive = banner.IsActive;
                //ban.CreatedDate = banner.CreatedDate;
                //ban.RecordStatus = banner.RecordStatus;

                banner.ImageUrl = "https://admin.sachigrocery.pk" + dirPath + fileName;
                banner.AdminImagePath = ".." + dirPath + fileName;

                bannerRepo.UpdateBanner(banner);
                return Redirect("/Banner/List");

            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                bannerRepo.DeleteBanner(id);
                return Redirect("/Banner/List");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}