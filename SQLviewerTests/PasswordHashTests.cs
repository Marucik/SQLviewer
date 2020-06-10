using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLviewer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLviewer.Tests
{
    [TestClass()]
    public class PasswordHashTests
    {
        [TestMethod()]
        public void EncryptTest()
        {
            string password = "t3st3Dp@Ssw0rd";
            string encryptedPassword = PasswordHash.Encrypt(password);
            string expected = PasswordHash.Decrypt(encryptedPassword);

            Assert.AreEqual(expected, password);
            Assert.AreNotEqual(password, encryptedPassword);
        }

        [TestMethod()]
        public void DecryptTest()
        {
            string password = "Str0ngp@ssword";
            string encryptedPassword = PasswordHash.Encrypt(password);
            string expected = PasswordHash.Decrypt(encryptedPassword);

            Assert.AreNotEqual(password, encryptedPassword);
            Assert.AreEqual(expected, password);
        }
    }
}