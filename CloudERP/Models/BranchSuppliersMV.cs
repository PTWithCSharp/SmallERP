using System.ComponentModel.DataAnnotations;

namespace CloudERP.Models
{
    public class BranchSuppliersMV
    {
        [Key] 
        public int Id { get; set; }

        public string SupplierName { get; set; }
        public string SupplierConatctNo { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierEmail { get; set; }

        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string User { get; set; }
        public string Discription { get; set; }
    }
}