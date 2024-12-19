using DatabaseAccess;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class tblStocksController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblStocks
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyId = 0;
            int branchId = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            var tblStocks = db.tblStocks.Include(t => t.tblBranch).Include(t => t.tblCategory).Include(t => t.tblCompany).Include(t => t.tblUser).Where(t => t.CompanyID == companyId && t.BranchID == branchId);
            return View(tblStocks.ToList());
        }

        // GET: tblStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            return View(tblStock);
        }

        // GET: tblStocks/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyId = 0;
            int branchId = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));

            ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "CategoryID", "categoryName", "0");
            return View();
        }

        // POST: tblStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblStock tblStock)
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

            tblStock.BranchID = branchId;
            tblStock.CompanyID = companyId;
            tblStock.UserID = userid;
            if (ModelState.IsValid)
            {
                var findproduct = db.tblStocks.Where(p => p.CompanyID == companyId && p.BranchID == branchId && p.ProductName == tblStock.ProductName).FirstOrDefault();
                if (findproduct == null)
                {
                    db.tblStocks.Add(tblStock);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Stock Is Already Exists ";
                }

            }


            ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == branchId && c.CompanyID == companyId), "CategoryID", "categoryName", tblStock.CategoryID);
            return View(tblStock);
        }

        // GET: tblStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == tblStock.BranchID && c.CompanyID == tblStock.CompanyID), "CategoryID", "categoryName", tblStock.CategoryID);

            return View(tblStock);
        }

        // POST: tblStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblStock tblStock)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            int userid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblStock.UserID = userid;
            if (ModelState.IsValid)
            {
                var findproduct = db.tblStocks.Where(p => p.CompanyID == tblStock.CompanyID && p.BranchID == tblStock.BranchID && p.ProductName == tblStock.ProductName && p.ProductID != tblStock.ProductID).FirstOrDefault();
                if (findproduct == null)
                {
                    db.Entry(tblStock).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Stock Is Already Exists ";
                }

            }


            ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == tblStock.BranchID && c.CompanyID == tblStock.CompanyID), "CategoryID", "categoryName", tblStock.CategoryID);

            return View(tblStock);
        }

        // GET: tblStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            return View(tblStock);
        }

        // POST: tblStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblStock tblStock = db.tblStocks.Find(id);
            db.tblStocks.Remove(tblStock);
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
