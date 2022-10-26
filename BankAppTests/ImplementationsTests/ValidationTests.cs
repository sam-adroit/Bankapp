using BankApp.Implementations;
using NUnit.Framework;
using System;


namespace BankAppTests.ImplementationsTests
{
    [TestFixture]
    public class ValidationTests
    {
        private Validation _validate;
        [SetUp]
        public void SetUp()
        {
            _validate = new Validation();
        }
        [Test]
        
        [TestCase("sam@mail.com")]
        [TestCase("sam@decagon.dev")]
        [TestCase("sam@gmail.eu")]
        public void ValidateEmail_WhenEmailValid_ReturnsTrue(string email)
        {
            var result = _validate.ValidateEmail(email);
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("sam@")]
        [TestCase("sam@mail")]
        [TestCase("sam@mail.")]
        [TestCase("sam-ade@g.t")]
        public void ValidateEmail_WhenEmailInvalid_ReturnsTrue(string email)
        {
            var result = _validate.ValidateEmail(email);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateEmail_WhenInputIsNull_ReturnsFalse()
        {
            
            Assert.That(() => _validate.ValidateEmail(null), Throws.ArgumentNullException);
        }

        [Test]

        [TestCase("Sam")]
        [TestCase("Bankole")]
        [TestCase("Adekunle")]
        public void ValidateName_WhenNameIsValid_ReturnsTrue(string name)
        {
            var result = _validate.ValidateName(name);
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("sam")]
        [TestCase("1Sam")]
        [TestCase(" Bansk")]
       
        public void ValidateName_WhenNameInvalid_ReturnsFalse(string name)
        {
            var result = _validate.ValidateName(name);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidateEmail_WhenNameIsNull_ReturnsTrue()
        {

            Assert.That(() => _validate.ValidateName(null), Throws.ArgumentNullException);
        }


        [Test]

        [TestCase("abcde123!")]
        [TestCase("Bankole@34")]
        [TestCase("Adekunle2!")]
        public void ValidatePassword_WhenPasswordIsValid_ReturnsTrue(string pass)
        {
            var result = _validate.ValidatePassword(pass);
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("sam")]
        [TestCase("1Sam")]
        [TestCase(" Bansksyyy")]

        public void ValidatePassword_WhenNPasswordInvalid_ReturnsFalse(string name)
        {
            var result = _validate.ValidatePassword(name);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ValidatePassword_WhenPasswordIsNull_ReturnsTrue()
        {

            Assert.That(() => _validate.ValidatePassword(null), Throws.ArgumentNullException);
        }
    }
}
