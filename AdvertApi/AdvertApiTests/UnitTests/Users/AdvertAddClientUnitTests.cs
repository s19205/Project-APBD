using AdvertApi.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvertApiTests.UnitTests.Users
{
    [TestFixture]
    class AdvertAddClientUnitTests
    {
        [Test]
        public void AddClient_CompleteRequest_Correct()
        {
            RegistrationRequest request = new RegistrationRequest { 
                FirstName = "Myname", 
                LastName = "Yourname", 
                Email = "Email", 
                Phone = "+48000000000", 
                Login = "Mylogin", 
                Password = "0password1" 
            };
            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();
            var isModelValid = Validator.TryValidateObject(request, context, results, true);

            Assert.IsTrue(isModelValid);
            Assert.IsTrue(results.Count == 0);
        }

        [Test]
        public void AddClient_IncompleteRequest_Incorrect()
        {
            RegistrationRequest req = new RegistrationRequest {
                FirstName = "Myname",
                Phone = "+48000000000",
                Login = "Mylogin",
                Password = "0password1"
            };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsFalse(isModelValid);
            Assert.IsTrue(results.Count == 2);
        }
    }
}
