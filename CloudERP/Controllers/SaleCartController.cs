using CloudERP.Models;
using DatabaseAccess;
using DatabaseAccess.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class SaleCartController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        private SaleEntry saleentry = new SaleEntry();

        // GET: SaleCart
        public ActionResult NewSale()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            double totalamount = 0;
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var find = db.tblSaleCartDetails.Where(i => i.BranchID == branchId && i.CompanyID == companyId && i.UserID == userid);

            foreach (var item in find)
            {
                totalamount += (item.SaleQuantity * item.SaleUnitPrice);

            }
            ViewBag.TotalAmount = totalamount;

            return View(find.ToList());
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
            var checkqty = db.tblStocks.Find(PID);
            if (Qty > checkqty.Quantity)
            {
                ViewBag.Message = "Sale Quantity must be less than or equal to avl quantity";
                return RedirectToAction("NewSale");
            }


            var find = db.tblSaleCartDetails.Where(i => i.ProductID == PID && i.BranchID == branchId && i.CompanyID == companyId).FirstOrDefault();
            if (find == null)
            {
                if (PID > 0 && Qty > 0 && price > 0)
                {
                    var newitem = new tblSaleCartDetail()
                    {
                        ProductID = PID,
                        SaleQuantity = Qty,
                        SaleUnitPrice = price,
                        BranchID = branchId,
                        CompanyID = companyId,
                        UserID = userid,
                    };

                    db.tblSaleCartDetails.Add(newitem);
                    db.SaveChanges();
                    ViewBag.Message = "Item Added Successfully";
                }

            }
            else
            {
                ViewBag.Message = "Already Exists";

            }
            return RedirectToAction("NewSale");
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

            var product = db.tblSaleCartDetails.Find(id);

            if (product != null)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                ViewBag.Message = "Deleted Successfully.";

                return RedirectToAction("NewSale");
            }
            ViewBag.Message = "Some Unexptected issue is occure, please contact to concern person!";
            var find = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);

            return View(find.ToList());

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
                list.Add(new ProductMV { Name = product.ProductName + "(Avl Qty: " + product.Quantity + ")", ProductID = product.ProductID });
            }

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult GetProductDetails(int? id)
        {



            var product = db.tblStocks.Find(id);


            return Json(new { data = product.SaleUnitPrice }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult CancelSale()
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

            var list = db.tblSaleCartDetails.Where(p => p.BranchID == branchid && p.CompanyID == companyid && p.UserID == userid).ToList();
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
                ViewBag.Message = "Sale is Canceled.";
                return RedirectToAction("NewSale");
            }
            ViewBag.Message = "Some Unexptected issue is occure, please contact to your administrator!";
            // var find = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);

            return RedirectToAction("NewSale");

        }


        public ActionResult SelectCustomer()
        {
            Session["ErrorSaleMessage"] = string.Empty;
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var saledetails = db.tblSaleCartDetails.Where(pd => pd.CompanyID == companyid && pd.BranchID == branchid).FirstOrDefault();
            if (saledetails == null)
            {
                Session["ErrorSaleMessage"] = "Sale Cart Empty";
                return RedirectToAction("NewSale");
            }

            var customers = db.tblCustomers.Where(s => s.CompanyID == companyid && s.BranchID == branchid).ToList();

            return View(customers);
        }


        [HttpPost]
        public ActionResult SaleConfirm(FormCollection formCollection)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            int customerid = 0;
            bool IsPayment = false;

            string[] keys = formCollection.AllKeys;
            foreach (var name in keys)
            {
                if (name.Contains("name"))
                {
                    string idname = name;
                    string[] valueids = idname.Split(' ');
                    customerid = Convert.ToInt32(valueids[1]);
                    System.Diagnostics.Debug.WriteLine($"idname:{idname}");
                    System.Diagnostics.Debug.WriteLine($"valueids:{valueids}");
                    System.Diagnostics.Debug.WriteLine($"supplierid:{customerid}");
                }
            }
            string Description = string.Empty;
            string[] Descriptionlist = formCollection["item.Description"].Split(',');
            if (Descriptionlist != null)
            {
                if (Descriptionlist[0] != null)
                {
                    Description = Descriptionlist[0];
                }
            }

            if (formCollection["IsPayment"] != null)
            {
                string[] ispaymentdircet = formCollection["IsPayment"].Split(',');
                if (ispaymentdircet[0] == "on")
                {
                    IsPayment = true;
                }
                else
                {
                    IsPayment = false;
                }
            }


            var customer = db.tblCustomers.Find(customerid);
            var saledetails = db.tblSaleCartDetails.Where(pd => pd.BranchID == branchid && pd.CompanyID == companyid).ToList();
            double totalamount = 0;
            foreach (var item in saledetails)
            {
                totalamount = totalamount + (item.SaleQuantity * item.SaleUnitPrice);
            }
            if (totalamount == 0)
            {
                ViewBag.Message = "Sale Cart Empty";
                return View("NewSale");
            }

            string Invoiceno = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

            var invoiceheader = new tblCustomerInvoice()
            {
                BranchID = branchid,
                Title = "Sale Invoice" + customer.Customername,
                CompanyID = companyid,
                Description = Description,
                InvoiceDate = DateTime.Now,
                InvoiceNo = Invoiceno,
                CustomerID = customerid,
                UserID = userid,
                TotalAmount = totalamount,
            };
            db.tblCustomerInvoices.Add(invoiceheader);
            db.SaveChanges();

            foreach (var item in saledetails)
            {
                var purdetails = new tblCustomerInvoiceDetail()
                {
                    ProductID = item.ProductID,
                    SaleQuantity = item.SaleQuantity,
                    SaleUnitPrice = item.SaleUnitPrice,
                    CustomerInvoiceID = invoiceheader.CustomerInvoiceID,

                };
                db.tblCustomerInvoiceDetails.Add(purdetails);
                db.SaveChanges();
            }
            string Message = saleentry.ConfirmSale(companyid, branchid, userid, Invoiceno, invoiceheader.CustomerInvoiceID.ToString(), (float)totalamount, customerid.ToString(), customer.Customername, IsPayment);

            if (Message.Contains("Success"))
            {
                foreach (var item in saledetails)
                {
                    var stockitem = db.tblStocks.Find(item.ProductID);

                    stockitem.Quantity = stockitem.Quantity - item.SaleQuantity;
                    db.Entry(stockitem).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            Session["Message"] = Message;

            return RedirectToAction("NewSale");
        }

    }
}