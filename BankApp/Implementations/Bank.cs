using BankApp.Data;
using BankApp.Interfaces;
using BankApp.Models;
using System;


namespace BankApp.Implementations
{
    public class Bank : IBank
    {
    
        private readonly ICustomerData _customerData;
       
        private readonly CustomerModel customerModel;

        public Bank(ICustomerData customerData)
        {
            _customerData = customerData;
            
        }
        public CustomerModel? Login(string email, string password)
        {
            var customer = _customerData.GetCustomerByEmail(email);
            
            if (customer.Password == password) return customer;
            return null;
        }

        public bool NewCustomer(int id, string firstname, string lastname, string email, string password)
        {
            var cust = new CustomerModel();
            cust.UserId = id;
            cust.Name = firstname + " "+ lastname;
            cust.Email = email;
            cust.Password = password;
            var added = _customerData.AddCustomer(cust);
            
            if(added) return added;
            return false;
        }
    }
}
