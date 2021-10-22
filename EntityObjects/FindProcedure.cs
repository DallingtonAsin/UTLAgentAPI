using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TransAPI.EntityObjects
{
    public static class FindProcedure
    {

        public static string depositMoney = "depositMoney";
        public static string custom_deposit_ledger = "CustomDepositLedger";
        public static string custom_withdraw_ledger = "CustomWithdrawLedger";
        public static string withdrawMoney = "withdrawMoney";
        public static string registerCustomer = "registerCustomer";
        public static string retrieveCustomers = "getCustomers";
        public static string depositLedger = "depositLedger";
        public static string withdrawLedger = "withdrawLedger";
        public static string transact = "transact";
        public static string getBalance = "getUserAccountBalance";
        public static string general_account_stmt = "GeneralAccountStmt";
        public static string custom_account_stmt = "CustomAccountStmt";
        public static string bank_value = "GetCurrentBankValue";
        public static string alter_bank_value = "AlterCorporateAccount";
        public static string getBankId = "getBankId";
        public static string success_log = "storeTransactionalLogs";
        public static string error_log = "storeErrorLogs";
        public static string getErrorLogs = "getErrorLogs";
        public static string getSuccessLogs = "getSuccessLogs";
        public static string customer_details = "getCustomerDetails";
        public static string getAccountNumbers = "getAvailableAccountNumbers";



    }
}