using System.ComponentModel.DataAnnotations;

namespace CloudERP.Models
{
    public class BranchCustomerMV
    {
        [Key]
        public int Id { get; set; } // Define a unique identifier (Primary Key)
        public string CompanyName { get; set; }
        public string BranchName { get; set; }



        public string Customername { get; set; }

        public string CustomerContact { get; set; }
        public string CustomerArea { get; set; }
        public string CustomerAddress { get; set; }
        public string Description { get; set; }
        public string User { get; set; }
    }
}