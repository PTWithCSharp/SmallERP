using System;
using System.Data;
using System.Linq;

namespace DatabaseAccess.Code
{
    public class PurchaseEntry
    {
        public string selectsupplierid = string.Empty;
        DataTable dtEntries = null;
        private CloudErpV1Entities db = new CloudErpV1Entities();

        public string ConfirmPurchase(int CompanyID, int BranchID, int UserID, string InvoiceNo, string SupplierInvoiceID, float Amount, string SupplierID, string SupplierName, bool ispayment)
        {
            try
            {

                //ep.Clear();
                dtEntries = null;



                //string invoiceno = "PUR" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                string pruchasetitle = "Purchase From " + SupplierName;
                float totalpurchaseamount = 0;

                var financialyearcheck = DatabaseQuery.Retrive("select top 1 FinancialYearID from tblFinancialYear where IsActive = 1");
                //float.TryParse(lblReamingPayment.Text.Trim(), out totalpurchaseamount);
                string FinancialYearID = (financialyearcheck != null ? Convert.ToString(financialyearcheck.Rows[0][0]) : string.Empty);
                if (string.IsNullOrEmpty(FinancialYearID))
                {

                    return "Invalid Financial Year";
                }



                string successmessage = "Purchase Success";




                string AccountHeadID = string.Empty;
                string AccountControlID = string.Empty;
                string AccountSubControlID = string.Empty;

                // Assests 1      increae(Debit)   decrese(Credit)
                // Liabilities 2     increae(Credit)   decrese(Debit)
                // Expenses 3     increae(Debit)   decrese(Credit)
                // Capital 4     increae(Credit)   decrese(Debit)
                // Revenue 5     increae(Credit)   decrese(Debit)
                var purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 3 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(purchaseaccount.AccountHeadID);
                AccountControlID = Convert.ToString(purchaseaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(purchaseaccount.AccountSubControlID);


                string transectiontitle = string.Empty;

                transectiontitle = "Purchase From " + SupplierName.Trim();
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);


                purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 8 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(purchaseaccount.AccountHeadID);
                AccountControlID = Convert.ToString(purchaseaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(purchaseaccount.AccountSubControlID);

                transectiontitle = SupplierName + " , Purchase Payment is Pending!";
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);








                if (ispayment == true)
                {
                    string payinvoicenno = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

                    purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 8 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                    AccountHeadID = Convert.ToString(purchaseaccount.AccountHeadID);
                    AccountControlID = Convert.ToString(purchaseaccount.AccountControlID);
                    AccountSubControlID = Convert.ToString(purchaseaccount.AccountSubControlID);



                    transectiontitle = "Payement Paid to " + SupplierName;
                    SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);


                    purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 9 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                    AccountHeadID = Convert.ToString(purchaseaccount.AccountHeadID);
                    AccountControlID = Convert.ToString(purchaseaccount.AccountControlID);
                    AccountSubControlID = Convert.ToString(purchaseaccount.AccountSubControlID);



                    transectiontitle = SupplierName + " Payment is Succeed";
                    SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);

                    string paymentquery = string.Format("insert into tblSupplierPayment (SupplierID,SupplierInvoiceID,UserID,InvoiceNo,TotalAmount,PaymentAmount,RemainingBalance,CompanyID,BranchID) " +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    SupplierID, SupplierInvoiceID, UserID, payinvoicenno, Amount, Amount, "0", CompanyID, BranchID);
                    DatabaseQuery.Insert(paymentquery);

                    successmessage = successmessage + " with Payment.";

                }

                foreach (DataRow entryrow in dtEntries.Rows)
                {
                    string entryquery = string.Format("insert into tblTransaction (FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID,InvoiceNo, UserID,Credit,Debit,TransectionDate,TransectionTitle,CompanyID,BranchID) values " +
                 " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                 Convert.ToString(entryrow[0]), Convert.ToString(entryrow[1]), Convert.ToString(entryrow[2]), Convert.ToString(entryrow[3]), Convert.ToString(entryrow[4]), Convert.ToString(entryrow[5]), Convert.ToString(entryrow[6]), Convert.ToString(entryrow[7]), Convert.ToDateTime(Convert.ToString(entryrow[8])), Convert.ToString(entryrow[9]), CompanyID, BranchID);
                    DatabaseQuery.Insert(entryquery);
                }

                return successmessage;

            }
            catch (Exception)
            {

                return "Unexpected Error";
            }

        }

