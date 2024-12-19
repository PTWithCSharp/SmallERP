using CloudERP.Models;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class PurchaseCartController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        // GET: PurchaseCart
        public ActionResult NewPurchase()
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

            var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchId && i.CompanyID == companyId && i.UserID == userid);

            return View(find.ToList());
        }
        public ActionResult AddItem()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddItem(int PID, int Qty, float price)
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


            var find = db.tblPurchaseCartDetails.Where(i => i.ProductID == PID && i.BranchID == branchId && i.CompanyID == companyId).FirstOrDefault();
            if (find == null)
            {
                if (PID > 0 && Qty > 0 && price > 0)
                {
                    var newitem = new tblPurchaseCartDetail()
                    {
                        ProductID = PID,
                        PurchaseQuantity = Qty,
                        purchaseUnitPrice = price,
                        BranchID = branchId,
                        CompanyID = companyId,
                        UserID = userid,
                    };

                    db.tblPurchaseCartDetails.Add(newitem);
                    db.SaveChanges();
                    ViewBag.Message = "Item Added Successfully";
                }

            }
            else
            {
                ViewBag.Message = "Already Exists";

            }
            return RedirectToAction("NewPurchase");
        }

        [HttpGet]
        public ActionResult GetProduct()
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
            List<ProductMV> list = new List<ProductMV>();

            var productlist = db.tblStocks.Where(p => p.BranchID == branchId && p.CompanyID == companyId).ToList();
            foreach (var product in productlist)
            {
                list.Add(new ProductMV { Name = product.ProductName, ProductID = product.ProductID });
            }

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteConfirm(int? id)

        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int companyid = 0;
            int branchid = 0;
            int userid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var product = db.tblPurchaseCartDetails.Find(id);

            if (product != null)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                ViewBag.Message = "Deleted Successfully.";

                return RedirectToAction("NewPurchase");
            }
            ViewBag.Message = "Some Unexptected issue is occure, please contact to concern person!";
            var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);

            return View(find.ToList());

        }
        public ActionResult CancelPurchase()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var list = db.tblPurchaseCartDetails.Where(p => p.BranchID == branchid && p.CompanyID == companyid && p.UserID == userid).ToList();
            bool cancelstatus = false;

            foreach (var item in list)
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                int noofrecords = db.SaveChanges();

                if (cancelstatus == false)
                {
                    if (noofrecords > 0)
                    {
                        cancelstatus = true;
                    }
                }
            }
            if (cancelstatus == true)
            {
                ViewBag.Message = "Purchase is Canceled.";
                return RedirectToAction("NewPurchase");
            }
            ViewBag.Message = "Some Unexptected issue is occure, please contact to your administrator!";
            var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);

            return RedirectToAction("NewPurchase");

        }


        public ActionResult PurchaseConfirm()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }


        [HttpPost]
        public ActionResult PurchaseConfirm(tblSupplier purchase)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

    }
}