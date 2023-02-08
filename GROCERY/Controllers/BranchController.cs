using GROCERY.DAL.Core;
using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GROCERY.Controllers
{
    public class BranchController : Controller
    {
        GROCERYEntities GROCERYEntities = new GROCERYEntities();
        BranchRepo branchRepo = new BranchRepo();
        readonly string sachiChakkiDb = "data source=sachichakkiwh.mine.nu;initial catalog=PWPOS_SC;user id=sa;password=golden@3864;MultipleActiveResultSets=True;App=EntityFramework&quot;";
        List<BRANCH> list = new List<BRANCH>();
        public BranchController()
        {
            GetBranchList();
        }

        // GET: Branch
        public ActionResult List()
        {
            return View(GROCERYEntities.BRANCHES.ToList());
        }

        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BRANCH branch)
        {
            try
            {
                BRANCH obj = null;
                BRANCH getBranch = branchRepo.getBranchById(branch.BRANCH_ID);
                obj = GROCERYEntities.BRANCHES.Where(x => x.BRANCH_ID == branch.BRANCH_ID).FirstOrDefault();
                if (obj != null)
                {
                    ViewBag.Message = "Branch Is Already Exist";
                    return View();
                }
                else
                {
                    obj = list.Where(x => x.BRANCH_ID == branch.BRANCH_ID).FirstOrDefault();
                    if (obj != null)
                        branch.BRANCH_NAME = obj.BRANCH_NAME;
                }

                BRANCH br = new BRANCH();
                br.BRANCH_ID = branch.BRANCH_ID;
                br.BRANCH_NAME = branch.BRANCH_NAME;
                br.LONGITUDE = branch.LONGITUDE;
                br.LATITUDE = branch.LATITUDE;
                br.ADDRESS = branch.ADDRESS;
                br.PHONE = branch.PHONE;
                br.IS_ACTIVE = branch.IS_ACTIVE;

                branchRepo.AddBranch(br);
                return Redirect("/Branch/List");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                BRANCH branch = branchRepo.getBranchById(id);
                //string BRANCH_NAME = list.Where(x => x.BRANCH_ID == id).FirstOrDefault().BRANCH_NAME;
                //branch.BRANCH_NAME = BRANCH_NAME;
                ViewBag.BranchList = new SelectList(GROCERYEntities.BRANCHES, "BRANCH_ID", "BRANCH_NAME", id);
                if (branch == null)
                {
                    return HttpNotFound();
                }
                return View(branch);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(BRANCH branch)
        {
            try
            {
                // not used right now because dropdown is disabled
                //BRANCH getBranch = branchRepo.getBranchById(branch.BRANCH_ID);
                //if (getBranch != null)
                //{
                //    ViewBag.Message = "Branch Is Already Exist";
                //    return View();
                //}
                BRANCH obj = GROCERYEntities.BRANCHES.Where(x => x.BRANCH_ID == branch.BRANCH_ID).FirstOrDefault();
                if (obj != null)
                {
                    string BRANCH_NAME = obj.BRANCH_NAME;
                    branch.BRANCH_NAME = BRANCH_NAME;
                }
                branchRepo.UpdateBranch(branch);
                return Redirect("/Branch/List");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                branchRepo.DeleteBranch(id);
                return Redirect("/Branch/Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        private void GetBranchList()
        {
            SqlConnection _con = new SqlConnection(sachiChakkiDb);
            _con.Open();
            SqlCommand cmd = new SqlCommand("Select * From LOC_CON", _con);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    list.Add(new BRANCH
                    {
                        BRANCH_ID = Convert.ToInt32(dr["CO1"]),
                        BRANCH_NAME = dr["CO4"].ToString()
                    });
                }

                ViewBag.BranchList = new SelectList(list, "BRANCH_ID", "BRANCH_NAME");
            }
        }
    }
}