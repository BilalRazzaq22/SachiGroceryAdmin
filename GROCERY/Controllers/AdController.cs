using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROCERY.Models;
using System.IO;
using GROCERY.DAL.Core;
using System.Net.Mail;
namespace GROCERY.Controllers
{
    public class AdController : Controller
    {
        AdRepo rp = new AdRepo();
        private USER user;
        //
        // GET: /Ad/
        public ActionResult Index()
        {
            
            return View(rp.getAllAds());
        }
        public ActionResult Save(AD ad, HttpPostedFileBase file)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                

                    if (file != null && file.ContentLength > 0)
                    {
                        ad.IMAGE_PATH = Path.Combine(Server.MapPath("~/assets/images/Ads/"), Path.GetFileName(file.FileName));
                        file.SaveAs(ad.IMAGE_PATH);
                        ad.IMAGE_PATH = "http://admin.chaarsu.pk/assets/Ads/" + file.FileName;
                    }

                ad.CREATED_ON = DateTime.Now;
                ad.CREATED_BY = user.USER_ID;
                ad.IS_ACTIVE = "Active";

                rp.saveAd(ad);
                return RedirectToAction("/Ad/Index");

            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.InnerException + ex.Message;

                return View("Error");
            }
        }
        public ActionResult Create()
        {
            //sendEmail();
            return View();
        }
        public ActionResult Edit(int id)
        {
            try
            {
                rp.updateStatus(id,0);
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        public ActionResult Edit2(int id)
        {
            try
            {
                rp.updateStatus(id,1);
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        protected void sendEmail()
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.Subject = "test";
                mailMessage.Body ="testing";
                mailMessage.To.Add("muhammad.ahsanasif10@gmail.com");
                mailMessage.To.Add("aibakmirza@gmail.com");
                EmailHelper.sendEmail(ref mailMessage);
                
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}