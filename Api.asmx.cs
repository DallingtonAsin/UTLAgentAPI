using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using TransAPI.BusinessLogic;
using System.IO;
using System.Net.Mail;

namespace TransAPI
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Api : System.Web.Services.WebService
    {

        

        [WebMethod]
        public object[] deposit(string acc_no, double amt)
        {
            try
            {
                
                BLogic logic = new BLogic();
                string action = "deposit";
                object[] res = logic.transact(action,acc_no, amt);
                return res;
            }catch(Exception ex)
            {
                throw ex;
            }
        }


        [WebMethod]
        public object[] withdraw(string acc_no, double amt)
        {
            try
            {

                BLogic logic = new BLogic();
                string action = "withdraw";
                object[] res = logic.transact(action,acc_no, amt);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [WebMethod]
        public object[] register(object[] customerData)
        {
            try
            {

                BLogic logic = new BLogic();
                object[] res = logic.registerCustomer(customerData);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataTable getCustomersList()
        {
            DataTable dt = null;
            try
            {
             
                BLogic logic = new BLogic();
                dt = logic.getCustomers();
                return dt;


            }catch(Exception ex)
            {
                throw ex;
                return null;
            }
        }


        [WebMethod]
        public string[] getListofAccounts()
        {
           
            try
            {

                BLogic logic = new BLogic();
                string[] data = logic.AvailableAccountNumbers();
                return data;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [WebMethod]
       public bool IsValidAccount(string account_number)
        {
            try
            {
                BLogic logic = new BLogic();
                bool isValid = logic.ValidateCustomerAccount(account_number);
                return isValid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [WebMethod]

        public DataTable getLedger(string account_no, string which_ledger)
        {

            DataTable dt = null;
            try
            {

                BLogic logic = new BLogic();
                dt = logic.getLedger(account_no, which_ledger);
                return dt;


            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }


        }

        [WebMethod]
        public double getCustomerAccountBalance(string account_number)
        {
            try
            {

                BLogic logic = new BLogic();
                double balance = logic.getUserBalance(account_number);
                return balance;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [WebMethod]
        public DataTable ListofLogs(string type)
        {
            DataTable dt = null;
            try
            {

                BLogic logic = new BLogic();
                dt = logic.getLogs(type);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [WebMethod]
        public void SendMail(object[] paras)
        {
            string from = "asingwire50dallington@gmail.com";
            string to = paras[0].ToString();
            string subject = paras[1].ToString();
            string message = paras[2].ToString();
            BLogic logic = null;
            try
            {
                logic = new BLogic();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.UseDefaultCredentials = false;
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("asingwire50dallington@gmail.com", "cmwtmaxirxfhoicd");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {

                logic.Log("error", ex.ToString(), from);
                Console.WriteLine(ex.ToString());
            }

        }




        [WebMethod]

        public object[] GetCustomerData(string account_no)
        {

            
            try
            {

                BLogic logic = new BLogic();
                object[] data = logic.getCustomerDetails(account_no);
                return data;


            }
            catch (Exception ex)
            {
                throw ex;
            }


        }






    }
}
