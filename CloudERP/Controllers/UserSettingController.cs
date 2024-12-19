using DatabaseAccess;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class UserSettingController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        // GET: UserSetting
        public ActionResult CreateUser(int? employeeid)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            Session["CEmployeeID"] = employeeid;
            var employee = db.tblEmployees.Find(employeeid);
            var user = new tblUser();

            user.Email = employee.Email;
            user.ContactNo = employee.ContactNo;
            user.FullName = employee.Name;
            user.IsActive = true;
            user.Password = employee.ContactNo;
            user.UserName = employee.Email;

            ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(tblUser user)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var guser = db.tblUsers.Where(u => u.Email == user.Email && u.UserID != user.UserID);
                if (guser.Count() > 0)
                {
                    ViewBag.Message = "Email is Already registered";
                }
                else
                {

                    db.tblUsers.Add(user);
                    db.SaveChanges(); // After this, user.UserID is auto-populated
                    //System.Diagnostics.Debug.WriteLine($"New UserID generated: {user.UserID}");

                    // Update the related employee record
                    int? employeeid = Convert.ToInt32(Session["CEmployeeID"]);
                    //System.Diagnostics.Debug.WriteLine($"Session[\"CEmployeeID\"]: {Session["CEmployeeID"]}");
                    //System.Diagnostics.Debug.WriteLine($"Converted employeeid: {employeeid}");

                    var employee = db.tblEmployees.Find(employeeid);
                    if (employee != null)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Employee.UserID before update: {employee.UserID}");

                        // Attach the employee to context and update only the UserID property
                        db.Entry(employee).State = System.Data.Entity.EntityState.Unchanged;
                        employee.UserID = user.UserID;
                        db.Entry(employee).Property(e => e.UserID).IsModified = true;

                        db.SaveChanges(); // Only updates the UserID column
                        //System.Diagnostics.Debug.WriteLine($"Employee.UserID after update: {employee.UserID}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Employee not found.");
                    }
                    Session["CEmployeeID"] = null;

                    return RedirectToAction("Index", "tblUsers");

                }
            }
            if (user == null)
            {
                ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType");
            }
            else
            {
                ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType", user.UserTypeID);
            }

            return RedirectToAction("Index", "tblUsers");



        }

    }
}