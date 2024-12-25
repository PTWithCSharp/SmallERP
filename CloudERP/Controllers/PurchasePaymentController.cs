using DatabaseAccess;
using DatabaseAccess.Code;
using DatabaseAccess.Code.SP_Code;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class PurchasePaymentController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        SP_Purchase purchase = new SP_Purchase();
        private PurchaseEntry paymententry = new PurchaseEntry();
        // GET: PurchasePayment
        public ActionResult RemainingPaymentList()
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

            var list = purchase.RemainingPaymentList(companyId, branchId);

            return View(list.ToList());
        }
        public ActionResult PaidHistory(int? id)
        {
            int companyId = 0;
            int branchId = 0;
            int userid = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var list = purchase.PurchasePaymentHistory((int)id);

            return View(list.ToList());
        }


        public ActionResult PaidAmount(int? id)
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


            var list = purchase.PurchasePaymentHistory((int)id);
            double remainingamount = 0;
            foreach (var item in list)
            {
                remainingamount = item.RemainingBalance;
            }
            if (remainingamount == 0)
            {
                remainingamount = db.tblSupplierInvoices.Find(id).TotalAmount;

            }

            ViewBag.PreviousRemainingAmount = remainingamount;
            ViewBag.InvoiceID = id;

            return View(list.ToList());
        }

        [HttpPost]
        public ActionResult PaidAmount(int? id, float previousremainingamount, float paymentamount)
        {
            try
            {
                if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])) || string.IsNullOrEmpty(System.Convert.ToString(id)))
                {
                    return RedirectToAction("Login", "Home");
                }



                int companyId = 0;
                int branchId = 0;
                int userid = 0;
                branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
                companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
                userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

                if (previousremainingamount < paymentamount)
                {

                    ViewBag.Message = "Payment must less than or equal to previous remaining amount";
                    var list = purchase.PurchasePaymentHistory((int)id);
                    double remainingamount = 0;
                    foreach (var item in list)
                    {
                        remainingamount = item.RemainingBalance;
                    }
                    if (remainingamount == 0)
                    {
                        remainingamount = db.tblSupplierInvoices.Find(id).TotalAmount;

                    }

                    ViewBag.PreviousRemainingAmount = remainingamount;
                    ViewBag.InvoiceID = id;
                    return View(list);
                }

                string payinvoicenno = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                var supplier = db.tblSuppliers.Find(db.tblSupplierInvoices.Find(id).SupplierID);
                var purchaseinvoice = db.tblSupplierInvoices.Find(id);
                var purchasepaymentdetail = db.tblSupplierPayments.Where(p => p.SupplierInvoiceID == id);

                string message = paymententry.PurchasePayment(companyId, branchId, userid, payinvoicenno, Convert.ToString(id), (float)purchaseinvoice.TotalAmount
                    , paymentamount, Convert.ToString(supplier.SupplierID), supplier.SupplierName, (previousremainingamount - paymentamount));

                Session["Message"] = message;

                return RedirectToAction("RemainingPaymentList");
            }
            catch (Exception)
            {
                ViewBag.Message = "Please Try Again";
                var list = purchase.PurchasePaymentHistory((int)id);
                double remainingamount = 0;
                foreach (var item in list)
                {
                    remainingamount = item.RemainingBalance;
                }
                if (remainingamount == 0)
                {
                    remainingamount = db.tblSupplierInvoices.Find(id).TotalAmount;

                }

                ViewBag.PreviousRemainingAmount = remainingamount;
                ViewBag.InvoiceID = id;
                return View(list);
            }
        }

        public ActionResult CustomPurchaseHistory(DateTime FromDate, DateTime ToDate)
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

            var list = purchase.CustomePurchaseList(companyId, branchId, FromDate, ToDate);

            return View(list.ToList());
        }

        public ActionResult PurchaseItemDetail(int? id)
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

            var list = db.tblSupplierInvoiceDetails.Where(d => d.SupplierInvoiceID == id);

            return View(list.ToList());
        }
    }
}