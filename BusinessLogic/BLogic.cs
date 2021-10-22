using System;
using System.Data;
using TransAPI.DataLogic;
using TransAPI.EntityObjects;
using System.Linq;


namespace TransAPI.BusinessLogic
{
    public class BLogic
    {

        private static Random random = new Random();
        private static Random rand = new Random();

        public static string number_format(double number)
        {
            string formated_number = string.Format("{0:n}", number);
            return formated_number;
        }

            
        public static int RandomNumber(int min, int max)
        {
            return rand.Next(min, max);
        }

        public string generateAccountNumber()
        {
            string acc_no = "AC"+RandomNumber(10000000,90000000)+"";
            return acc_no;

        }

        public object[] transact(string action,string account_no, double amount)
        {
            string message = null;
            bool isExecuted = false;
            string transRef = null;
            try
            {
                transRef = generateTransactionRef();
                DBConnection conn = new DBConnection();
                string bank_name = "PegBank";


                if (action.Equals("deposit"))
                {
               
                        double balance = getUserBalance(account_no);
                        double deposited_amount = amount;
                        double withdrawn_amt = 0;
                        double new_balance = balance + deposited_amount;


                        double bank_balance_x1 = getBankValue(bank_name) - deposited_amount;
                        object[] bank_paras_x1 = { bank_name, bank_balance_x1 };

                        string type = "deposit";
                        string narration = "Deposit of " + number_format(deposited_amount) + " for account " + account_no + " made successfully";
                        string narration2 = "Debt bank account amount worth " + number_format(amount) + " ";
                        object[] deposit_paras = { transRef, account_no, type, narration, deposited_amount, withdrawn_amt, new_balance };
                        object[] bank_paras = { transRef, getBankId(bank_name), type, narration2, 0, amount, bank_balance_x1 };

                        int deposit_result = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.transact, deposit_paras));
                        int bank_transact = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.transact, bank_paras));
                        int AlterBankValueResult = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.alter_bank_value, bank_paras_x1));

                        if (deposit_result == 0 || AlterBankValueResult == 0 || bank_transact == 0)
                        {
                            message = "Transaction of depositing amount " + number_format(amount) + " from " + account_no + " failed!";
                            isExecuted = false;
                            Log("error", message, account_no);

                        }
                        else
                        {
                            message = "You have successfully deposited " + number_format(amount) + " to account number " + account_no + "";
                            isExecuted = true;
                            Log("success", message, account_no);

                        }

                }
              
                    if (action.Equals("withdraw"))
                    {


                    double balance = getUserBalance(account_no);

                    if (balance > amount)
                    {
                        double deposited_amount = 0;
                        double withdrawn_amt = amount;
                        double new_balance = balance - withdrawn_amt;
                        double bank_balance_x2 = getBankValue(bank_name) + withdrawn_amt;
                        object[] bank_paras_x2 = { bank_name, bank_balance_x2 };

                        string narration = "Withdraw of " + number_format(withdrawn_amt) + " from account " + account_no + " made successfully";
                        string narration2 = "Credit bank account amount worth " + number_format(amount) + "";
                        object[] withdraw_paras = { transRef, account_no, action, narration, deposited_amount, withdrawn_amt, new_balance };
                        object[] bank_paras = { transRef, getBankId(bank_name),action, narration2, amount, 0, bank_balance_x2 };

                        int withdraw_result = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.transact, withdraw_paras));
                        int bank_transact1 = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.transact, bank_paras));
                        int AlterBankValueResultX2 = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.alter_bank_value, bank_paras_x2));
                        if (withdraw_result == 0 || AlterBankValueResultX2 == 0 || bank_transact1==0)
                        {
                            message = "Transaction of withdrawing amount "+number_format(amount)+" from "+account_no+" failed!";
                            isExecuted = false;
                            Log("error", message, account_no);


                        }
                        else
                        {
                            message = "You have successfully withdrawn " + number_format(amount) + " from account " + account_no + "";
                            isExecuted = true;
                            Log("success", message, account_no);


                        }

                    }
                    else
                    {
                        message = "Sorry,you have insufficient funds on your account!";
                        isExecuted = false;
                        Log("error", message, account_no);

                    }
                }
                 

            }
            catch(Exception exception)
            {
                message = "exception";
                isExecuted = false;
                Log("error", exception.ToString(), account_no);
                throw exception;
               
            }

            object[] ExecutionResults = { isExecuted, message, transRef };
            return ExecutionResults;


        }


        public bool ValidateCustomerAccount(string account_no)
        {
            bool isValid = false;
            string[] AccountNumbersArr = AvailableAccountNumbers();
            if (AccountNumbersArr.Contains(account_no)){
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;

        }
        

        public double getUserBalance(string acc_no)
        {
            DBConnection conn = null;
            DataTable dt = null;
            double balance = 0.0;
            try
            {
                conn = new DBConnection();
                object[] paras = { acc_no };
                
                dt = conn.retrieveData(FindProcedure.getBalance,paras);
                if (!dt.Rows[0][0].ToString().Equals(""))
                {
                    string bal = dt.Rows[0][0].ToString();
                    balance = Convert.ToDouble(bal);
                }
                else
                {
                    balance = 0.0;

                }
               
                
                return balance;
            }
            catch(Exception e)
            {

                throw e;
             

            }
        
        }


        public double getBankValue(string bank_name)
        {
            DBConnection conn = null;
            DataTable dt = null;
            double bankValue= 0.0;
            try
            {
                conn = new DBConnection();
                object[] paras = { bank_name };

                dt = conn.retrieveData(FindProcedure.bank_value, paras);
                if (!dt.Rows[0][0].ToString().Equals(""))
                {
                    string bal = dt.Rows[0][0].ToString();
                    bankValue = Convert.ToDouble(bal);
                }
                else
                {
                    bankValue = 0.0;

                }


                return bankValue;
            }
            catch (Exception e)
            {

                throw e;


            }

        }


        public string getBankId(string bank_name)
        {
            DBConnection conn = null;
            DataTable dt = null;
            string bankId = null;
            try
            {
                conn = new DBConnection();
                object[] paras = { bank_name };

                dt = conn.retrieveData(FindProcedure.getBankId, paras);
                if (!dt.Rows[0][0].ToString().Equals(""))
                {
                    bankId = dt.Rows[0][0].ToString();
                }
                else
                {
                    bankId = null;

                }


                return bankId;
            }
            catch (Exception e)
            {

                throw e;


            }

        }

        public object[] getCustomerDetails(string account_no)
        {
            DBConnection conn = null;
            DataTable dt = null;
            try
            {
                conn = new DBConnection();
                object[] paras = { account_no };
                object[] EmptyCustomerData = {null, null, null, null };

                dt = conn.retrieveData(FindProcedure.customer_details, paras);
                if (dt.Rows.Count > 0)
                {
                    string fname = dt.Rows[0][1].ToString();
                    string lname = dt.Rows[0][2].ToString();
                    string tel_no = dt.Rows[0][3].ToString();
                    string email = dt.Rows[0][4].ToString();
                    object[] customerData = { fname, lname, tel_no, email};
                    return customerData;
                }
                else
                {

                    return EmptyCustomerData;

                }

                

            }
            catch (Exception e)
            {
                Log("error", e.ToString(), "system");
                throw e;


            }
        

        }



        public object[] registerCustomer(object[] customer)
        {
            string message, account_number = null;
            DBConnection conn = null;
            BLogic logic = null;
            string user = "Dallington";
            try
            {
                Array.Resize(ref customer, 5);
                logic = new BLogic();
                customer[4] = account_number = logic.generateAccountNumber();
                conn = new DBConnection();
                int res = Convert.ToInt32(conn.ExecuteDataSet(FindProcedure.registerCustomer, customer));
                if (res == 0)
                {
                    message = "Insertion of customer details failed!";
                    object[] ResponseData = {message, null};
                    Log("error", message, user);
                    return ResponseData;

                }
                else
                {
                    message = "Customer "+customer[0]+" "+customer[1]+" registered successfully";
                    object[] ResponseData = { message, account_number };
                    Log("success", message, user);
                    return ResponseData;

                }
              

            }
            catch(Exception ex)
            {
                throw ex;
                return null;
            }

        }


        public void Log(string log_type, string action, string user)
        {
            string message = null;
            DBConnection conn = null;
            BLogic logic = null;

            try
            {
              
                conn = new DBConnection();
                object[] paras = { action, user };
                if (log_type == "error")
                { 
                    conn.ExecuteDataSet(FindProcedure.error_log,paras);
                }
                else if (log_type == "success")
                {
                    conn.ExecuteDataSet(FindProcedure.success_log, paras);

                }
            }
            catch (Exception ex)
            {
                Log("error", ex.ToString(),"Dallington");
                throw ex;
            }

        }


        public static string generateTransactionRef()
        {
            Random random = new Random();
            int _8digit_number = random.Next(10000000, 90000000);
            string trans_ref = "TA" + _8digit_number + "";
            return trans_ref;
        }



        public DataTable getCustomers()
        {
            DBConnection conn = new DBConnection();
            DataTable dt = null;
            try
            {
                object[] paras = { };
                dt = conn.retrieveData(FindProcedure.retrieveCustomers, paras);
                return dt;


            }catch(Exception ex)
            {
                throw ex;
            }
        }


        public string[] AvailableAccountNumbers()
        {
            DBConnection conn = new DBConnection();
            DataTable dt = null;
            try
            {
                object[] paras = { };
                dt = conn.retrieveData(FindProcedure.getAccountNumbers, paras);
                string[] AccountNumbers = new string[dt.Rows.Count];
                for(int x=0; x<dt.Rows.Count; x++)
                {
                    AccountNumbers[x] = dt.Rows[0]["account_number"].ToString();

                }
                return AccountNumbers;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable getLedger(string account_no, string ledger)
        {
            DataTable dt = null;
            DBConnection conn = null;
            try
            {
                object[] paras_x1 = { };
                object[] paras_x2 = { account_no };

                conn = new DBConnection();
                if (ledger.Equals("depositLedger"))
                {
                    if (string.IsNullOrEmpty(account_no))
                    {
                        
                        dt = conn.retrieveData(FindProcedure.depositLedger, paras_x1);
                    }
                    else
                    {
                       
                        dt = conn.retrieveData(FindProcedure.custom_deposit_ledger, paras_x2);
                    }
                }
                if (ledger.Equals("withdrawLedger"))
                {
                    if (string.IsNullOrEmpty(account_no))
                    {
                  
                        dt = conn.retrieveData(FindProcedure.withdrawLedger, paras_x1);
                    }
                    else
                    {
                       
                        dt = conn.retrieveData(FindProcedure.custom_withdraw_ledger, paras_x2);
                    }
                }

                if (ledger.Equals("AccountStmt"))
                {
                    if (string.IsNullOrEmpty(account_no))
                    {

                        dt = conn.retrieveData(FindProcedure.general_account_stmt, paras_x1);
                    }
                    else
                    {

                        dt = conn.retrieveData(FindProcedure.custom_account_stmt, paras_x2);
                    }
                }

                return dt;

            }catch(Exception e)
            {
                throw e;
                return dt;
            }
        }


        public DataTable getLogs(string log_type)
        {
            DBConnection conn = null;
            DataTable dt = null;
            try
            {
                conn = new DBConnection();
                object[] paras = { };
                if (log_type.Equals("error_logs"))
                { 
               
                        dt = conn.retrieveData(FindProcedure.getErrorLogs,paras);
                    }
                else if(log_type.Equals("success_logs"))
                {
                    dt = conn.retrieveData(FindProcedure.getSuccessLogs, paras);

                }


                return dt;
            }
            catch (Exception e)
            {

                throw e;


            }

        }
















    }
}