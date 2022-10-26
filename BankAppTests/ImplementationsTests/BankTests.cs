using BankApp.Data;
using BankApp.Implementations;
using BankApp.Interfaces;
using BankApp.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAppTests.ImplementationsTests
{
    [TestFixture]
    public class BankTests
    {
        private IBank _bank;
        private Mock<ICustomerData> _customersData;
        [SetUp]
        public void SetUp()
        {
            _customersData = new Mock<ICustomerData>();
            _bank = new Bank(_customersData.Object);
        }
        [Test]
        public void Login_WhenLoginDetailsIsIncorrect_ReturnNull()
        {
            _customersData.Setup(ad => ad.GetCustomerByEmail("samuel@mail.com")).Returns( new CustomerModel { Email = "samuel@mail.com", Name = "Samuel Banks", Password = "Sam123!@", UserId = 1 });
            var result = _bank.Login("samuel@mail.com", "abcdefg");
            Assert.IsNull(result);
        }
        [Test]

        public void Login_WhenLoginDetailsCorrect_ReturnNull()
        {
            _customersData.Setup(ad => ad.GetCustomerByEmail("samuel@mail.com")).Returns(new CustomerModel { Email = "samuel@mail.com", Name = "Samuel Banks", Password = "Sam123!@", UserId = 1 });
            var result = _bank.Login("samuel@mail.com", "Sam123!@");
            Assert.That(result, Is.TypeOf(typeof(CustomerModel)));
        }
    }
}
