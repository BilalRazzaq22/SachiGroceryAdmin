using GROCERY.DAL.Core;
using GROCERY.DAL.Managers;
using GROCERY.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class RiderController : Controller
    {
        UsersRepo ru = new UsersRepo();
        RiderManager riderManager = new RiderManager();
        private USER user = null;
        public ActionResult List()
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Riders";
                ViewBag.Data = LoginUser.Instance.RoleName;
                return View();
            }
            catch (System.Exception e)
            {
                return View("Error");
            }
        }

        //
        // GET: /User/Create
        public ActionResult Create()
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
                userP.USER_TYPE = 5;
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
                userP.USER_TYPE = 5;
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

        public ActionResult Delete(int id)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "View User";
                var obj = ru.DeleteUser(id);
                if (obj == false)
                    return View("Error");
                return View("List");
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

        public ActionResult GetRiderJobs(int id)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "View User";
                var obj = ru.GetRiderOrderJobs(id);
                if (obj == null)
                    return View("Error");
                return View(obj);
            }
            catch (System.Exception e)
            {
                return View("Error");
            }
        }
    }
}