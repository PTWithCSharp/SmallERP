using DatabaseAccess;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class CompanyRegistrationController : Controller
    {
        CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: CompanyRegistration
        public ActionResult RegistrationForm()
        {
            return View();
        }


        [HttpPost]
        public ActionResult RegistrationForm(
            string UserName,
            string Password,
            string CPassword,
            string EName,
            string EContactNo,
            string EEmail,
            string ECNIC,
            string EDesignation,
            float EMonthlySalary,
            string EAddress,
            string CName,
            string BranchName,
            string BranchContact,
            string BranchAddress
            )
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName)
                     && !string.IsNullOrEmpty(Password)
                     && !string.IsNullOrEmpty(CPassword)
                     && !string.IsNullOrEmpty(EName)
                     && !string.IsNullOrEmpty(EContactNo)
                     && !string.IsNullOrEmpty(EEmail)
                     && !string.IsNullOrEmpty(ECNIC)
                     && !string.IsNullOrEmpty(EDesignation)
                     && EMonthlySalary >= 0
                     && !string.IsNullOrEmpty(EAddress)
                     && !string.IsNullOrEmpty(CName)
                     && !string.IsNullOrEmpty(BranchName)
                     && !string.IsNullOrEmpty(BranchContact)
                     && !string.IsNullOrEmpty(BranchAddress))

                {
                    var company = new tblCompany() { Name = CName, Logo = string.Empty };

                    db.tblCompanies.Add(company);
                    db.SaveChanges();

                    var branch = new tblBranch()
                    {
                        BranchAddress = BranchAddress,
                        BranchContact = BranchContact,
                        BranchName = BranchName,
                        BranchTypeID = 1,
                        CompanyID = company.CompanyID,
                        BrchID = null
                    };
                    db.tblBranches.Add(branch);
                    db.SaveChanges();

                    var user = new tblUser()
                    {
                        ContactNo = EContactNo,
                        Email = EEmail,
                        FullName = EName,
                        IsActive = true,
                        Password = Password,
                        UserName = UserName,
                        UserTypeID = 1,
                    };
                    db.tblUsers.Add(user);
                    db.SaveChanges();

                    var employee = new tblEmployee()
                    {
                        Address = EAddress,
                        BranchID = branch.BranchID,
                        CNIC = ECNIC,
                        CompanyID = company.CompanyID,
                        ContactNo = EContactNo,
                        Designation = EDesignation,
                        Email = EEmail,
                        MonthlySalary = EMonthlySalary,

                        Name = EName,
                        Description = string.Empty,
                        UserID = user.UserID,
                    };
                    db.tblEmployees.Add(employee);
                    db.SaveChanges();

                    ViewBag.Message = "Registration Succesfully";

                    return RedirectToAction("Login", "Home");

                }
                else
                {
                    ViewBag.Message = "Incorrect Information";
                    return View("RegistrationForm");

                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Message = "Something Went Wrong, Please Contact Administrator";
                throw ex;
            }

        }
    }
}