using BankApp.Data;
using BankApp.Implementations;
using BankApp.Interfaces;
using BankApp.Models;
using BankAppWinForm;
using Moq;
using System;


namespace BankAppTests.ImplementationsTests
{
    [TestFixture]
    public class CustomerTests
    {
       
        private Mock<IAccountData> _accountData;
        private Customer _customer;
        [SetUp]
        /*
        public void SetUp()
        {
            _accountData = new Mock<IAccountData>();
            
            _customer = new Customer(_accountData.Object);
        }
        */
        /*
        [Test] 
        public void GetAllCustomerAccounts_WhenUserIdDoesNotExist_ReturnZero()
        {
            _accountData.Setup(ad => ad.GetAccountsByUserId(55)).Returns(new List<AccountModel>());
            var result = _customer.GetAllCustomerAccounts(50, "Samuel");
            Assert.That(result.Count, Is.EqualTo(0));
        }
        */
        [Test]
        public void GetAllCustomerAccounts_WhenUserIdExist_ReturnTrue()
        {
            _accountData = new Mock<IAccountData>();

            _customer = new Customer(_accountData.Object);
            _accountData.Setup(ad => ad.GetAccountsByUserId(1)).Returns(new List<AccountModel> { new AccountModel { AccountNo = "0123456789", AccountType = "Savings", Balance = 3000.0, userId = 7, TransactionList = new List<TransactionModel> { new TransactionModel { Balance = 5000, Amount = 500, Date = "10/2/2020", Description = "Good" } } } });
            var result = _customer.GetAllCustomerAccounts(7, "Samuel");
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));
        }
        
    }
}
