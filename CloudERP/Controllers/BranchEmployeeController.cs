﻿using CloudERP.HelperClass;
using DatabaseAccess;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class BranchEmployeeController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        // GET: BranchEmployee
        public ActionResult Employee()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyId = 0;
            int branchId = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            var tblEmployee = db.tblEmployees.Where(c => c.CompanyID == companyId && c.BranchID == branchId);

            return View(tblEmployee);
        }



        public ActionResult EmployeeRegistration()
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeRegistration(tblEmployee employee)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int companyId = 0;
            int branchId = 0;
            branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));

            employee.BranchID = branchId;
            employee.CompanyID = companyId;

            if (ModelState.IsValid)
            {
                var folder = "~/Content/EmployeePhoto";
                var name = string.Format("{0}.png", employee.EmployeeID);
                var file = employee.LogoFile;
                var response = FileHelper.UploadPhoto(file, folder, name);

                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, name);
                    employee.Photo = pic;

                }

                db.tblEmployees.Add(employee);
                System.Diagnostics.Debug.WriteLine($"Employee ID: {employee.EmployeeID}, " +
                                    $"Name: {employee.Name}, " +
                                    $"Contact No: {employee.ContactNo}, " +
                                    $"Photo: {employee.Photo}, " +
                                    $"Address: {employee.Address}, " +
                                    $"CNIC: {employee.CNIC}, " +
                                    $"Designation: {employee.Designation}, " +
                                    $"Description: {employee.Description}, " +
                                    $"Branch ID: {employee.BranchID}, " +
                                    $"Company ID: {employee.CompanyID}, " +
                                    $"User ID: {employee.UserID}");

                db.SaveChanges();
                return RedirectToAction("Employee");

            }
            else
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return RedirectToAction("Employee");
            }


        }




        public ActionResult EmployeeUpdation(int? id)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = db.tblEmployees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeUpdation(tblEmployee employee)
        {
            if (string.IsNullOrEmpty(System.Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            //int companyId = 0;
            //int branchId = 0;
            //branchId = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            //companyId = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));

            //employee.BranchID = branchId;
            //employee.CompanyID = companyId;

            if (ModelState.IsValid)
            {
                var existingEmployee = db.tblEmployees.Find(employee.EmployeeID);
                if (existingEmployee == null)
                {
                    return HttpNotFound(); // Handle case where employee does not exist
                }


                existingEmployee.Name = employee.Name;
                existingEmployee.ContactNo = employee.ContactNo;
                existingEmployee.Email = employee.Email;
                existingEmployee.Address = employee.Address;
                existingEmployee.CNIC = employee.CNIC;
                existingEmployee.Designation = employee.Designation;
                existingEmployee.Description = employee.Description;
                existingEmployee.MonthlySalary = employee.MonthlySalary;

                var folder = "~/Content/EmployeePhoto";
                var name = string.Format("{0}.png", employee.EmployeeID);
                var file = employee.LogoFile;
                var response = FileHelper.UploadPhoto(file, folder, name);

                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, name);
                    existingEmployee.Photo = pic;


                }

                db.SaveChanges();
                return RedirectToAction("Employee");
            }
            return View();
        }
    }
}