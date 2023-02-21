using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class UserTypeController : Controller
    {
        UserTypeRepo userTypeRepo = new UserTypeRepo();
        // GET: UserType
        public ActionResult Index()
        {
            return View(userTypeRepo.UserTypeList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(USER_TYPES utype)
        {
            userTypeRepo.AddUserType(utype);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            USER_TYPES utype = userTypeRepo.getUserTypeById(id);
            if (utype != null)
            {
                return View(utype);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult Edit(USER_TYPES utype)
        {
            userTypeRepo.UpdateUserType(utype);
            return RedirectToAction("Index");
        }   

        public ActionResult Delete(int id)
        {
            userTypeRepo.DeleteUserType(id);
            return RedirectToAction("Index");
        }
    }
}