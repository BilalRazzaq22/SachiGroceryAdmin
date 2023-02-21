using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class UserRightController : Controller
    {
        private USER user = null;
        UserRightRepo userRightRepo = new UserRightRepo();
        private static int UserRoleId = 0;
        
        // GET: UserRight
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<GROCERY.Models.MainMenuModel> model)
        {
            if (model != null)
            {
                userRightRepo.Save(model, UserRoleId);
                RedirectToAction("List", "Order");
            }
            return RedirectToAction("List", "Order");
        }

        public ActionResult GetUser(int userTypeId = 0)
        {
            user = (USER)Session["UserLoggedIn"];

            if (userTypeId == 0)
            {
                UserRoleId = user.USER_TYPE;
            }
            else
            {
                UserRoleId = userTypeId;
            }
            List<MainMenuModel> mainmenu = new List<MainMenuModel>();
            using (GROCERYEntities db = new GROCERYEntities())
            {
                if (db.MAINMENUs.Where(x => x.USERTYPEID == UserRoleId).Count() > 0)
                {
                    foreach (var item in db.MAINMENUs.Where(x => x.USERTYPEID == UserRoleId))
                    {
                        MainMenuModel main = new MainMenuModel
                        {
                            ID = item.ID,
                            MainMenu = item.MAINMENU1,
                            //UserID = Convert.ToInt32(item.USERID),
                            UserTypeID = UserRoleId,
                            IsActive = Convert.ToBoolean(item.IsActive),
                            SubMenus = new List<SubMenuViewModel>()
                        };
                        foreach (var items in db.SUBMENUs.Where(x => x.MAINMENUID == item.ID))
                        {
                            SubMenuViewModel sub = new SubMenuViewModel
                            {
                                ID = items.ID,
                                SubMenu = items.SUBMENU1,
                                MainMenuID = items.MAINMENUID,
                                //UserID = Convert.ToInt32(items.USERID),
                                UserTypeID = UserRoleId,
                                IsActive = Convert.ToBoolean(items.IsActive)
                            };
                            main.SubMenus.Add(sub);
                        }
                        mainmenu.Add(main);
                    }
                }
                else
                {
                    foreach (var item in db.MAINMENUs.Where(x => x.USERTYPEID == 0))
                    {
                        MainMenuModel main = new MainMenuModel
                        {
                            ID = item.ID,
                            MainMenu = item.MAINMENU1,
                            //UserID = Convert.ToInt32(item.USERID),
                            UserTypeID = UserRoleId,
                            IsActive = Convert.ToBoolean(item.IsActive),
                            SubMenus = new List<SubMenuViewModel>()
                        };
                        foreach (var items in db.SUBMENUs.Where(x => x.MAINMENUID == item.ID))
                        {
                            SubMenuViewModel sub = new SubMenuViewModel
                            {
                                ID = items.ID,
                                SubMenu = items.SUBMENU1,
                                MainMenuID = items.MAINMENUID,
                                //UserID = Convert.ToInt32(items.USERID),
                                UserTypeID = UserRoleId,
                                IsActive = Convert.ToBoolean(items.IsActive)
                            };
                            main.SubMenus.Add(sub);
                        }
                        mainmenu.Add(main);
                    }
                }
                //var a = db.SUBMENUs.ToList();
            }
            return PartialView("_UserRight", mainmenu);
        }
    }
}