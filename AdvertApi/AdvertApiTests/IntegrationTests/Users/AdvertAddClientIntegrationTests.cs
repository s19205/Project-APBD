using AdvertApi.Controllers;
using AdvertApi.DTOs.Requests;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvertApiTests.IntegrationTests.Users
{
    [TestFixture]
    class AdvertAddClientIntegrationTests
    {
        [Test]
        public void AddClient_CompleteRequest_Correct()
        {
            RegistrationRequest request = new RegistrationRequest
            {
                FirstName = "Myname",
                LastName = "Yourname",
                Email = "Email",
                Phone = "+48000000000",
                Login = "Mylogin",
                Password = "0password1"
            };
            var controller = new AdvertController(new SqlServerDbService(new s19205Context(), new ConfigurationBuilder().Build()));
            var result = controller.AddClient(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is CreatedResult);
        }

        [Test]
        public void AddClient_IncompleteRequest_Incorrect()
        {
            RegistrationRequest req = new RegistrationRequest
            {
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
