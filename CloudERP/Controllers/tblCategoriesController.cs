using DatabaseAccess;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class tblCategoriesController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblCategories
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

            var tblCategories = db.tblCategories.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser).Where(c => c.CompanyID == companyId && c.BranchID == branchId);
            return View(tblCategories.ToList());
        }

        // GET: tblCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // GET: tblCategories/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        // POST: tblCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCategory tblCategory)
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

            tblCategory.BranchID = branchId;
            tblCategory.CompanyID = companyId;
            tblCategory.UserID = userid;

            if (ModelState.IsValid)
            {
                var findcategory = db.tblCategories.Where(c => c.CompanyID == companyId && c.BranchID == branchId && c.categoryName == tblCategory.categoryName).FirstOrDefault();
                if (findcategory == null)
                {
                    db.tblCategories.Add(tblCategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }



            }


            return View(tblCategory);
        }

        // GET: tblCategories/Edit/5
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
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }

            return View(tblCategory);
        }

        // POST: tblCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCategory tblCategory)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblCategory.UserID = userid;

            if (ModelState.IsValid)
            {
                // Find the existing category by its ID
                var existingCategory = db.tblCategories.Find(tblCategory.CategoryID);
                if (existingCategory != null)
                {
                    // Check for duplicate category name in the same company and branch
                    var duplicateCategory = db.tblCategories
                        .Where(c => c.CompanyID == tblCategory.CompanyID &&
                                    c.BranchID == tblCategory.BranchID &&
                                    c.categoryName == tblCategory.categoryName &&
                                    c.CategoryID != tblCategory.CategoryID)
                        .FirstOrDefault();

                    if (duplicateCategory == null)
                    {
                        // Update existing category properties
                        existingCategory.categoryName = tblCategory.categoryName;
                        existingCategory.CompanyID = tblCategory.CompanyID;
                        existingCategory.BranchID = tblCategory.BranchID;
                        existingCategory.UserID = tblCategory.UserID;

                        // Mark entity as modified
                        db.Entry(existingCategory).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = "Already Exists";
                    }
                }
                else
                {
                    ViewBag.Message = "Category not found.";
                }
            }

            return View(tblCategory);
        }

        // GET: tblCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // POST: tblCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            db.tblCategories.Remove(tblCategory);
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
