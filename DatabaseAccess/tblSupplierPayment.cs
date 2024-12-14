//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tblSupplierPayment
    {
        public int SupplierPaymentID { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Select Supplier")]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Enter Invoice No")]
        public int SupplierInvoiceID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string InvoiceNo { get; set; }

        [Required(ErrorMessage = "*Required")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Amount")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "*Required")]
        [DataType (DataType.Currency)]
        [Display(Name = "Enter Payment")]
        public double PaymentAmount { get; set; }
        public double RemainingBalance { get; set; }
        public int UserID { get; set; }
    
        public virtual tblSupplier tblSupplier { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