        public string PurchasePayment(int CompanyID, int BranchID, int UserID, string InvoiceNo, string SupplierInvoiceID,float TotalAmount, float Amount, string SupplierID, string SupplierName, float RemainingBalance)
        {
            try
            {

                //ep.Clear();
                dtEntries = null;



                //string invoiceno = "PUR" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                string pruchasetitle = "Purchase From " + SupplierName;
                float totalpurchaseamount = 0;

                var financialyearcheck = DatabaseQuery.Retrive("select top 1 FinancialYearID from tblFinancialYear where IsActive = 1");
                //float.TryParse(lblReamingPayment.Text.Trim(), out totalpurchaseamount);
                string FinancialYearID = (financialyearcheck != null ? Convert.ToString(financialyearcheck.Rows[0][0]) : string.Empty);
                if (string.IsNullOrEmpty(FinancialYearID))
                {

                    return "Invalid Financial Year";
                }



                string successmessage = "Purchase Payment Success";




                string AccountHeadID = string.Empty;
                string AccountControlID = string.Empty;
                string AccountSubControlID = string.Empty;

                // Assests 1      increae(Debit)   decrese(Credit)
                // Liabilities 2     increae(Credit)   decrese(Debit)
                // Expenses 3     increae(Debit)   decrese(Credit)
                // Capital 4     increae(Credit)   decrese(Debit)
                // Revenue 5     increae(Credit)   decrese(Debit)

                var transectiontitle = string.Empty;


               

                var purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 8 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(purchaseaccount.AccountHeadID);
                AccountControlID = Convert.ToString(purchaseaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(purchaseaccount.AccountSubControlID);



                transectiontitle = "Payement Paid to " + SupplierName;
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);


                purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 9 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(purchaseaccount.AccountHeadID);
                AccountControlID = Convert.ToString(purchaseaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(purchaseaccount.AccountSubControlID);



                transectiontitle = SupplierName + " Payment is Succeed";
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);

                string paymentquery = string.Format("insert into tblSupplierPayment (SupplierID,SupplierInvoiceID,UserID,InvoiceNo,TotalAmount,PaymentAmount,RemainingBalance,CompanyID,BranchID) " +
                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                SupplierID, SupplierInvoiceID, UserID, InvoiceNo, TotalAmount, Amount, Convert.ToString(RemainingBalance), CompanyID, BranchID);
                DatabaseQuery.Insert(paymentquery);




                foreach (DataRow entryrow in dtEntries.Rows)
                {
                    string entryquery = string.Format("insert into tblTransaction (FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID,InvoiceNo, UserID,Credit,Debit,TransectionDate,TransectionTitle,CompanyID,BranchID) values " +
                 " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                 Convert.ToString(entryrow[0]), Convert.ToString(entryrow[1]), Convert.ToString(entryrow[2]), Convert.ToString(entryrow[3]), Convert.ToString(entryrow[4]), Convert.ToString(entryrow[5]), Convert.ToString(entryrow[6]), Convert.ToString(entryrow[7]), Convert.ToDateTime(Convert.ToString(entryrow[8])), Convert.ToString(entryrow[9]), CompanyID, BranchID);
                    DatabaseQuery.Insert(entryquery);
                }

                return "Payment Paid";

            }
            catch (Exception)
            {

                return "Unexpected Error";
            }




        }






        private void SetEntries(
       string FinancialYearID,
       string AccountHeadID,
       string AccountControlID,
       string AccountSubControlID,
       string InvoiceNo,
       string UserID,
       string Credit,
       string Debit,
       DateTime TransactionDate,
       string TransectionTitle)
        {
            if (dtEntries == null)
            {
                dtEntries = new DataTable();
                dtEntries.Columns.Add("FinancialYearID");
                dtEntries.Columns.Add("AccountHeadID");
                dtEntries.Columns.Add("AccountControlID");
                dtEntries.Columns.Add("AccountSubControlID");
                dtEntries.Columns.Add("InvoiceNo");
                dtEntries.Columns.Add("UserID");
                dtEntries.Columns.Add("Credit");
                dtEntries.Columns.Add("Debit");
                dtEntries.Columns.Add("TransactionDate");
                dtEntries.Columns.Add("TransectionTitle");
            }

            if (dtEntries != null)
            {

                dtEntries.Rows.Add(
                FinancialYearID,
                AccountHeadID,
                AccountControlID,
                AccountSubControlID,
                InvoiceNo,
                UserID,
                Credit,
                Debit,
                TransactionDate,
                TransectionTitle);


            }



        }

    }
}
