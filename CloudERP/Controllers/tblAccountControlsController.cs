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
    public class tblAccountControlsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        private List<AccountControlMV> accountControl = new List<AccountControlMV>();

        // GET: tblAccountControls
        public ActionResult Index()
        {
            accountControl.Clear();
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }

            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var tblAccountControls = db.tblAccountControls.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser).Where(a => a.CompanyID == companyId && a.BranchID == branchId);
            foreach (var item in tblAccountControls)
            {
                accountControl.Add(new AccountControlMV
                {
                    AccountControlID = item.AccountControlID,
                    AccountControlName = item.AccountControlName,
                    AccountHeadID = item.AccountHeadID,
                    AccountHeadName = db.tblAccountHeads.Find(item.AccountHeadID).AccountHeadName,
                    BranchID = item.BranchID,
                    BranchName = item.tblBranch.BranchName,
                    CompanyID = item.CompanyID,
                    Name = item.tblCompany.Name,
                    UserID = item.UserID,
                    UserName = item.tblUser.UserName,
                });
            }


            return View(accountControl.ToList());
        }

        // GET: tblAccountControls/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }
            accountControl.Clear();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            if (tblAccountControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountControl);
        }

        // GET: tblAccountControls/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }

            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName");

            return View();
        }

        // POST: tblAccountControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAccountControl tblAccountControl)
        {

            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblAccountControl.BranchID = branchId;
            tblAccountControl.CompanyID = companyId;
            tblAccountControl.UserID = userid;

            if (ModelState.IsValid)
            {
                var findcontrol = db.tblAccountControls.Where(a => a.CompanyID == companyId && a.BranchID == branchId && a.AccountControlName == tblAccountControl.AccountControlName).FirstOrDefault();
                if (findcontrol == null)
                {
                    db.tblAccountControls.Add(tblAccountControl);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }

            }

            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountControl.AccountHeadID);
            return View(tblAccountControl);
        }

        // GET: tblAccountControls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            if (tblAccountControl == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountControl.AccountHeadID);
            return View(tblAccountControl);
        }

        // POST: tblAccountControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAccountControl tblAccountControl)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }


            int userid = 0;

            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            tblAccountControl.UserID = userid;

            if (ModelState.IsValid)
            {
                var findcontrol = db.tblAccountControls.Where(a => a.CompanyID == tblAccountControl.CompanyID && a.BranchID == tblAccountControl.BranchID && a.AccountControlName == tblAccountControl.AccountControlName && a.AccountControlID != tblAccountControl.AccountControlID).FirstOrDefault();
                if (findcontrol == null)
                {
                    db.Entry(tblAccountControl).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }

            }

            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountControl.AccountHeadID);
            return View(tblAccountControl);

        }

        // GET: tblAccountControls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            if (tblAccountControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountControl);
        }

        // POST: tblAccountControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Convert.ToInt32(Session["UserTypeID"]) == 2)
            {
                return RedirectToAction("EP600", "EP");
            }
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            db.tblAccountControls.Remove(tblAccountControl);
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
