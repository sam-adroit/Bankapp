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
    public class AccountTests
    {
        //
        private IAccount _account;
        private Mock<IAccountData> _accountData;
       
        [SetUp] 
        public void SetUp()
        {
            _accountData = new Mock<IAccountData>();
            
            _account = new Account(_accountData.Object);
        }

        [Test]
         public void Balance_WhenGivenCorrectUserDetails_ReturnsBalance()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(6, "0172787992")).Returns( new AccountModel { AccountNo= "0172787992", AccountType="Savings", Balance=1900.0, userId=6,});
            var result = _account.Balance(6, "0172787992");
           Assert.That(result, Is.EqualTo(1900.0));
        }

        [Test]
        public void Balance_WhenGivenIncorrectCorrectUserDetails_ReturnsException()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(35, "0172787992")).Returns(new AccountModel());
            Assert.That(() => _account.Balance(35, "0172787992"), Throws.Exception.TypeOf<UserNotFoundException>());
        }

        [Test]
        public void Deposit_WhenGivenCorrectUserDetails_ReturnsAddedTrue()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(6, "0172787992")).Returns(new AccountModel { AccountNo = "0172787992", AccountType = "Savings", Balance = 1900.0, userId = 6, TransactionList = new List<TransactionModel>() });
            var result = _account.Deposit(6, "0172787992",1900.0);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Deposit_WhenGivenIncorrectCorrectUserDetails_ReturnsException()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(50, "0172787992")).Returns(new AccountModel());
            Assert.That(() => _account.Deposit(50, "0172787992",300.0), Throws.Exception.TypeOf<UserNotFoundException>());
        }

        

        [Test]
        
        public void Withdrawal_WhenGivenCorrectUserDetailsAndAmountForSavings_ReturnsTrue()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(6, "0172787992")).Returns(new AccountModel { AccountNo = "0172787992", AccountType = "Savings", Balance = 1900.0, userId = 6, TransactionList = new List<TransactionModel>() });
            var result = _account.Withdrawal(6, "0172787992", 500);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Withdrawal_WhenGivenInCorrectUserDetailsAndAmountForSavings_ReturnsTrue()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(6, "0172787992")).Returns(new AccountModel { AccountNo = "0172787992", AccountType = "Savings", Balance = 1900.0, userId = 6, TransactionList = new List<TransactionModel>() });
            var result = _account.Withdrawal(6, "0172787992", 1200);
            Assert.That(result, Is.False);
        }

        [Test]
        public void Withdrawal_WhenGivenCorrectUserDetailsAndAmountForCurrent_ReturnsTrue()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(6, "0172787992")).Returns(new AccountModel { AccountNo = "0172787992", AccountType = "Current", Balance = 1900.0, userId = 6, TransactionList = new List<TransactionModel>() });
            var result = _account.Withdrawal(6, "0172787992", 1500);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Withdrawal_WhenGivenInCorrectUserDetailsAndAmountForCurrent_ReturnsTrue()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(6, "0172787992")).Returns(new AccountModel { AccountNo = "0172787992", AccountType = "Current", Balance = 900.0, userId = 6, TransactionList = new List<TransactionModel>() });
            var result = _account.Withdrawal(6, "0172787992", 1200);
            Assert.That(result, Is.False);
        }


        [Test]
        public void Withdrawal_WhenGivenIncorrectCorrectUserDetails_ReturnsException()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(50, "0172787992")).Returns(new AccountModel());
            Assert.That(() => _account.Deposit(50, "0172787992", 300.0), Throws.Exception.TypeOf<UserNotFoundException>());
        }


        //Transfer Test
        [Test]
        [TestCase(1,"0123456789","0234567891",500.0)]
        [TestCase(2,"0123456789","0234567891",300.0)]
        public void Transfer_WhenTransferIsValidSavings_ReturnsTrue(int userId, string sender, string reciever, double amount)
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(userId, sender)).Returns(new AccountModel { AccountNo = sender, AccountType="Savings", Balance = 1700.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 1700, Date = "10/02/2020", Description = "Ownabe"} } });
            _accountData.Setup(ad => ad.GetAccountByAccountNo(reciever)).Returns(new AccountModel { AccountNo = reciever, AccountType="Savings", Balance = 1700.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 1700, Date = "10/02/2020", Description = "Ownabe"} } });
            var result = _account.Transfer(userId, amount, sender, reciever);

            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(1, "0123456789", "0234567891", 800.0)]
        [TestCase(2, "0123456789", "0234567891", 1800.0)]
        public void Transfer_WhenTransferAmountIsInvalidSavings_ReturnsFalse(int userId, string sender, string reciever, double amount)
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(userId, sender)).Returns(new AccountModel { AccountNo = sender, AccountType = "Savings", Balance = 1700.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 1700, Date = "10/02/2020", Description = "Ownabe" } } });
            _accountData.Setup(ad => ad.GetAccountByAccountNo(reciever)).Returns(new AccountModel { AccountNo = reciever, AccountType = "Savings", Balance = 1700.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 1700, Date = "10/02/2020", Description = "Ownabe" } } });
            var result = _account.Transfer(userId, amount, sender, reciever);

            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase(1, "0123456789", "0234567891", 500.0)]
        [TestCase(2, "0123456789", "0234567891", 300.0)]
        public void Transfer_WhenTransferIsValidCurrent_ReturnsTrue(int userId, string sender, string reciever, double amount)
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(userId, sender)).Returns(new AccountModel { AccountNo = sender, AccountType = "Current", Balance = 1700.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 500.0, Date = "10/02/2020", Description = "Ownabe" } } });
            _accountData.Setup(ad => ad.GetAccountByAccountNo(reciever)).Returns(new AccountModel { AccountNo = reciever, AccountType = "Current", Balance = 1700.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 1700, Date = "10/02/2020", Description = "Ownabe" } } });
            var result = _account.Transfer(userId, amount, sender, reciever);

            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(1, "0123456789", "0234567891", 500.0)]
        [TestCase(2, "0123456789", "0234567891", 300.0)]
        public void Transfer_WhenTransferAmountIsInvalidCurrent_ReturnsFalse(int userId, string sender, string reciever, double amount)
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(userId, sender)).Returns(new AccountModel { AccountNo = sender, AccountType = "Current", Balance = 0.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 400, Date = "10/02/2020", Description = "Ownabe" } } });
            _accountData.Setup(ad => ad.GetAccountByAccountNo(reciever)).Returns(new AccountModel { AccountNo = reciever, AccountType = "Current", Balance = 250.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 0, Date = "10/02/2020", Description = "Ownabe" } } });
            var result = _account.Transfer(userId, amount, sender, reciever);

            Assert.That(result, Is.False);
        }

        [Test]
        [TestCase(1, "0123456789", "0234567891", 500.0)]
        [TestCase(2, "0123456789", "0234567891", 300.0)]
        public void Transfer_TransferFromOrToInvalidAccount_ReturnsUserNotFoundException(int userId, string sender, string reciever, double amount)
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(userId, sender)).Returns(new AccountModel { AccountNo = "0345678910", AccountType = "Current", Balance = 5000, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 400, Date = "10/02/2020", Description = "Ownabe" } } });
            _accountData.Setup(ad => ad.GetAccountByAccountNo(reciever)).Returns(new AccountModel { AccountNo = "0234517893", AccountType = "Current", Balance = 2550.0, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = 200, Balance = 0, Date = "10/02/2020", Description = "Ownabe" } } });
           

            Assert.That(() => _account.Transfer(userId, amount, sender, reciever), Throws.Exception.TypeOf<UserNotFoundException>());
        }

        //Get all AccountTransactions
        [Test]
        public void GetAllTransactions_WhenUserAccountDoesNotExist_ReturnUserNotFoundException()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(1, "0123456789")).Returns(new AccountModel());
            Assert.That(() => _account.GetAllTransactions(1, "0123456789"), Throws.Exception.TypeOf<UserNotFoundException>());
        }

        [Test]
        public void GetAllTransactions_WhenUserAccountIsValid_ReturnUserNotFoundException()
        {
            _accountData.Setup(ad => ad.GetAccountByUserIdAndAccountNo(1, "0123456789")).Returns(new AccountModel { AccountNo = "0123456789", TransactionList = new List<TransactionModel> { new TransactionModel {Balance = 200.0, Amount=10, Date="11/05/2020", Description="Good pay" } } });
            var result = _account.GetAllTransactions(1, "0123456789");
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));
        }

        //Generate Account Number Test
        [Test]
        public void GenerateAcctNo_WhenCalled_ReturnsLengthAndFirstIndexOfAccount()
        {
            var result = _account.GenerateAcctNo();
            Assert.That(result.Length, Is.EqualTo(10));
            Assert.That(result[0], Is.EqualTo('0'));
        }

        // Create Account Test
        [Test]
        [TestCase(1,"Savings",3000.0)]
        [TestCase(1,"Current",0.0)]
        public void CreateAccount_WhenCalledAmountIsValid_ReturnsTrue(int userId, string accountType, double balance)
        {
            _accountData.Setup(ad => ad.InsertAccount(new AccountModel { AccountNo = "0123456789", userId= userId, AccountType = accountType, Balance = balance, TransactionList = new List<TransactionModel> { new TransactionModel { Amount = balance, Balance = balance, Date = "10/02/2003", Description = "First Deposit" } } })).Returns(true);
            var result = _account.CreateAccount(userId, accountType, balance);
            
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(1, "Savings", 200.0)]
        [TestCase(1, "Current", -20.0)]
        public void CreateAccount_WhenCalledAmountIsInValid_ReturnsTrue(int userId, string accountType, double balance)
        {
            var result = _account.CreateAccount(userId, accountType, balance);
            Assert.That(result, Is.False);
        }

    }
}
