using DatabaseAccess;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class tblAccountSubControlsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblAccountSubControls
        public ActionResult Index()
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

            var tblAccountSubControls = db.tblAccountSubControls.Include(t => t.tblAccountControl).Include(t => t.tblAccountHead).Include(t => t.tblBranch).Include(t => t.tblUser).Where(t => t.CompanyID == companyId && t.BranchID == branchId);
            return View(tblAccountSubControls.ToList());
        }

        // GET: tblAccountSubControls/Details/5
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            if (tblAccountSubControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSubControl);
        }

        // GET: tblAccountSubControls/Create
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
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(a => a.BranchID == branchId && a.CompanyID == companyId), "AccountControlID", "AccountControlName", "0");


            return View();
        }

        // POST: tblAccountSubControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAccountSubControl tblAccountSubControl)
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
            tblAccountSubControl.BranchID = branchId;
            tblAccountSubControl.CompanyID = companyId;
            tblAccountSubControl.UserID = userid;
            tblAccountSubControl.AccountHeadID = db.tblAccountControls.Find(tblAccountSubControl.AccountControlID).AccountHeadID;


            if (ModelState.IsValid)
            {
                var find = db.tblAccountSubControls.Where(s => s.CompanyID == tblAccountSubControl.CompanyID && s.BranchID == tblAccountSubControl.BranchID && s.AccountSubControlName == tblAccountSubControl.AccountSubControlName).FirstOrDefault();
                if (find == null)
                {
                    db.tblAccountSubControls.Add(tblAccountSubControl);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }


            }

            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(a => a.BranchID == branchId && a.CompanyID == companyId), "AccountControlID", "AccountControlName", tblAccountSubControl.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSubControl.AccountHeadID);

            return View(tblAccountSubControl);
        }

        // GET: tblAccountSubControls/Edit/5
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
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            if (tblAccountSubControl == null)
            {
                return HttpNotFound();
            }
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblAccountSubControl.BranchID = branchId;
            tblAccountSubControl.CompanyID = companyId;
            tblAccountSubControl.UserID = userid;

            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(a => a.BranchID == branchId && a.CompanyID == companyId), "AccountControlID", "AccountControlName", tblAccountSubControl.AccountControlID);


            return View(tblAccountSubControl);
        }

        // POST: tblAccountSubControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAccountSubControl tblAccountSubControl)
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
            tblAccountSubControl.BranchID = branchId;
            tblAccountSubControl.CompanyID = companyId;
            tblAccountSubControl.UserID = userid;
            tblAccountSubControl.AccountHeadID = db.tblAccountControls.Find(tblAccountSubControl.AccountControlID).AccountHeadID;


            if (ModelState.IsValid)
            {
                var find = db.tblAccountSubControls.Where(s => s.CompanyID == tblAccountSubControl.CompanyID && s.BranchID == tblAccountSubControl.BranchID && s.AccountSubControlName == tblAccountSubControl.AccountSubControlName && s.AccountSubControlID != tblAccountSubControl.AccountSubControlID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(tblAccountSubControl).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }
            }
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(a => a.BranchID == branchId && a.CompanyID == companyId), "AccountControlID", "AccountControlName", tblAccountSubControl.AccountControlID);

            return View(tblAccountSubControl);
        }

        // GET: tblAccountSubControls/Delete/5
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
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            if (tblAccountSubControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSubControl);
        }

        // POST: tblAccountSubControls/Delete/5
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
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            db.tblAccountSubControls.Remove(tblAccountSubControl);
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
