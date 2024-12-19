using CloudERP.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudERP.HelperClass
{
    public class Branch
    {

        public static List<int> GetBranchids(int? brid, CloudErpV1Entities db)
        {
            List<int> branchids = new List<int>();
            List<int> IsSubBranchs1 = new List<int>();
            List<int> IsSubBranchs2 = new List<int>();

            List<BranchCustomerMV> list = new List<BranchCustomerMV>();


            int branchId = 0;




            branchId = Convert.ToInt32(brid);
            var brnch = db.tblBranches.Where(b => b.BrchID == b.BranchID);

            foreach (var item in brnch)
            {
                IsSubBranchs1.Add(item.BranchID);
            }
        subbranch:
            foreach (var item in IsSubBranchs1)
            {
                branchids.Add(item);
                foreach (var sub in db.tblBranches.Where(b => b.BrchID == item))
                {
                    IsSubBranchs2.Add(sub.BranchID);
                }

            }
            if (IsSubBranchs2.Count > 0)
            {
                foreach (var item in IsSubBranchs2)
                {
                    IsSubBranchs1.Add(item);
                }
                IsSubBranchs2.Clear();
                goto subbranch;
            }
            return branchids;

        }
    }
}