using CloudERP.HelperClass;
using DatabaseAccess;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class tblCompaniesController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblCompanies
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View(db.tblCompanies.ToList());
        }

        // GET: tblCompanies/Details/5
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
            tblCompany tblCompany = db.tblCompanies.Find(id);
            if (tblCompany == null)
            {
                return HttpNotFound();
            }
            return View(tblCompany);
        }

        // GET: tblCompanies/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: tblCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCompany tblCompany)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.tblCompanies.Add(tblCompany);
                db.SaveChanges();
                if (tblCompany.LogoFile != null)
                {
                    var folder = "~Content/CompanyLogos";
                    var file = string.Format("{0}.jpg", tblCompany.CompanyID);
                    var response = FileHelper.UploadPhoto(tblCompany.LogoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        tblCompany.Logo = pic;

                        db.Entry(tblCompany).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }


                return RedirectToAction("Index");
            }

            return View(tblCompany);
        }

        // GET: tblCompanies/Edit/5
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
            tblCompany tblCompany = db.tblCompanies.Find(id);
            if (tblCompany == null)
            {
                return HttpNotFound();
            }
            return View(tblCompany);
        }

        // POST: tblCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCompany tblCompany)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var existingCompany = db.tblCompanies.Find(tblCompany.CompanyID);
                if (existingCompany == null)
                {
                    return HttpNotFound(); // Handle case where company does not exist
                }

                // Update properties
                existingCompany.Name = tblCompany.Name;
                if (tblCompany.LogoFile != null)
                {
                    var folder = "~/Content/CompanyLogos";
                    var name = string.Format("{0}.png", tblCompany.CompanyID);
                    var file = tblCompany.LogoFile;
                    var response = FileHelper.UploadPhoto(file, folder, name);

                    var pic = string.Format("{0}/{1}", folder, name);
                    existingCompany.Logo = pic;

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCompany);
        }

        // GET: tblCompanies/Delete/5
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
            tblCompany tblCompany = db.tblCompanies.Find(id);
            if (tblCompany == null)
            {
                return HttpNotFound();
            }
            return View(tblCompany);
        }

        // POST: tblCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblCompany tblCompany = db.tblCompanies.Find(id);
            db.tblCompanies.Remove(tblCompany);
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
