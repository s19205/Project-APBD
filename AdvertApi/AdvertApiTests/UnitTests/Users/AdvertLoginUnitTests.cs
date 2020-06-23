using AdvertApi.Controllers;
using AdvertApi.DTOs.Requests;
using AdvertApi.Exceptions;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertApiTests.UnitTests.Users
{
    [TestFixture]
    class AdvertLoginUnitTests
    {
        [Test]
        public void Login_ExistLogin_Correct()
        {
            var request = new LoginRequest
            {
                Login = "user",
                Password = "password"
            };

            var dbService = new Mock<IDbService>();
            dbService.Setup(l => l.LoginClient(request))
                        .Returns(new AdvertApi.DTOs.Responses.LoginResponse
                        {
                            RefreshToken = "refresh",
                            AccessToken = "acces"
                        });
            var controller = new AdvertController(dbService.Object);
            var result = controller.LoginClient(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);

        }

        [Test]
        public void Login_IncorrectLogin_Incorrect()
        {
            var request = new LoginRequest
            {
                Login = "user",
                Password = "password"
            };
            var dbService = new Mock<IDbService>();
            dbService.Setup(l => l.LoginClient(request))
                        .Throws(new IncorrectLoginException());
            var controller = new AdvertController(dbService.Object);
            var result = controller.LoginClient(request);
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult);
            var oR = (ObjectResult)result;
            Assert.IsTrue(oR.StatusCode == 404);
        }

        [Test]
        public void Login_IncorrectPassword_Incorrect()
        {
            var request = new LoginRequest
            {
                Login = "user",
                Password = "password"
            };
            var dbService = new Mock<IDbService>();
            dbService.Setup(l => l.LoginClient(request))
                        .Throws(new IncorrectPasswordException());
            var controller = new AdvertController(dbService.Object);
            var result = controller.LoginClient(request);
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult);
            var oR = (ObjectResult)result;
            Assert.IsTrue(oR.StatusCode == 400);
        }

    }
}
