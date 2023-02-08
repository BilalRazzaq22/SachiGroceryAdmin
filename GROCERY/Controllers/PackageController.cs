using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GROCERY.DAL.Core;
using GROCERY.Models;
namespace GROCERY.Controllers
{
    public class PackageController : Controller
    {
        private USER user = null;
        private PackageRepo repo = new PackageRepo();
        //
        // GET: /Package/
        public ActionResult Index()
        {
            try
            {
                return View(repo.getAllPackages());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int pId)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return Redirect("/Home/Login");
                }
                ViewBag.Title = "Edit Package";
                if (pId == 0)
                {
                    ViewBag.Status = "";
                    return View(new PACKAGE() { PACKAGE_ID = -1 });
                }
                PACKAGE p = repo.getPackage(pId);
                if (p == null)
                {
                    ViewBag.Status = "";
                    return View();
                }
   
                ViewBag.pDetails = repo.getPackageDetails(p.PACKAGE_ID, 0);
                
                return View(p);
            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;

                return View("Error");
            }
        }
        public bool save(PackageModel pkg)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return false;
                }
                int packageId = 0;
                packageId = repo.addPackage(new PACKAGE
                    {
                        CREATED_BY = user.USER_ID,
                        NAME = pkg.Name,
                        DESCRIPTION = pkg.Desc,
                    });
                if (packageId > 0)
                    return (repo.generatePackageProduct(pkg.cOrders, packageId));
                else
                    return false;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }

        public bool UpdatePackage(PackageModel pkg)
        {
            try
            {
                user = (USER)Session["UserLoggedIn"];
                if (user == null)
                {
                    return false;
                }
                
                return repo.updatePackage(new PACKAGE
                    {
                        PACKAGE_ID = pkg.PackageId,
                        NAME = pkg.Name,
                        DESCRIPTION = pkg.Desc,
                        UPDATED_BY = user.USER_ID
                    }, pkg.cOrders);
                

            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.InnerException + e.Message;
                return false;
            }
        }

	}
}