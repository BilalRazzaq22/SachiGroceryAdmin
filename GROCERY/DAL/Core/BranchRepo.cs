using GROCERY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GROCERY.DAL.Core
{
    public class BranchRepo
    {
        GROCERYEntities gEnt = new GROCERYEntities();

        public void AddBranch(BRANCH branch)
        {
            gEnt.BRANCHES.Add(branch);
            gEnt.SaveChanges();
        }

        public BRANCH getBranchById(int branchId)
        {
            try
            {
                var q = from c in gEnt.BRANCHES
                        where c.BRANCH_ID == branchId
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

        public void UpdateBranch(BRANCH branch)
        {
            try
            {
                BRANCH br = getBranchById(branch.BRANCH_ID);
                if (br != null)
                {
                    br.BRANCH_NAME = branch.BRANCH_NAME;
                    br.LONGITUDE = branch.LONGITUDE;
                    br.LATITUDE = branch.LATITUDE;
                    br.ADDRESS = branch.ADDRESS;
                    br.PHONE = branch.PHONE;
                    br.IS_ACTIVE = branch.IS_ACTIVE;
                    gEnt.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void DeleteBranch(int id)
        {
            BRANCH br = getBranchById(id);
            if (br != null)
            {
                gEnt.BRANCHES.Remove(br);
                gEnt.SaveChanges();
            }
        }
    }
}