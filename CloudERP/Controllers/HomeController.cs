using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class HomeController : Controller
    {
        DatabaseAccess.CloudErpV1Entities db = new DatabaseAccess.CloudErpV1Entities();
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoginUser(string email, string password)
        {
            var user = db.tblUsers.Where(u => u.Email == email && u.Password == password && u.IsActive == true).FirstOrDefault();
            if (user != null)
            {
                Session["UserId"] = user.UserID;
                Session["UserTypeID"] = user.UserTypeID;
                Session["FullName"] = user.FullName;
                Session["Email"] = user.Email;
                Session["ContactNo"] = user.ContactNo;
                Session["UserName"] = user.UserName;
                Session["Password"] = user.Password;
                Session["IsActive"] = user.IsActive;

                var EmployeeDetails = db.tblEmployees.Where(e => e.UserID == user.UserID).FirstOrDefault();
                if (EmployeeDetails == null)
                {
                    ViewBag.Message = "Please Contact Administrator";
                    Session["UserTypeID"] = string.Empty;
                    Session["FullName"] = string.Empty;
                    Session["Email"] = string.Empty;
                    Session["ContactNo"] = string.Empty;
                    Session["UserName"] = string.Empty;
                    Session["Password"] = string.Empty;
                    Session["IsActive"] = string.Empty;

                    Session["EmployeeID"] = string.Empty;
                    Session["EName"] = string.Empty;
                    Session["EPhoto"] = string.Empty;
                    Session["Designation"] = string.Empty;
                    Session["BranchID"] = string.Empty;
                    Session["CompanyID"] = string.Empty;
                    return View("Login");

                }

                Session["EmployeeID"] = EmployeeDetails.EmployeeID;
                Session["EName"] = EmployeeDetails.Name;
                Session["EPhoto"] = EmployeeDetails.Photo;
                Session["Designation"] = EmployeeDetails.Designation;
                Session["BranchID"] = EmployeeDetails.BranchID;
                Session["CompanyID"] = EmployeeDetails.CompanyID;

                var company = db.tblCompanies.Where(c => c.CompanyID == EmployeeDetails.CompanyID).FirstOrDefault();
                if (company == null)
                {
                    ViewBag.Message = "Please Contact Administrator";
                    Session["UserTypeID"] = string.Empty;
                    Session["FullName"] = string.Empty;
                    Session["Email"] = string.Empty;
                    Session["ContactNo"] = string.Empty;
                    Session["UserName"] = string.Empty;
                    Session["Password"] = string.Empty;
                    Session["IsActive"] = string.Empty;

                    Session["EmployeeID"] = string.Empty;
                    Session["EName"] = string.Empty;
                    Session["EPhoto"] = string.Empty;
                    Session["Designation"] = string.Empty;
                    Session["BranchID"] = string.Empty;
                    Session["CompanyID"] = string.Empty;
                    return View("Login");

                }

                Session["CName"] = company.Name;
                Session["Logo"] = company.Logo;

                var BranchType = db.tblBranches.Where(b => b.BranchID == EmployeeDetails.BranchID).FirstOrDefault();
                if (BranchType == null)
                {
                    ViewBag.Message = "Please Contact Administrator";
                    return View("Login");
                }
                Session["BranchTypeID"] = BranchType.BranchTypeID;

                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Message = "incorrect email or password";

                Session["UserTypeID"] = string.Empty;
                Session["FullName"] = string.Empty;
                Session["Email"] = string.Empty;
                Session["ContactNo"] = string.Empty;
                Session["UserName"] = string.Empty;
                Session["Password"] = string.Empty;
                Session["IsActive"] = string.Empty;

            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session["UserTypeID"] = string.Empty;
            Session["FullName"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["ContactNo"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["IsActive"] = string.Empty;

            Session["EmployeeID"] = string.Empty;
            Session["EName"] = string.Empty;
            Session["EPhoto"] = string.Empty;
            Session["Designation"] = string.Empty;
            Session["BranchID"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            return View("Login");

        }

        public ActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            // Find the user in the database
            var user = db.tblUsers.Where(u => u.Email == email).FirstOrDefault();

            if (user != null)
            {
                // Generate a new random password
                string newPassword = GenerateRandomPassword();

                // Update the user's password in the database (hashed if needed)
                user.Password = newPassword; // Use a hashing mechanism here if passwords are hashed
                db.SaveChanges();

                // Send the new password via Gmail
                string subject = "Your New Password";
                string body = $"Hello {user.UserName},\n\nYour new password is: {newPassword}\n\nPlease change your password after logging in.";

                try
                {
                    SendEmail(email, subject, body);
                    ViewBag.Message = "A new password has been sent to your email address.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error while sending email: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Email address not found.";
            }

            return View("Login");
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                                  .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Helper method to send email using MailDev
        private void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "test@example.com"; // Use any dummy email address
            var smtp = new SmtpClient
            {
                Host = "localhost",  // MailDev runs on localhost
                Port = 1025,         // MailDev's default SMTP port
                EnableSsl = false,   // MailDev does not require SSL
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true // MailDev does not require authentication
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}