using BankApp.Models;
using BankAppWinForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface ICustomer
    {
        List<AccountDetailsFormatModel> GetAllCustomerAccounts(int id, string name);
    }
}
