using DatabaseAccess;
using DatabaseAccess.Code;
using DatabaseAccess.Code.SP_Code;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class SalePaymentController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        SP_Sale sale = new SP_Sale();
        private SaleEntry paymententry = new SaleEntry();
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

            var list = sale.RemainingSaleList(companyId, branchId);

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

            var list = sale.SalePaymentHistory((int)id);

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


            var list = sale.SalePaymentHistory((int)id);
            double remainingamount = 0;
            foreach (var item in list)
            {
                remainingamount = item.RemainingBalance;
            }
            if (remainingamount == 0)
            {
                remainingamount = db.tblCustomerInvoices.Find(id).TotalAmount;

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
                    var list = sale.SalePaymentHistory((int)id);
                    double remainingamount = 0;
                    foreach (var item in list)
                    {
                        remainingamount = item.RemainingBalance;
                    }
                    if (remainingamount == 0)
                    {
                        remainingamount = db.tblCustomerInvoices.Find(id).TotalAmount;

                    }

                    ViewBag.PreviousRemainingAmount = remainingamount;
                    ViewBag.InvoiceID = id;
                    return View(list);
                }

                string payinvoicenno = "INP" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                var customer = db.tblCustomers.Find(db.tblCustomerInvoices.Find(id).CustomerID);
                var purchaseinvoice = db.tblCustomerInvoices.Find(id);
                var purchasepaymentdetail = db.tblCustomerPayments.Where(p => p.CustomerInvoiceID == id);

                string message = paymententry.SalePayment(companyId, branchId, userid, payinvoicenno, Convert.ToString(id), (float)purchaseinvoice.TotalAmount
                    , paymentamount, Convert.ToString(customer.CustomerID), customer.Customername, (previousremainingamount - paymentamount));

                Session["Message"] = message;

                return RedirectToAction("RemainingPaymentList");
            }
            catch (Exception)
            {
                ViewBag.Message = "Please Try Again";
                var list = sale.SalePaymentHistory((int)id);
                double remainingamount = 0;
                foreach (var item in list)
                {
                    remainingamount = item.RemainingBalance;
                }
                if (remainingamount == 0)
                {
                    remainingamount = db.tblCustomerInvoices.Find(id).TotalAmount;

                }

                ViewBag.PreviousRemainingAmount = remainingamount;
                ViewBag.InvoiceID = id;
                return View(list);
            }
        }

        public ActionResult CustomSaleHistory(DateTime FromDate, DateTime ToDate)
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

            var list = sale.CustomSaleList(companyId, branchId, FromDate, ToDate);

            return View(list.ToList());
        }


        public ActionResult SaleItemDetail(int? id)
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

            var list = db.tblCustomerInvoiceDetails.Where(d => d.CustomerInvoiceID == id);

            return View(list.ToList());
        }
    }
}