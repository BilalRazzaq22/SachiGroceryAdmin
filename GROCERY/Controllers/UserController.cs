using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROCERY.DAL.Core;

namespace GROCERY.Controllers
{
    public class UserController : Controller
    {
        UsersRepo ru = new UsersRepo();
        OrdersRepo repo = new OrdersRepo();
        CouponsRepo cRepo = new CouponsRepo();
        private USER user = null;
        //
        // GET: /Users/
        public ActionResult List()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Users";
                return View();
            }
            catch (System.Exception e)
            {
                return View("Error");
            }
        }

        //
        // GET: /User/View/5
        public ActionResult View(int id)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "View User";
                var obj = ru.getUser(id);
                if (obj == null)
                    return View("Error");
                return View(obj);
            }
            catch (System.Exception e)
            {
                return View("Error");
            }
        }

        //
        // GET: /User/Create
        public ActionResult Add()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        public ActionResult Create(USER userP)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
               int uID = ru.addUser(userP);
                if (uID < 0)
                    return View("Error");
                TempData["showSuccessMsg"] = "addedFlag";
                return Redirect("/User/View/" + uID);
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        //
        // GET: /User/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Update User";
                USER obj = ru.getUser(id);
                if (obj == null)
                    return View("Error");
                return View(obj);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        //
        // POST: /User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, USER userP)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
               int uID = ru.updateUser(userP);
                if (uID < 0)
                    return View("Error");
                TempData["showSuccessMsg"] = "updatedFlag";
                return Redirect("/User/View/" + uID);
            }
            catch (System.Exception e)
            {
                return View("Error");
            }
        }


        public ActionResult LoginCheck(string uName, string uPassword)
        {
            try
            {
                USER u = ru.getUser(uName, uPassword);
                if (u == null)
                {
                    Session["LoginError"] = "Invalid Credentials! Please check provided information";
                    return Redirect("/Home/Login");
                }
                Session["UserLoggedIn"] = u;
                LoginUser.Instance.GetRoleName(u);
                Session["Username"] = u.USERNAME;
                Session["Rolename"] = LoginUser.Instance.RoleName;
                return Redirect("/Order/List");
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }
        public ActionResult Customers()
        {
            return View();
        }
        public ActionResult Orders(int uId)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "View Orders";
                //if (uId == 0)
                //{
                //    ViewBag.Status = "";
                //    return View(new ORDER() { ORDER_ID = -1 });
                //}
                //ORDER p = repo.getPackage(pId);
                //if (p == null)
                //{
                //    ViewBag.Status = "";
                //    return View();
                //}

                //ViewBag.pDetails = repo.getPackageDetails(p.PACKAGE_ID, 0);
                ViewBag.UserId = uId;
                return View();
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;

                return View("Error");
            }
        }
        public ActionResult ViewOrder(int oId)
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
                if(res != null && res.item != null)
                {
                    o = res.item;
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
                    ViewBag.Time = o.DELIVERY_TIME;
                }
                switch (o.BRANCH_ID)
                {
                    case 1: ViewBag.Branch = "Karim Market";
                        ViewBag.BranchId = 1;
                        break;
                    case 2: ViewBag.Branch = "Asif Block";
                        ViewBag.BranchId = 2;
                        break;
                    case 3: ViewBag.Branch = "Wapda Town";
                        ViewBag.BranchId = 3;
                        break;
                    case 5: ViewBag.Branch = "Johar Town";
                        ViewBag.BranchId = 5;
                        break;
                    case 6:
                        ViewBag.Branch = "Behria Town";
                        ViewBag.BranchId = 6;
                        break;
                    default: ViewBag.Branch = "";
                        ViewBag.BranchId = -1;
                        break;
                }
                switch (o.STATUS)
                {

                    case 1: ViewBag.Status = "Pending";
                        break;
                    case 2: ViewBag.Status = "Assigned to floorman";
                        break;
                    case 3: ViewBag.Status = "Dispatched";
                        break;
                    case 4: ViewBag.Status = "Delivered";
                        break;
                    case 5: ViewBag.Status = "Bounced";
                        break;
                    case 6: ViewBag.Status = "Reject By Manager";
                        break;
                    case 7: ViewBag.Status = "Reject By Rider";
                        break;

                    default: ViewBag.Status = "";
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


    }
}
