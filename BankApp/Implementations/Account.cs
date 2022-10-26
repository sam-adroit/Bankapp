using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Implementations
{
    public class Account : IAccount
    {
        public static int Count = 0;
        private readonly IAccountData _accountData;
        
        public  Account(IAccountData accountData)
        {
            _accountData = accountData; 
        }

        public bool CreateAccount(int userId, string accountType, double amount)
        {
            if((accountType == "Savings" && amount < 1000) || amount < 0)
            {
                return false;
            }
            var listTrans = new List<TransactionModel>();
            AccountModel accountModel = new AccountModel() { AccountNo = GenerateAcctNo(), AccountType = accountType, Balance = amount, userId = userId };
            
            var trans = AddTransactions(amount, amount, "First deposit");
            listTrans.Add(trans);
            accountModel.TransactionList = listTrans;
           

            _accountData.InsertAccount(accountModel);
            
            return true;
        }

        private TransactionModel AddTransactions(double balance, double amount, string description)
        {
            TransactionModel transactions = new TransactionModel();
            string date = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
           
            
            transactions.Date = date;
            transactions.Balance = balance;
            transactions.Amount = amount;
            transactions.Description = description;
            
            return transactions;
        }
        public double Balance(int userId, string accountNo)
        {
            var acct = _accountData.GetAccountByUserIdAndAccountNo(userId, accountNo);
            
            if(acct.AccountNo != accountNo)
            {
                throw new UserNotFoundException("User not found");
            }
            
            //if(acct.Balance != null) return acct.Balance;
            return acct.Balance;
        }
        
        public bool Deposit(int userId, string accountNo, double amount)
        {
            var acct = _accountData.GetAccountByUserIdAndAccountNo(userId, accountNo);
            if (acct.AccountNo != accountNo)
            {
                throw new UserNotFoundException("User not found");
            }
            
            acct.Balance += amount;
            var trans = AddTransactions(acct.Balance, amount, "Credit Alert!");
            acct.TransactionList.Add(trans);
            _accountData.UpdateAccount(acct.AccountNo, acct);
            return true;
            
        }

        public bool Withdrawal(int userId, string accountNo, double amount)
        {
            var acct = _accountData.GetAccountByUserIdAndAccountNo(userId, accountNo);

            if (acct.AccountNo != accountNo)
            {
                throw new UserNotFoundException("This Account does not belong to this user!");
            }

            
            if ((acct.Balance - amount < 1000 && acct.AccountType == "Savings") || acct.Balance - amount < 0)
            {
                return false;
            }

            acct.Balance -= amount;
            var trans = AddTransactions(acct.Balance, amount, "Debit Alert!");
            acct.TransactionList.Add(trans);
            _accountData.UpdateAccount(acct.AccountNo, acct);
            return true;
            
            return false;
        }

        public string GenerateAcctNo()
        {
            var acctNo = "0";
            var str = "1234567890";
            var lsAcct = _accountData.GetAllAccountNo();
            Random rd = new Random();
            for (int i = 0; i < 9; i++)
            {
                int rand_num = rd.Next(0, str.Length - 1);
                acctNo += str[rand_num];
            }
            if(lsAcct != null)
            {
                while (lsAcct.Contains(acctNo))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        int rand_num = rd.Next(0, str.Length - 1);
                        acctNo += str[rand_num];
                    }
                }
            }
            
            return acctNo;
        }


        public List<TransactionModel> GetAllTransactions(int userId, string accountNo)
        {
            var acct = _accountData.GetAccountByUserIdAndAccountNo(userId, accountNo);
            if(acct.AccountNo != accountNo)
            {
                throw new UserNotFoundException("User can't access transactions from this account");
            }
            
            return acct.TransactionList;
            
        }
        public bool Transfer(int userId, double amount, string senderAcct, string destinationAcct)
        {
            var senderAcc = _accountData.GetAccountByUserIdAndAccountNo(userId, senderAcct);
            var destAcc = _accountData.GetAccountByAccountNo(destinationAcct);
            if(senderAcc.AccountNo != senderAcct)
            {
                throw new UserNotFoundException("User can't transfer from this account");
            }
            if (destAcc.AccountNo != destinationAcct)
            {
                throw new UserNotFoundException("Account does not exist");
            }
            
            if(senderAcc.Balance < amount || (senderAcc.Balance - amount < 1000 && senderAcc.AccountType == "Savings")) return false;
            senderAcc.Balance -= amount;
            destAcc.Balance += amount;

            var trans = AddTransactions(senderAcc.Balance, amount, "Transfer of "+amount+" to "+destinationAcct);
            senderAcc.TransactionList.Add(trans);
            _accountData.UpdateAccount(senderAcc.AccountNo, senderAcc);

            var trans2 = AddTransactions(destAcc.Balance, amount, "Transfer from "+ senderAcct);
            destAcc.TransactionList.Add(trans2);
            _accountData.UpdateAccount(destAcc.AccountNo, destAcc);
            return true;
        }
        
    }
}
