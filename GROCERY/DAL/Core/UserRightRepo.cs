using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class UserRightRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public void Save(List<MainMenuModel> mainMenuModels, int userRoleId)
        {
            foreach (var item in mainMenuModels)
            {
                MAINMENU main = new MAINMENU();
                MAINMENU mainMenu = GetUserMainMenuByID(item.ID, userRoleId);
                if (mainMenu != null)
                {
                    //mainMenu.USERID = userId;
                    mainMenu.USERTYPEID = userRoleId;
                    mainMenu.IsActive = item.IsActive;
                }
                else
                {
                    main.MAINMENU1 = item.MainMenu;
                    //main.USERID = userId;
                    main.USERTYPEID = userRoleId;
                    main.IsActive = item.IsActive;
                    gEnt.MAINMENUs.Add(main);
                }
                gEnt.SaveChanges();
                foreach (var items in item.SubMenus.Where(x => x.MainMenuID == item.ID && x.UserTypeID == userRoleId))
                {
                    SUBMENU sub = GetSubMenuByID(items.ID, userRoleId);
                    if (sub != null)
                    {
                        sub.ID = items.ID;
                        sub.SUBMENU1 = items.SubMenu;
                        sub.MAINMENUID = items.MainMenuID;
                        sub.USERTYPEID = userRoleId;
                        //sub.USERID = userId;
                        sub.IsActive = Convert.ToBoolean(items.IsActive);
                    }
                    else
                    {
                        SUBMENU subMenu = new SUBMENU();
                        subMenu.SUBMENU1 = items.SubMenu;
                        subMenu.MAINMENUID = main.ID;
                        subMenu.USERTYPEID = userRoleId;
                        //subMenu.USERID = userId;
                        subMenu.IsActive = Convert.ToBoolean(items.IsActive);
                        gEnt.SUBMENUs.Add(subMenu);
                    }
                    //mainMenu9.SUBMENUs.Add(sub);
                }
                gEnt.SaveChanges();
            }
        }

        public MAINMENU GetUserMainMenuByID(int mainMenuId, int userTypeId)
        {
            try
            {
                var q = from c in gEnt.MAINMENUs
                        where c.ID == mainMenuId
                        //&& c.USERID == userId
                        && c.USERTYPEID == userTypeId
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

        public SUBMENU GetSubMenuByID(int subMenuId, int userTypeId)
        {
            try
            {
                var q = from c in gEnt.SUBMENUs
                        where c.ID == subMenuId
                        //&& c.USERID == userId
                        && c.USERTYPEID == userTypeId
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
    }
}