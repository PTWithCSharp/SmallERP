using CloudERP.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class tblAccountSettingsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblAccountSettings
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

            var tblAccountSettings = db.tblAccountSettings.Include(t => t.tblAccountActivity).Include(t => t.tblAccountControl).Include(t => t.tblAccountHead).Include(t => t.tblAccountSubControl)
                .Include(t => t.tblBranch).Include(t => t.tblCompany)
                .Where(t => t.CompanyID == companyId && t.BranchID == branchId);
            return View(tblAccountSettings.ToList());
        }

        // GET: tblAccountSettings/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
        //    if (tblAccountSetting == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblAccountSetting);
        //}

        // GET: tblAccountSettings/Create
        public ActionResult Create()
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

            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", "0");
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountControlID", "AccountControlName", "0");
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", "0");
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountSubControlID", "AccountSubControlName", "0");

            return View();
        }

        // POST: tblAccountSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAccountSetting tblAccountSetting)
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
            tblAccountSetting.CompanyID = companyId;
            tblAccountSetting.BranchID = branchId;

            if (ModelState.IsValid)
            {
                var find = db.tblAccountSettings.Where(c => c.CompanyID == tblAccountSetting.CompanyID && c.BranchID == tblAccountSetting.BranchID && c.AccountActivityID == tblAccountSetting.AccountActivityID).FirstOrDefault();
                if (find == null)
                {
                    db.tblAccountSettings.Add(tblAccountSetting);
                    db.SaveChanges();
                    ViewBag.Message = "Save Sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Please Try Again";
                }



            }

            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", tblAccountSetting.AccountActivityID);
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountControlID", "AccountControlName", tblAccountSetting.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSetting.AccountHeadID);
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountSubControlID", "AccountSubControlName", tblAccountSetting.AccountSubControlID);

            return View(tblAccountSetting);
        }

        // GET: tblAccountSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            if (tblAccountSetting == null)
            {
                return HttpNotFound();
            }
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", tblAccountSetting.AccountActivityID);
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountControlID", "AccountControlName", tblAccountSetting.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSetting.AccountHeadID);
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountSubControlID", "AccountSubControlName", tblAccountSetting.AccountSubControlID);

            return View(tblAccountSetting);
        }

        // POST: tblAccountSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAccountSetting tblAccountSetting)
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
            tblAccountSetting.CompanyID = companyId;
            tblAccountSetting.BranchID = branchId;

            if (ModelState.IsValid)
            {
                var find = db.tblAccountSettings.Where(c => c.CompanyID == tblAccountSetting.CompanyID && c.BranchID == tblAccountSetting.BranchID && c.AccountActivityID == tblAccountSetting.AccountActivityID && c.AccountSettingID != tblAccountSetting.AccountSettingID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(tblAccountSetting).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Message = "Save Sucessfully";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Please Try Again";
                }

            }

            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", tblAccountSetting.AccountActivityID);
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountControlID", "AccountControlName", tblAccountSetting.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSetting.AccountHeadID);
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "AccountSubControlID", "AccountSubControlName", tblAccountSetting.AccountSubControlID);

            return View(tblAccountSetting);
        }

        [HttpGet]
        public ActionResult GetAccountControls(int? id)
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
            List<AccountSettingMV> Controls = new List<AccountSettingMV>();

            var list = db.tblAccountControls.Where(p => p.BranchID == branchId && p.CompanyID == companyId && p.AccountHeadID == id).ToList();
            foreach (var product in list)
            {
                Controls.Add(new AccountSettingMV { AccountControlID = product.AccountControlID, AccountControlName = product.AccountControlName });
            }

            return Json(new { data = Controls }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetSubControls(int? id)
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
            List<AccountSubControlMV> Controls = new List<AccountSubControlMV>();

            var list = db.tblAccountSubControls.Where(p => p.BranchID == branchId && p.CompanyID == companyId && p.AccountControlID == id).ToList();
            foreach (var product in list)
            {
                Controls.Add(new AccountSubControlMV { AccountSubControlID = product.AccountSubControlID, AccountSubControlName = product.AccountSubControlName });
            }

            return Json(new { data = Controls }, JsonRequestBehavior.AllowGet);

        }

        // GET: tblAccountSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            if (tblAccountSetting == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSetting);
        }

        // POST: tblAccountSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            db.tblAccountSettings.Remove(tblAccountSetting);
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
