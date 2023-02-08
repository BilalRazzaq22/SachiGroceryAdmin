using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class MiniItemController : Controller
    {
        //
        // GET: /MiniItem/
        ProductsRepo rp = new ProductsRepo();
        BusinessController controller = new BusinessController();
        private USER user;
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Add()
        {
            user = (USER)Session["UserLoggedIn"];
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.Title = "Add Product";
            return View();
        }

        public ActionResult AddProduct(PRODUCT p)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                p.CREATED_BY = user.USER_ID;
                p.CREATED_DATE = DateTime.Now;
                p.HAS_IMAGE = false;
                p.HAS_THUMBNAIL_IMAGE = false;

                //Corresponding entries for barcode table
                BARCODE b = new BARCODE();
                b.BAR_CODE = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                b.bDEFAULT = true;
                b.CDATE = DateTime.Now;
                b.CUSER = user.USER_ID;
                if (p.DISCOUNT == null)
                    p.DISCOUNT = 0;


                b.DISC = (decimal)p.DISCOUNT;
                b.DISC_TYPE = 0;
                b.IsActive = true;
                b.MUSER = user.USER_ID;
                b.PACK_CODE = 1;
                b.PACK_DESC = "SINGLE";
                b.UNIT = 1;
                if (p.PRICE == null)
                    p.PRICE = 0;
                b.UNIT_PRICE = (decimal)p.PRICE;
                
                int pID = rp.addMiniItem(p,b);
                if (pID < 0)
                    return View("Error");

                

                    TempData["showSuccessMsg"] = "addedFlag";
                return Redirect("/MiniItem/Update?pID=" + pID);
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return View("Error");
            }
        }

        public ActionResult Update(int pID)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Update Product";
                var obj = rp.getProduct(pID);

                if (obj == null)
                    return View("Error");

                var imgObj = rp.getProductImages(pID);
                if (imgObj == null)
                {
                    ViewBag.Thumbnail = "../Product_Images/default.jpg";
                    ViewBag.Image = "../Product_Images/default.jpg";
                }
                else
                {
                    ViewBag.Thumbnail = imgObj.ADMIN_THUMNAIL_IMAGE_PATH;
                    ViewBag.Image = imgObj.ADMIN_IMAGE_PATH;
                }

                ViewBag.Quantity = rp.getProductQuantity(pID);
                return View(obj);
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;

                return View("Error");
            }
        }
        public ActionResult UpdateProduct(PRODUCT p)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                p.HAS_IMAGE = controller.hasImage(p.PRODUCT_ID);
                p.MODIFIED_BY = user.USER_ID;
                int pID = rp.updateProduct(p);
                
                if (pID < 0)
                    return View("Error");

                int result = controller.updateMiniProductBarcode((decimal)p.PRICE,(double)p.DISCOUNT,user.USER_ID,(int)p.OLD_PRODUCT_ID);
                TempData["showSuccessMsg"] = "updatedFlag";
                return Redirect("/Item/Update?pID=" + pID);
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return View("Error");
                // return Redirect("/Item/View?pID=" + 1);
            }
        }
        [HttpPost]
        public ActionResult UploadImageThumbnail(HttpPostedFileBase thumbnail, int PRODUCT_ID)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                if (thumbnail.ContentLength > 0)
                {

                    if (thumbnail != null && thumbnail.ContentLength > 0)
                    {
                        string dirPath = @"/Product_Images/Thumbnails/";
                        var fileName = PRODUCT_ID + "_" + Path.GetFileName(thumbnail.FileName);
                        string extension = Path.GetExtension(thumbnail.FileName);
                        var path = Path.Combine(Server.MapPath("../Product_Images/Thumbnails/"), fileName);
                        thumbnail.SaveAs(path);

                        saveImageThumbnailData(dirPath + fileName, PRODUCT_ID);
                    }
                }
                ViewBag.Title = "Upload Product";
                //return Redirect("/Item/List");
                return Redirect("/Item/Update?pID=" + PRODUCT_ID);
            }
            catch (System.Exception ex)
            {

                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, int PRODUCT_ID)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                if (file.ContentLength > 0)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        string dirPath = @"/Product_Images/";
                        var fileName = PRODUCT_ID + "_" + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("../Product_Images/"), fileName);
                        file.SaveAs(path);

                        saveImageData(dirPath + fileName, PRODUCT_ID);
                    }
                }
                ViewBag.Title = "Upload Product";
                //return Redirect("/Item/List");
                return Redirect("/Item/Update?pID=" + PRODUCT_ID);
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message +ex.InnerException;
                return View("Error");
            }
        }

        private void saveImageData(string FilePath, int productID)
        {
            //bool has_image = false;
            int has_image = 0, result = 0;
            string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["GROCERYDB"].ConnectionString;
            //execute a query to erase any previous data from our destination table

            string hasImagesql = string.Format("select PRODUCT_ID from PRODUCT_IMAGES where PRODUCT_ID  = {0}", productID);
            SqlConnection sqlconn0 = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd0 = new SqlCommand(hasImagesql, sqlconn0);
            sqlconn0.Open();
            object o = sqlcmd0.ExecuteScalar();
            has_image = o == null ? 0 : (int)o;
            sqlconn0.Close();

            if (has_image == 0)
            {
                //insert thumbnail column
                string sclearsql2 = string.Format("Insert into PRODUCT_IMAGES values ( {0},'{1}',null,'{2}',null)", productID, ".." + FilePath, "http://admin.chaarsu.pk" + FilePath);
                SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
                sqlconn2.Open();
                result = sqlcmd2.ExecuteNonQuery();
                sqlconn2.Close();
            }
            else
            {
            
                string sclearsql2 = string.Format("update PRODUCT_IMAGES set ADMIN_IMAGE_PATH = '{1}',CHAARSU_IMAGE_PATH = '{2}'  where PRODUCT_ID = {0}", productID, ".." + FilePath, "http://admin.chaarsu.pk" + FilePath);
                SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
                sqlconn2.Open();
                result = sqlcmd2.ExecuteNonQuery();
                sqlconn2.Close();


            }


            if (result > 0)
            {
                string sclearsql3 = string.Format("update PRODUCTS set HAS_IMAGE = 1 where PRODUCT_ID = {0}", productID);
                SqlConnection sqlconn3 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd3 = new SqlCommand(sclearsql3, sqlconn3);
                sqlconn3.Open();
                sqlcmd3.ExecuteNonQuery();
                sqlconn3.Close();

            }


        }

        private void saveImageThumbnailData(string FilePath, int productID)
        {
            //bool has_image = false;
            int has_image = 0, result = 0;
            string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["GROCERYDB"].ConnectionString;
            //execute a query to erase any previous data from our destination table

            string hasImagesql = string.Format("select PRODUCT_ID from PRODUCT_IMAGES where PRODUCT_ID  = {0}", productID);
            SqlConnection sqlconn0 = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd0 = new SqlCommand(hasImagesql, sqlconn0);
            sqlconn0.Open();
            object o = sqlcmd0.ExecuteScalar();
            has_image = o == null ? 0 : (int)o;
            sqlconn0.Close();
            if (has_image == 0)
            {
                //insert thumbnail column
                string sclearsql2 = string.Format("Insert into PRODUCT_IMAGES values ( {0},null,'{1}',null,'{2}')", productID, ".." + FilePath, "http://admin.chaarsu.pk" + FilePath);
                SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
                sqlconn2.Open();
                result = sqlcmd2.ExecuteNonQuery();
                sqlconn2.Close();
            }
            else
            {
                string sclearsql2 = string.Format("update PRODUCT_IMAGES set ADMIN_THUMNAIL_IMAGE_PATH = '{1}', CHAARSU_THUMBNAIL_PATH = '{2}' where PRODUCT_ID = {0}", productID, ".." + FilePath, "http://admin.chaarsu.pk" + FilePath);
                SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
                sqlconn2.Open();
                result = sqlcmd2.ExecuteNonQuery();
                sqlconn2.Close();


            }


            if (result > 0)
            {
                string sclearsql3 = string.Format("update PRODUCTS set HAS_THUMBNAIL_IMAGE = 1 where PRODUCT_ID = {0}", productID);
                SqlConnection sqlconn3 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd3 = new SqlCommand(sclearsql3, sqlconn3);
                sqlconn3.Open();
                sqlcmd3.ExecuteNonQuery();
                sqlconn3.Close();

            }
        }

        
	}
}