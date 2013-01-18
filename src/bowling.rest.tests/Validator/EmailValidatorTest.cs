using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bowling.Rest.Service.Interface.Validation;

namespace bowling.rest.tests
{
    [TestClass]
    public class EmailValidatorTest
    {
        EmailValidator validator = null;

        [TestInitialize]
        public void TestInit() 
        {
           validator  = new EmailValidator();
        }

        [TestMethod]
        public void ValidateEmailNull()
        {
            Assert.IsFalse(validator.IsValid(null));

        }

        [TestMethod]
        public void ValidateEmailIsEmpty()
        {
            Assert.IsFalse(validator.IsValid(" "));

        }

        [TestMethod]
        public void ValidateEmailOnlyNumber()
        {
            Assert.IsFalse(validator.IsValid("4684698"));
        }

        [TestMethod]
        public void ValidateEmailWithoutAt()
        {
            Assert.IsFalse(validator.IsValid("bibi%bor.dk"));
        }

        [TestMethod]
        public void ValidateEmailNoValidDomain()
        {
            Assert.IsFalse(validator.IsValid("user@kdlsso"));
        }


        [TestMethod]
        public void ValidateEmailNoUsername()
        {
            Assert.IsFalse(validator.IsValid("@dtu.dk"));
        }


        [TestMethod]
        public void ValidateEmailNumberUsername()
        {
            Assert.IsTrue(validator.IsValid("123@domain.gr"));
        }

        [TestMethod]
        public void ValidateEmailWithSpace()
        {
            Assert.IsFalse(validator.IsValid("dane@ _domain.dk"));
        }


        [TestMethod]
        public void ValidateEmailOrdinary()
        {
            Assert.IsTrue(validator.IsValid("mplmpla@domain.gr"));
        }
                

        [TestMethod]
        public void ValidateEmailNumberDomain()
        {
            Assert.IsTrue(validator.IsValid("mplmpla@123.gr"));
        }










    }
}
