using BankApp.Implementations;
using BankApp.Interfaces;
using BankApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class AccountData : IAccountData
    {
        public List<AccountModel> GetAllAccounts()
        {
            
            var listOfAccounts = new List<AccountModel>();
            if(File.Exists("accounts.json"))
            {
                var file = new FileInfo("accounts.json");
                if (file.Length <= 0) return listOfAccounts;
                var jsonStr = File.ReadAllLines("accounts.json")[0];
                listOfAccounts = JsonConvert.DeserializeObject<List<AccountModel>>(jsonStr);

            }
            return listOfAccounts;
        }

        public List<string> GetAllAccountNo()
        {
            var list = new List<string>();
            var listOfAccounts = GetAllAccounts();
            if(listOfAccounts.Count > 0)
            {
                foreach(var account in listOfAccounts)
                {
                    list.Add(account.AccountNo);
                }
                
            }

            return list;
        }
        public AccountModel GetAccountByAccountNo(string accountNo)
        {
            var allAccounts = GetAllAccounts();
            if(allAccounts.Count > 0)
            {
                foreach(var account in allAccounts)
                {
                    if(account.AccountNo == accountNo)
                    {
                        return account;
                    }
                }
            }
            return null;
        }

        public List<AccountModel> GetAccountsByUserId(int id)
        {
            var allAccounts = GetAllAccounts();
            var accountList = new List<AccountModel>();
            if (allAccounts.Count > 0)
            {
                foreach (var account in allAccounts)
                {
                    if (account.userId == id)
                    {
                        accountList.Add(account);
                        
                    }
                }
            }
            return accountList;
        }
        

        public bool InsertAccount(AccountModel accountModel)
        {
            if(accountModel.Balance >= 0 && accountModel.TransactionList.Count > 0 && accountModel.userId >= 1 && accountModel.AccountNo != "" && (accountModel.AccountType == "Savings" || accountModel.AccountType == "Current")) {
                var getAllAccounts = GetAllAccounts();
                getAllAccounts.Add(accountModel);
                var objToJson = JsonConvert.SerializeObject(getAllAccounts);
                File.WriteAllText("accounts.json", objToJson);

                return true;
            }
            
            return false;
        }

        public AccountModel GetAccountByUserIdAndAccountNo(int id, string accountNo)
        {
            var getAllAccountsByUser = GetAccountsByUserId(id);
            var accountModel = new AccountModel();
            if(getAllAccountsByUser.Count > 0)
            {
                foreach(AccountModel account in getAllAccountsByUser)
                {
                    if(account.AccountNo == accountNo)
                    {
                        return account;
                    }
                }
            }
            return accountModel;
        }
        
        public bool UpdateAccount(string accountNo, AccountModel accountModel)
        {
            var getAllAccounts = GetAllAccounts();
            if(getAllAccounts.Count > 0)
            {
                for(int i = 0; i < getAllAccounts.Count; i++)
                {
                    if(getAllAccounts[i].AccountNo == accountNo)
                    {
                        getAllAccounts[i] = accountModel;
                        var objToJson = JsonConvert.SerializeObject(getAllAccounts);
                        File.WriteAllText("accounts.json", objToJson);

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
