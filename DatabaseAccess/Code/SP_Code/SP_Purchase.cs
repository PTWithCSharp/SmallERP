using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseAccess.Code.SP_Code
{
    public class SP_Purchase
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        public List<PurchasePaymentModel> RemainingPaymentList(int CompanyID, int BranchID)
        {
            var remainingpaymentlist = new List<PurchasePaymentModel>();

            SqlCommand command = new SqlCommand("GetSupplierRemainingPaymentRecord", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BranchID", BranchID);
            command.Parameters.AddWithValue("@CompanyID", CompanyID);

            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                int supplierid = Convert.ToInt32(Convert.ToString(row["SupplierID"]));
                var supplier = db.tblSuppliers.Find(supplierid);




                var payment = new PurchasePaymentModel();


                payment.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                payment.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                payment.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                payment.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                payment.InvoiceNo = Convert.ToString(row[5]);

                double payamount = 0;
                double.TryParse(Convert.ToString(row[7]), out payamount);
                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingbalance);
                double totalamount = 0;
                double.TryParse(Convert.ToString(row[6]), out totalamount);

                payment.PaymentAmount = payamount;
                payment.RemainingBalance = remainingbalance;
                payment.SupplierContactNo = supplier.SupplierConatctNo;
                payment.SupplierAddress = supplier.SupplierAddress;
                payment.SupplierID = supplier.SupplierID;
                payment.SupplierName = supplier.SupplierName;
                payment.TotalAmount = totalamount;





                remainingpaymentlist.Add(payment);
            }

            return remainingpaymentlist;

        }


        public List<PurchasePaymentModel> CustomePurchaseList(int CompanyID, int BranchID, DateTime FromDate, DateTime ToDate)
        {
            var remainingpaymentlist = new List<PurchasePaymentModel>();

            SqlCommand command = new SqlCommand("GetPurchasesHistory", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BranchID", BranchID);
            command.Parameters.AddWithValue("@CompanyID", CompanyID);
            command.Parameters.AddWithValue("@FromDate", FromDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@ToDate", ToDate.ToString("yyyy-MM-dd"));

            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                int supplierid = Convert.ToInt32(Convert.ToString(row["SupplierID"]));
                var supplier = db.tblSuppliers.Find(supplierid);




                var payment = new PurchasePaymentModel();


                payment.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                payment.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                payment.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                payment.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                payment.InvoiceNo = Convert.ToString(row[5]);

                double payamount = 0;
                double.TryParse(Convert.ToString(row[7]), out payamount);
                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingbalance);
                double totalamount = 0;
                double.TryParse(Convert.ToString(row[6]), out totalamount);

                payment.PaymentAmount = payamount;
                payment.RemainingBalance = remainingbalance;
                payment.SupplierContactNo = supplier.SupplierConatctNo;
                payment.SupplierAddress = supplier.SupplierAddress;
                payment.SupplierID = supplier.SupplierID;
                payment.SupplierName = supplier.SupplierName;
                payment.TotalAmount = totalamount;





                remainingpaymentlist.Add(payment);
            }

            return remainingpaymentlist;

        }

        public List<PurchasePaymentModel> PurchasePaymentHistory(int SupplierInvoiceID)
        {
            var remainingpaymentlist = new List<PurchasePaymentModel>();

            SqlCommand command = new SqlCommand("GetSupplierPaymentHistory", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SupplierInvoiceID", SupplierInvoiceID);


            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                int supplierid = Convert.ToInt32(Convert.ToString(Convert.ToString(row[4])));
                int userid = Convert.ToInt32(Convert.ToString(row[9]));
                var supplier = db.tblSuppliers.Find(supplierid);
                var user = db.tblUsers.Find(userid);




                var payment = new PurchasePaymentModel();


                payment.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                payment.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                payment.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                payment.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                payment.InvoiceNo = Convert.ToString(row[5]);

                double payamount = 0;
                double.TryParse(Convert.ToString(row[7]), out payamount);
                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingbalance);
                double totalamount = 0;
                double.TryParse(Convert.ToString(row[6]), out totalamount);

                payment.PaymentAmount = payamount;
                payment.RemainingBalance = remainingbalance;
                payment.SupplierContactNo = supplier.SupplierConatctNo;
                payment.SupplierAddress = supplier.SupplierAddress;
                payment.SupplierID = supplier.SupplierID;
                payment.SupplierName = supplier.SupplierName;
                payment.UserID = user.UserID;
                payment.UserName = user.UserName;
                payment.TotalAmount = totalamount;





                remainingpaymentlist.Add(payment);
            }

            return remainingpaymentlist;

        }


    }
}
