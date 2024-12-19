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
    public class tblCustomersController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();


        public ActionResult AllCustomer()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var tblCustomers = db.tblCustomers.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser);
            return View(tblCustomers.ToList());
        }
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

            var tblCustomers = db.tblCustomers.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser).Where(c => c.CompanyID == companyId && c.BranchID == branchId);
            return View(tblCustomers.ToList());
        }

        // GET: tblCustomers
        public ActionResult SubBranchCustomer()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<BranchCustomerMV> list = new List<BranchCustomerMV>();
            int branchid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));

            List<int> branchids = CloudERP.HelperClass.Branch.GetBranchids(branchid, db);


            foreach (var item in branchids)
            {
                foreach (var customer in db.tblCustomers.Where(c => c.BranchID == item))
                {
                    var cus = new BranchCustomerMV();
                    cus.BranchName = customer.tblBranch.BranchName;
                    cus.CompanyName = customer.tblCompany.Name;
                    cus.CustomerAddress = customer.CustomerAddress;
                    cus.CustomerArea = customer.CustomerArea;
                    cus.CustomerContact = customer.CustomerContact;
                    cus.Customername = customer.Customername;
                    cus.Description = customer.Description;
                    cus.User = customer.tblUser.UserName;
                    list.Add(cus);


                }
            }


            return View(list);
        }


        // GET: tblCustomers/Details/5
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
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // GET: tblCustomers/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        // POST: tblCustomers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblCustomer tblCustomer)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyId = 0;
            int branchId = 0;
            int userid = 0;

            int usertypeid = 0;
            int branchtypeid = 0;

            usertypeid = Convert.ToInt32(Convert.ToString(Session["UserTypeID"]));
            branchtypeid = Convert.ToInt32(Convert.ToString(Session["BranchTypeID"]));

            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblCustomer.BranchID = branchId;
            tblCustomer.CompanyID = companyId;
            tblCustomer.UserID = userid;



            if (ModelState.IsValid)
            {
                var find = db.tblCustomers.Where(c => c.Customername == tblCustomer.Customername && c.CustomerContact == tblCustomer.CustomerContact).FirstOrDefault();
                if (find == null)
                {
                    db.tblCustomers.Add(tblCustomer);
                    db.SaveChanges();
                    if (usertypeid == 1 && branchtypeid == 1)
                    {
                        return RedirectToAction("AllCustomer");
                    }
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }


            }

            return View(tblCustomer);
        }

        // GET: tblCustomers/Edit/5
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
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // POST: tblCustomers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblCustomer tblCustomer)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int userid = 0;
            int usertypeid = 0;
            int branchtypeid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            usertypeid = Convert.ToInt32(Convert.ToString(Session["UserTypeID"]));
            branchtypeid = Convert.ToInt32(Convert.ToString(Session["BranchTypeID"]));

            tblCustomer.UserID = userid;




            if (ModelState.IsValid)
            {
                var find = db.tblCustomers.Where(c => c.Customername == tblCustomer.Customername && c.CustomerContact == tblCustomer.CustomerContact && c.CustomerID != tblCustomer.CustomerID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(tblCustomer).State = EntityState.Modified;
                    db.SaveChanges();
                    if (usertypeid == 1 && branchtypeid == 1)
                    {
                        return RedirectToAction("AllCustomer");
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already Exists";
                }


            }

            return View(tblCustomer);
        }

        // GET: tblCustomers/Delete/5
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
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            if (tblCustomer == null)
            {
                return HttpNotFound();
            }
            return View(tblCustomer);
        }

        // POST: tblCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblCustomer tblCustomer = db.tblCustomers.Find(id);
            db.tblCustomers.Remove(tblCustomer);
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
