using AdvertApi.Controllers;
using AdvertApi.DTOs.Requests;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertApiTests.IntegrationTests.Users
{
    [TestFixture]
    class AdvertLoginClientIntegrationTests
    {
        [Test]
        public void Login_ExistLogin_Correct()
        {
            var request = new LoginRequest
            {
                Login = "user",
                Password = "password"
            };
            var controller = new AdvertController(new SqlServerDbService(new s19205Context(), new ConfigurationBuilder().Build()));
            var result = controller.LoginClient(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }
    }
}
