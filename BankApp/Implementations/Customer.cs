using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;
using BankAppWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Implementations
{
    public class Customer: ICustomer
    {
        private readonly IAccountData _accountData;
        

        public Customer(IAccountData accountData)
        {
            //_accountData = accountData;
            _accountData = new AccountData();
        }
        
        public List<AccountDetailsFormatModel> GetAllCustomerAccounts(int id, string name)
        {
            
            var list = new List<AccountDetailsFormatModel>();
            var allAccount = _accountData.GetAccountsByUserId(id);

            if (allAccount.Count > 0)
            {
                foreach (var account in allAccount)
                {
                    list.Add(new AccountDetailsFormatModel() { Name = name, AccountNo = account.AccountNo, AccountType = account.AccountType, Balance = account.Balance });
                }
            }
            
            return list;
        }
    }
}
