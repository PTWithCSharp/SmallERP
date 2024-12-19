using CloudERP.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class tblSuppliersController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        public ActionResult AllSupplier()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var tblSuppliers = db.tblSuppliers.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser);
            return View(tblSuppliers.ToList());
        }

        // GET: tblSuppliers
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var tblSuppliers = db.tblSuppliers.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser).Where(c => c.BranchID == branchId && c.CompanyID == companyId);
            return View(tblSuppliers.ToList());
        }
        public ActionResult SubBranchSupplier()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<BranchSuppliersMV> list = new List<BranchSuppliersMV>();
            int branchid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));

            List<int> branchids = CloudERP.HelperClass.Branch.GetBranchids(branchid, db);


            foreach (var item in branchids)
            {
                foreach (var supplier in db.tblSuppliers.Where(c => c.BranchID == item))
                {
                    var sus = new BranchSuppliersMV();
                    sus.Id = supplier.SupplierID;
                    sus.BranchName = supplier.tblBranch.BranchName;
                    sus.CompanyName = supplier.tblCompany.Name;
                    sus.SupplierAddress = supplier.SupplierAddress;
                    sus.SupplierConatctNo = supplier.SupplierConatctNo;
                    sus.SupplierEmail = supplier.SupplierEmail;
                    sus.Discription = supplier.Discription;
                    sus.SupplierName = supplier.SupplierName;
                    sus.User = supplier.tblUser.UserName;
                    list.Add(sus);


                }
            }


            return View(list);
        }

        // GET: tblSuppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // GET: tblSuppliers/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: tblSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSupplier tblSupplier)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblSupplier.BranchID = branchId;
            tblSupplier.CompanyID = companyId;
            tblSupplier.UserID = userid;
            if (ModelState.IsValid)
            {
                var find = db.tblSuppliers.Where(s => s.SupplierName == tblSupplier.SupplierName && s.SupplierConatctNo == tblSupplier.SupplierConatctNo && s.BranchID == tblSupplier.BranchID).FirstOrDefault();
                if (find == null)
                {
                    db.tblSuppliers.Add(tblSupplier);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }


            }
            return View(tblSupplier);
        }

        // GET: tblSuppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblSupplier.BranchID);
            ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblSupplier.CompanyID);
            ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblSupplier.UserID);
            return View(tblSupplier);
        }

        // POST: tblSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSupplier tblSupplier)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = 0;

            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            tblSupplier.UserID = userid;
            if (ModelState.IsValid)
            {
                var find = db.tblSuppliers.Where(s => s.SupplierName == tblSupplier.SupplierName && s.SupplierConatctNo == tblSupplier.SupplierConatctNo && s.BranchID == tblSupplier.BranchID && s.SupplierID != tblSupplier.SupplierID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(tblSupplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }


            }

            return View(tblSupplier);
        }

        // GET: tblSuppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // POST: tblSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            db.tblSuppliers.Remove(tblSupplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
