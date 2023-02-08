using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

namespace GROCERY.Controllers
{
    public class ItemController : Controller
    {
        BusinessController controller = new BusinessController();
        ProductsRepo rp = new ProductsRepo();
        private USER user;
        public ActionResult List()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Product";
                return View();
            }
            catch (System.Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Quantity(int pID)
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
                ProductQuantity pQ = new ProductQuantity
                {
                    item = obj,
                    branches = rp.getBranches(),
                    stocks = rp.getStockDetailsByProduct(pID)
                };
                return View(pQ);
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;

                return View("Error");
            }
        }

        public ActionResult Stock()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }

                var oldPID = int.Parse(Request["item.OLD_PRODUCT_ID"]);

                List<STOCK> stkList = new List<STOCK>();

                var branches = rp.getBranches();
                foreach (var item in branches)
                {
                    stkList.Add(new STOCK 
                    {
                        BRANCH_ID = Convert.ToInt16( item.BRANCH_ID ),
                        PRODUCT_ID = oldPID,
                        QTY = int.Parse(Request["Branch_"+item.BRANCH_ID]),
                        IsActive = true
                    });
                }

                var obj = rp.updateStock(stkList, oldPID);
                if (obj == false)
                    return View("Error");

                return View("List");
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

        public ActionResult View(int pID)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "View Product";
                var obj = rp.getProduct(pID);
                if(obj==null)
                    return View("Error");

                var imgObj = rp.getProductImages(pID);
                if(imgObj == null)
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

        public ActionResult Add()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Add Product";
                return View();
            }
            catch (System.Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Upload()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Upload Product";
                return View();
            }
            catch (System.Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UploadProduct(HttpPostedFileBase file)
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
                        var fileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("../App_Data/"), fileName);
                        file.SaveAs(path);

                        Import_To_Grid(path, extension, "Yes");
                        //one without oledb driver
                        //importToTable(path);
                    }
                }
                ViewBag.Title = "Upload Product";
                //return Redirect("/Item/List");
                return Redirect("/Item/List");
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;

                return View("Error");
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

                        saveImageThumbnailData(dirPath+fileName,PRODUCT_ID );
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
                        var fileName = PRODUCT_ID+ "_" + Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("../Product_Images/"), fileName);
                        file.SaveAs(path);

                        saveImageData(dirPath+fileName,PRODUCT_ID );
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
                

                int pID =  rp.addProduct(p);
                if(pID < 0 )
                    return View("Error");
                TempData["showSuccessMsg"] = "addedFlag";  
                return Redirect("/Item/Update?pID="+pID);
            }
            catch (System.Exception e)
            {
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

        private void Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string ssqltable = "Product_Temp_Load";

            string conStr = "";

            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }

            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();

            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            connExcel.Close();


            string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["GROCERYDB"].ConnectionString;
            //execute a query to erase any previous data from our destination table
            string sclearsql = "delete from " + ssqltable;
            SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
            sqlconn.Open();
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();
            //series of commands to bulk copy data from the excel file into our sql table
            OleDbConnection oledbconn = new OleDbConnection(conStr);
            OleDbCommand oledbcmd = new OleDbCommand(cmdExcel.CommandText, oledbconn);
            oledbconn.Open();
            OleDbDataReader dr = oledbcmd.ExecuteReader();
            SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);
            bulkcopy.DestinationTableName = ssqltable;
            while (dr.Read())
            {
                bulkcopy.WriteToServer(dr);
            }
            dr.Close();
            oledbconn.Close();

            string query = @" INSERT INTO PRODUCTS 
                    ([OLD_PRODUCT_ID]
                   ,[NAME]
                   ,[DESCRIPTION]
                   ,[SUB_CATEGORY_ID]
                   ,[UNIT_OF_MEASUREMENT]
                   ,[AVG_COST]
                   ,[PRICE]
                   ,[DISCOUNT_PERCENTAGE]
                   ,[IMPORTED]
                   ,[IS_ACTIVE]
                   ,[CREATED_DATE]
                   ,[CREATED_BY]
                   ,[MODIFIED_DATE]
                   ,[MODIFIED_BY]
                   ,[COLOR]
                   ,[BRAND]
                   ,[FLAVOR]
                   ,[PACKING]
                   ,[TAG]
                   ,[TYPES]
                   ,[DISCOUNT]
                   ,[SEC_SUB_CATEGORY_A]
                   ,[SEC_SUB_CATEGORY_B]
                   ,[PRICE2]
                   ,[HAS_IMAGE]
                   ,[HAS_THUMBNAIL_IMAGE]
                   ,[VENDOR_ID]
                   ,[CATEGORY_ID]
                   ,[IS_FEATURED]) 
                    SELECT * FROM PRODUCT_TEMP_LOAD;";

            sqlconn.Open();
            sqlcmd.CommandText = query;
            sqlcmd.ExecuteNonQuery();
            sqlcmd.CommandText = sclearsql;
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();

        }
        private void importToTable(string FilePath)
        {
            string tempTable = "Product_Temp_Load";


            string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["GROCERYDB"].ConnectionString;
            //execute a query to erase any previous data from our destination table
            string sclearsql = "delete from " + tempTable;
            SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
            sqlconn.Open();
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;
            
            int rCnt = 0;
            int rw = 0;
            int cl = 0;



            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(FilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;
            
            sqlconn.Open();

            for (rCnt = 1; rCnt <= rw; rCnt++)
            {
                if (rCnt > 2)
                {
                    string ssqltable = "insert into Product_Temp_Load values ( ";

                    string NAME = "";
                    string DESCRIPTION = "";
                    int VENDOR_ID = 0;
                    int CATEGORY_ID = 0;
                    int SUB_CATEGORY_ID = 0;
                    string UNIT_OF_MEASUREMENT = "";
                    double AVG_COST = 0;
                    int IS_ACTIVE = 1;
                    string TAGS = "";
                    string TYPE = "";
                    string FLAVORS = "";

                    NAME = " '" + (range.Cells[rCnt, 1] as Excel.Range).Value2 + "' ";
                    DESCRIPTION = " '" + (range.Cells[rCnt, 2] as Excel.Range).Value2 + "' ";
                    VENDOR_ID = (int)(range.Cells[rCnt, 3] as Excel.Range).Value2;
                    CATEGORY_ID = (int)(range.Cells[rCnt, 4] as Excel.Range).Value2;
                    SUB_CATEGORY_ID = (int)(range.Cells[rCnt, 5] as Excel.Range).Value2;
                    UNIT_OF_MEASUREMENT = " '" + (range.Cells[rCnt, 6] as Excel.Range).Value2 + "' ";
                    AVG_COST = (range.Cells[rCnt, 7] as Excel.Range).Value2;
                    IS_ACTIVE = (int)(range.Cells[rCnt, 8] as Excel.Range).Value;
                    TAGS = " '" + (range.Cells[rCnt, 9] as Excel.Range).Value2 + "' ";
                    TYPE = " '" + (range.Cells[rCnt, 10] as Excel.Range).Value2 + "' ";
                    FLAVORS = " '" + (range.Cells[rCnt, 11] as Excel.Range).Value2 + "' ";

                    string insertStatment = ssqltable + NAME + "," + DESCRIPTION + "," + VENDOR_ID + ", " + CATEGORY_ID + ", " +
                        SUB_CATEGORY_ID + ", " + UNIT_OF_MEASUREMENT + "," + AVG_COST + ", " + IS_ACTIVE + ", " + TAGS + "," + TYPE + ","
                        + FLAVORS + ");";

                    //for (cCnt = 1; cCnt <= cl; cCnt++)
                    //{
                    //    str = "" + (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                    //    ssqltable += " '" + str + "' ,";
                    //    //  Console.WriteLine(str);
                    //    //MessageBox.Show(str);
                    //}
                    //  Console.WriteLine(ssqltable);

                    sqlcmd.CommandText = insertStatment;
                    sqlcmd.ExecuteNonQuery();

                }

            }
            sqlconn.Close();
            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

           
            
            string query = " INSERT INTO PRODUCTS (NAME, DESCRIPTION, VENDOR_ID, CATEGORY_ID, SUB_CATEGORY_ID, UNIT_OF_MEASUREMENT, AVG_COST, IS_ACTIVE, TAGS, TYPE, FLAVORS) SELECT * FROM PRODUCT_TEMP_LOAD;";
            sqlconn.Open();
            sqlcmd.CommandText = query;
            sqlcmd.ExecuteNonQuery();
            sqlcmd.CommandText = sclearsql;
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();

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
                //update thumbnail column
                string sclearsql2 = string.Format("Insert into PRODUCT_IMAGES values ( {0},'{1}',null,'{2}',null)", productID, ".." + FilePath, "http://admin.chaarsu.pk" + FilePath);
                SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
                sqlconn2.Open();
                result = sqlcmd2.ExecuteNonQuery();
                sqlconn2.Close();
            }
            else
            {
                //string sclearsql = string.Format("delete from PRODUCT_IMAGES where PRODUCT_ID = {0}", productID);
                //SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
                //SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
                //sqlconn.Open();
                //sqlcmd.ExecuteNonQuery();
                //sqlconn.Close();

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
            
            
            //string sclearsql = string.Format("delete from PRODUCT_IMAGES where PRODUCT_ID = {0}", productID);
            //SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
            //SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
            //sqlconn.Open();
            //sqlcmd.ExecuteNonQuery();
            //sqlconn.Close();
            
            //string sclearsql2 = string.Format("Insert into PRODUCT_IMAGES values ( {0},'{1}',null,null)",productID,FilePath);
            //SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
            //SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
            //sqlconn2.Open();
            //int result = sqlcmd2.ExecuteNonQuery();
            //sqlconn2.Close();

            //if(result > 0)
            //{
            //    string sclearsql3 = string.Format("update PRODUCTS set HAS_IMAGE = 1 where PRODUCT_ID = {0}",productID);
            //    SqlConnection sqlconn3 = new SqlConnection(ssqlconnectionstring);
            //    SqlCommand sqlcmd3 = new SqlCommand(sclearsql3, sqlconn3);
            //    sqlconn3.Open();
            //    sqlcmd3.ExecuteNonQuery();
            //    sqlconn3.Close();

            //}
        }

        private void saveImageThumbnailData(string FilePath, int productID)
        {
            //bool has_image = false;
            int has_image=0,result = 0;
            string ssqlconnectionstring = ConfigurationManager.ConnectionStrings["GROCERYDB"].ConnectionString;
            //execute a query to erase any previous data from our destination table

            string hasImagesql = string.Format("select PRODUCT_ID from PRODUCT_IMAGES where PRODUCT_ID  = {0}", productID);
            SqlConnection sqlconn0 = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd0 = new SqlCommand(hasImagesql, sqlconn0);
            sqlconn0.Open();
            object o = sqlcmd0.ExecuteScalar();
            has_image = o == null ? 0 : (int)o;
            sqlconn0.Close();
            if(has_image ==0)
            {
                //update thumbnail column
                string sclearsql2 = string.Format("Insert into PRODUCT_IMAGES values ( {0},null,'{1}',null,'{2}')", productID,".."+FilePath, "http://admin.chaarsu.pk" + FilePath);
                SqlConnection sqlconn2 = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd2 = new SqlCommand(sclearsql2, sqlconn2);
                sqlconn2.Open();
                result = sqlcmd2.ExecuteNonQuery();
                sqlconn2.Close();
            }
            else
            {
                //string sclearsql = string.Format("delete from PRODUCT_IMAGES where PRODUCT_ID = {0}", productID);
                //SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
                //SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
                //sqlconn.Open();
                //sqlcmd.ExecuteNonQuery();
                //sqlconn.Close();

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
