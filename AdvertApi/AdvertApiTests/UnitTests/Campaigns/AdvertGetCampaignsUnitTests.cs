using AdvertApi.Controllers;
using AdvertApi.DTOs.Responses;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertApiTests.UnitTests.Campaigns
{
    [TestFixture]
    class AdvertGetCampaignsUnitTests
    {
        [Test]
        public void GetCampaigns_CampaingsExists_Correct()
        {
            var dbService = new Mock<IDbService>();
            dbService.Setup(d => d.GetCampaigns()).Returns(new List<CampaignsResponse>()
            {
                new CampaignsResponse{
                    Client = new Client{FirstName="Myname", LastName = "Yourname"},
                    Campaign = new Campaign{StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(15),PricePerSquareMeter=35}
                }
            });
            var controller = new AdvertController(dbService.Object);
            var result = controller.GetCampaings();

            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }
    }
}
