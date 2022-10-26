using BankApp.Implementations;
using BankApp.Interfaces;
using BankApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class CustomerData : ICustomerData
    {
        public bool AddCustomer(CustomerModel customerModel)
        {
            var getAllCustomer = Task.Run(() => GetAllCustomer()).Result;
            
            

            if (getAllCustomer.Count != 0)
            {
                foreach(var customer in getAllCustomer)
                {
                    if (customer.Email == customerModel.Email) return false;
                } 
            }

            getAllCustomer.Add(customerModel);
            var json = JsonConvert.SerializeObject(getAllCustomer);
            File.WriteAllText("customers.json", json);
            
            return true;
        }

        public List<CustomerModel> GetAllCustomer()
        {
            List<CustomerModel> listOfCustomerModels = new List<CustomerModel>();
            if(File.Exists("customers.json") && new FileInfo("customers.json").Length != 0)
            {
                var customerStrings = File.ReadAllLines("customers.json")[0];
                listOfCustomerModels = JsonConvert.DeserializeObject<List<CustomerModel>>(customerStrings);
                
            }
            return listOfCustomerModels;
        }

        public int LastId()
        {
            if (!File.Exists("customers.json")) return 1;
            var getAllCustomer = GetAllCustomer();
            
            int lastId = 0;
            foreach(var cust in getAllCustomer)
            {
                if(cust.UserId > lastId)
                {
                    lastId = cust.UserId;
                }
            }
            return lastId;
        }

        public  CustomerModel GetCustomerByEmail(string email)
        {
            var getAllCustomer = GetAllCustomer();
            
            var customer = new CustomerModel();
            foreach(var cust in getAllCustomer)
            {
                if(cust.Email == email)
                {
                    customer = cust;
                }
            }
            return customer;
        }

        public CustomerModel GetCustomerById(int id)
        {
            var getAllCustomer = GetAllCustomer();
            var customer = new CustomerModel();
            foreach (var cust in getAllCustomer)
            {
                if(cust.UserId == id)
                {
                    customer = cust;
                }
            }
            return customer;
        }
    }
}
