using DatabaseAccess;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class UserFinancialYearController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: UserFinancialYear
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            int userid = 0;

            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var tblFinancialYears = db.tblFinancialYears;
            return View(tblFinancialYears.ToList());
        }
    }
}