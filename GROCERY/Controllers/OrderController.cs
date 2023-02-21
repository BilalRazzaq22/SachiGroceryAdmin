using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GROCERY.Models;
using GROCERY.DAL.Core;
using GROCERY.SyncService;
using System.Threading;
using GROCERY.Notifications;
using Newtonsoft.Json;
using System.Net.Http;
using ClosedXML.Excel;
using System.IO;
using System.Web.Http.Results;
using System.Runtime.InteropServices.ComTypes;

namespace GROCERY.Controllers
{
    public class OrderController : Controller
    {
        private OrdersRepo repo = new OrdersRepo();
        private UsersRepo uRepo = new UsersRepo();
        private CouponsRepo cRepo = new CouponsRepo();
        GROCERYEntities db = new GROCERYEntities();
        BusinessController controller = new BusinessController();
        private USER user = null;
        // GET: /Order/
        public ActionResult List()
        {
            try
            {
                ViewBag.TabName = "";
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Riders = repo.getRiders();
                ViewBag.AllOrders = repo.getAllOrders();
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Index()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return View("Error");
            }
        }

        // GET: /Order/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.BRANCH_ID = -1;
                ViewBag.RoleName = LoginUser.Instance.RoleName;
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.BRANCH_ID = user.BRANCH_ID == null ? -1 : user.BRANCH_ID;
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return View("Error");
            }
        }

        // POST: /Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public bool ProcessOrder(CustomerOrderBO custOrd)
        {
            try
            {
                if (custOrd.isTestOrder)
                {
                    return false;
                }

                int uID = -1;

                var userData = uRepo.getUserByMobileNumber(custOrd.MobileNumber);

                if (userData == null)
                {
                    uID = uRepo.addUser(new USER
                    {
                        USERNAME = custOrd.CustomerName,
                        PASSWORD = custOrd.MobileNumber,
                        MOBILE_NO = custOrd.MobileNumber,
                        ADDRESS = custOrd.Address,
                        BRANCH_ID = custOrd.BranchID,
                        USER_TYPE = 4
                    });

                    if (uID < 1)
                        return false;


                }
                else
                {
                    uID = userData.USER_ID;
                    if (userData.ADDRESS != null && (userData.ADDRESS.ToUpper() != custOrd.Address.ToUpper() || userData.BRANCH_ID != custOrd.BranchID))
                    {
                        //latest address
                        if (uRepo.updateUser(new USER
                        {
                            USER_ID = userData.USER_ID,
                            USERNAME = custOrd.CustomerName,
                            PASSWORD = custOrd.MobileNumber,
                            MOBILE_NO = custOrd.MobileNumber,
                            ADDRESS = custOrd.Address,
                            BRANCH_ID = custOrd.BranchID,
                            USER_TYPE = 4
                        }) < 1)
                            return false;
                    }

                }
                //save in user address if not exist
                uRepo.saveAddress(custOrd.Address, uID);

                var coupon = cRepo.getCouponByCode(custOrd.coupon);

                if (coupon == null)
                    coupon = new COUPON();
                else
                    if (custOrd.totalAmount < coupon.UNLOCK_AMOUNT)
                    coupon = new COUPON();
                else
                        if (!cRepo.updateCouponStatus(coupon.COUPON_ID))
                    return false;


                int orderID = 0;
                if (custOrd.DeliveryTime != null)
                {
                    orderID = repo.addOrder(new ORDER
                    {
                        CUSTOMER_ID = uID,
                        NAME = custOrd.CustomerName,
                        MOBILE = custOrd.MobileNumber,
                        ADDRESS = custOrd.Address,
                        STATUS = 1,
                        IS_ACTIVE = 1,
                        BRANCH_ID = custOrd.BranchID,
                        PAYMENT_MODE_ID = custOrd.PaymentMode,
                        DELIVERY_DESCRIPTION = custOrd.DeliveryDescription,
                        MANUAL_DISCOUNT = custOrd.extraDisc,
                        COUPON_ID = coupon.COUPON_ID,
                        COUPON_DISCOUNT = coupon.VALUE,
                        IS_PACKAGE = custOrd.Package,
                        DELIVERY_TIME = DateTime.Parse(custOrd.DeliveryTime),
                        IsTestOrder = custOrd.isTestOrder
                    });
                }
                else
                {
                    DateTime currentdate;
                    DateTime date1 = DateTime.UtcNow;
                    TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
                    currentdate = TimeZoneInfo.ConvertTime(date1, tz);
                    orderID = repo.addOrder(new ORDER
                    {
                        CUSTOMER_ID = uID,
                        NAME = custOrd.CustomerName,
                        MOBILE = custOrd.MobileNumber,
                        ADDRESS = custOrd.Address,
                        STATUS = 1,
                        IS_ACTIVE = 1,
                        BRANCH_ID = custOrd.BranchID,
                        PAYMENT_MODE_ID = custOrd.PaymentMode,
                        DELIVERY_DESCRIPTION = custOrd.DeliveryDescription,
                        MANUAL_DISCOUNT = custOrd.extraDisc,
                        COUPON_ID = coupon.COUPON_ID,
                        COUPON_DISCOUNT = coupon.VALUE,
                        IS_PACKAGE = custOrd.Package,
                        DELIVERY_TIME = currentdate,
                        IsTestOrder = custOrd.isTestOrder
                    });
                }
                sendOrderSms("Your order (" + orderID + " ) has been placed ", custOrd.MobileNumber);
                DataSet managers_fcm_token = controller.managerFCMToken(custOrd.BranchID.ToString());
                FCMPushNotification notification = new FCMPushNotification();
                foreach (DataRow r in managers_fcm_token.Tables[0].Rows)
                {
                    notification.SendNotification("Chaarsu", "New Order!!!" + orderID, "news", "477625648329", r["FCM_TOKEN"].ToString());
                }
                if ((repo.generateProductOrders(custOrd.cOrders, orderID)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public bool SaveTransactionNumber(string paymentModeId, string transactionNumber)
        {
            try
            {
                repo.SaveTransactionNumber(paymentModeId, transactionNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void sendOrderSms(string msg, string num)
        {
            string username = "chaarsu@bizsms.pk";
            string pass = "ch33rsuw9";
            string destinationnum = num;
            string masking = "CHAARSU";
            string text = msg;
            string language = "English";
            int msgCount = 0;
            //start sending SMS on request
            if (msg != string.Empty)
            {
                GenerateSMSAlert(masking, destinationnum, msg, username, pass);
            }

            //end sending SMS on request
        }
        #region Generate SMS

        public int GenerateSMSAlert(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            int count = 0;
            if (toNumber.Length == 12 && toNumber.Substring(0, 2).Equals("92"))
            {
                count++;
                SendSMS(Masking, toNumber, MessageText, MyUsername, MyPassword);
            }
            return count;
        }
        #endregion

        #region Send SMS

        public static string SendSMS(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            // OLD IMPLEMENTATION
            //string URI = @"http://api.bizsms.pk/api-send-branded-sms.aspx?username=" + MyUsername + "&pass=" + MyPassword +
            //    "&text=" + MessageText + "&masking=" + Masking + "&destinationnum=" + toNumber + "&language=English";

            //WebRequest req = WebRequest.Create(URI);
            //WebResponse resp = req.GetResponse();
            //var sr = new System.IO.StreamReader(resp.GetResponseStream());
            //return sr.ReadToEnd().Trim();

            // NEW IMPLEMENTATION
            //string URI = @"http://api.bizsms.pk/api-send-branded-sms.aspx?username=" + MyUsername + "&pass=" + MyPassword +
            //    "&text=" + MessageText + "&masking=" + Masking + "&destinationnum=" + toNumber + "&language=English";

            //string URI = "@https://cbs.zong.com.pk/reachcwsv2?loginId= chaarsu@bizsms.pk&loginPassword=ch33rsuw9&Message=Your order (76470 ) has been placed &Mask=CHAARSU&Destination=03314784501&UniCode=0" +
            //    "&ShortCodePrefered=n";
            string URI = @"http://cbs.zong.com.pk/reachcwsv2/corporatesms.svc?wsdl&loginId=" + MyUsername + "&loginPassword=" + MyPassword +
                "&Message=" + MessageText + "&Mask=" + Masking + "&Destination=" + toNumber + "&UniCode=0&ShortCodePrefered=n";

            WebRequest req = WebRequest.Create(URI);
            req.Method = "GET";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12  | SecurityProtocolType.Ssl3;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


            WebResponse resp = req.GetResponse();
            var sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }
        #endregion
        public bool UpdateOrder(CustomerOrderBO custOrd)
        {
            try
            {
                var userData = uRepo.getUser(custOrd.CustomerID);
                //userData = null;
                if (userData == null)
                    return false;
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return false;
                }
                if (uRepo.updateUser(new USER
                {
                    USER_ID = custOrd.CustomerID,
                    USERNAME = custOrd.CustomerName,
                    MOBILE_NO = custOrd.MobileNumber,
                    ADDRESS = custOrd.Address,
                    CITY = userData.CITY,
                    USER_TYPE = userData.USER_TYPE,
                    IS_ACTIVE = userData.IS_ACTIVE,
                    BRANCH_ID = userData.BRANCH_ID,
                    IS_AVAILABLE = userData.IS_AVAILABLE,
                    UPDATED_ON = DateTime.Now,
                    UPDATED_BY = user.USER_ID
                }) < 1)
                    return false;
                var coupon = cRepo.getCouponByCode(custOrd.coupon);

                if (coupon == null)
                    coupon = new COUPON();
                else
                    if (custOrd.totalAmount < coupon.UNLOCK_AMOUNT)
                    coupon = new COUPON();
                else
                        if (!cRepo.updateCouponStatus(coupon.COUPON_ID))
                    return false;

                if (custOrd.DeliveryTime != null)
                {
                    return repo.updateOrder(new ORDER
                    {
                        ORDER_ID = custOrd.OrderID,
                        CUSTOMER_ID = custOrd.CustomerID,
                        NAME = custOrd.CustomerName,
                        ADDRESS = custOrd.Address,
                        MOBILE = custOrd.MobileNumber,
                        BRANCH_ID = custOrd.BranchID,
                        PAYMENT_MODE_ID = custOrd.PaymentMode,
                        DELIVERY_DESCRIPTION = custOrd.DeliveryDescription,
                        MANUAL_DISCOUNT = custOrd.extraDisc,
                        UPDATED_BY = user.USER_ID,
                        DELIVERY_TIME = DateTime.Parse(custOrd.DeliveryTime),
                        IS_PACKAGE = custOrd.Package,
                        COUPON_ID = coupon.COUPON_ID,
                        IsTestOrder = custOrd.isTestOrder,
                        COUPON_DISCOUNT = coupon.VALUE,
                    }, custOrd.cOrders, custOrd.coupon, custOrd.totalAmount);
                }
                else
                {
                    return repo.updateOrder(new ORDER
                    {
                        ORDER_ID = custOrd.OrderID,
                        CUSTOMER_ID = custOrd.CustomerID,
                        NAME = custOrd.CustomerName,
                        ADDRESS = custOrd.Address,
                        MOBILE = custOrd.MobileNumber,
                        BRANCH_ID = custOrd.BranchID,
                        PAYMENT_MODE_ID = custOrd.PaymentMode,
                        DELIVERY_DESCRIPTION = custOrd.DeliveryDescription,
                        MANUAL_DISCOUNT = custOrd.extraDisc,
                        UPDATED_BY = user.USER_ID,
                        IS_PACKAGE = custOrd.Package,
                        COUPON_ID = coupon.COUPON_ID,
                        COUPON_DISCOUNT = coupon.VALUE,
                        IsTestOrder = custOrd.isTestOrder,
                        //DELIVERY_TIME = custOrd.DeliveryTime,
                    }, custOrd.cOrders, custOrd.coupon, custOrd.totalAmount);
                }

            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return false;
            }
        }


        // GET: /Order/Edit
        public ActionResult Edit(ORDER o)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                if (o.ORDER_ID == 0)
                {
                    ViewBag.Status = "";
                    return View(new ORDER() { ORDER_ID = -1 });
                }
                var res = repo.getOrder(o.ORDER_ID);
                o = null;
                if (res != null && res.item != null)
                {
                    o = res.item;
                }
                if (o == null)
                {
                    ViewBag.Status = "";
                    return View();
                }

                if (o.IS_ACTIVE == 0)
                {
                    ViewBag.Status = "Order you are searching for is either rejected or fullfilled.";
                    return View(new ORDER() { ORDER_ID = -1 });
                }
                ViewBag.oDetails = repo.getOrderDetails(o.ORDER_ID, (int)o.BRANCH_ID);
                return View(o);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return View("Error");
            }
        }

        public ActionResult Update(ORDER o)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }

                ViewBag.oDetails = repo.rejectOrder(o);
                return Redirect("/Home/Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        public ActionResult View(int oId)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "View Order";
                if (oId == 0)
                {
                    ViewBag.Status = "";
                    return View(new ORDER() { ORDER_ID = -1 });
                }
                var res = repo.getOrder(oId);
                ORDER o = null;
                if (res != null && res.item != null)
                {
                    o = res.item;
                    ViewBag.Email = res.Email ?? "";
                }
                if (o == null)
                {
                    ViewBag.Status = "";
                    return View();
                }
                ViewBag.PaymentMode = o.PAYMENT_MODE_ID == null ? 1 : o.PAYMENT_MODE_ID;
                ViewBag.Package = o.IS_PACKAGE == null ? 0 : o.IS_PACKAGE;
                if (o.DELIVERY_TIME != null)
                {
                    ViewBag.Time = Convert.ToDateTime(o.DELIVERY_TIME).ToString("yyyy-MM-ddTH:mm");
                }
                switch (o.BRANCH_ID)
                {
                    case 1:
                        ViewBag.Branch = "Karim Market";
                        ViewBag.BranchId = 1;
                        break;
                    case 2:
                        ViewBag.Branch = "Asif Block";
                        ViewBag.BranchId = 2;
                        break;
                    case 3:
                        ViewBag.Branch = "Wapda Town";
                        ViewBag.BranchId = 3;
                        break;
                    case 5:
                        ViewBag.Branch = "Johar Town";
                        ViewBag.BranchId = 5;
                        break;
                    case 6:
                        ViewBag.Branch = "Behria Town";
                        ViewBag.BranchId = 6;
                        break;
                    case 7:
                        ViewBag.Branch = "Eme Society";
                        ViewBag.BranchId = 7;
                        break;
                    default:
                        ViewBag.Branch = "";
                        ViewBag.BranchId = -1;
                        break;
                }
                switch (o.STATUS)
                {

                    case 1:
                        ViewBag.Status = "Pending";
                        break;
                    case 2:
                        ViewBag.Status = "Assigned to floorman";
                        break;
                    case 3:
                        ViewBag.Status = "Dispatched";
                        break;
                    case 4:
                        ViewBag.Status = "Delivered";
                        break;
                    case 5:
                        ViewBag.Status = "Bounced";
                        break;
                    case 6:
                        ViewBag.Status = "Reject By Manager";
                        break;
                    case 7:
                        ViewBag.Status = "Reject By Rider";
                        break;

                    default:
                        ViewBag.Status = "";
                        break;
                }
                if (o.IS_ACTIVE == 0)
                {
                    ViewBag.Status = "Order you are searching for is either rejected or fullfilled.";
                    return View(new ORDER() { ORDER_ID = -1 });
                }
                ViewBag.Discount = o.MANUAL_DISCOUNT;
                ViewBag.oDetails = repo.getOrderDetails(o.ORDER_ID, (int)o.BRANCH_ID);
                ViewBag.CPNDiscount = o.COUPON_DISCOUNT == null ? 0 : o.COUPON_DISCOUNT;

                var coupon = o.COUPON_ID == null ? null : cRepo.getCouponById((int)o.COUPON_ID);
                ViewBag.Coupon = coupon == null ? new COUPON
                {
                    COUPON_ID = -1,
                    CODE = -1,
                    PROMO = "nil"
                } : coupon;
                return View(o);
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;

                return View("Error");
            }
        }

        public JsonResult GetRights()
        {
            user = (USER)Session["UserLoggedIn"];
            //using (GROCERYEntities db = new GROCERYEntities())
            //{
            //List<MAINMENU> b = db.MAINMENUs.ToList();
            List<MainMenuModel> mainmenu = new List<MainMenuModel>();
            //var a = db.SUBMENUs.ToList();
            foreach (var item in db.MAINMENUs.Where(x => x.USERTYPEID == user.USER_TYPE))
            {
                MainMenuModel main = new MainMenuModel();
                main.ID = item.ID;
                main.MainMenu = item.MAINMENU1;
                main.UserID = Convert.ToInt32(item.USERID);
                main.UserTypeID = Convert.ToInt32(item.USERTYPEID);
                main.IsActive = Convert.ToBoolean(item.IsActive);
                main.SubMenus = new List<SubMenuViewModel>();
                foreach (var items in db.SUBMENUs.Where(x => x.MAINMENUID == item.ID))
                {
                    SubMenuViewModel sub = new SubMenuViewModel();
                    sub.ID = items.ID;
                    sub.SubMenu = items.SUBMENU1;
                    sub.MainMenuID = items.MAINMENUID;
                    sub.UserID = Convert.ToInt32(items.USERID);
                    sub.UserTypeID = user.USER_TYPE;
                    sub.IsActive = Convert.ToBoolean(items.IsActive);
                    main.SubMenus.Add(sub);
                }
                mainmenu.Add(main);
            }
            return Json(mainmenu, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpGet]
        public bool ExportOrdersToExcel(string tabName, string oDateFrom, string oDateTo)
        {
            DataSet ds = new DataSet();
            string fileName = "";

            switch (tabName.Trim())
            {
                case "All":
                    ds = controller.getOrdersForExcel(0, oDateFrom, oDateTo);
                    fileName = "All Orders";
                    break;
                case "Pending Orders":
                    ds = controller.getOrdersForExcel(1, oDateFrom, oDateTo);
                    fileName = "Pending Orders";
                    break;
                case "Allocate Orders/Assign to Floor Man":
                    ds = controller.getOrdersForExcel(2, oDateFrom, oDateTo);
                    fileName = "Allocate Orders";
                    break;
                case "Dispatched Orders":
                    ds = controller.getOrdersForExcel(3, oDateFrom, oDateTo);
                    fileName = "Dispatched Orders";
                    break;
                case "Shipped Orders":
                    ds = controller.getOrdersForExcel(4, oDateFrom, oDateTo);
                    fileName = "Shipped Orders";
                    break;
                default:
                    break;
            }

            if (ds.Tables.Count > 0)
            {
                ds.Tables[0].TableName = "OrdersTable";

                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        DataTable dt = new DataTable();
                        wb.Worksheets.Add(ds.Tables[0], fileName);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", $"attachment;filename={fileName}.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
