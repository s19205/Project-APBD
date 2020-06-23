using AdvertApi.Controllers;
using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertApiTests.IntegrationTests.Campaigns
{
    [TestFixture]
    class AdvertGetCampaignsIntegrationTests
    {
        [Test]
        public void GetCampaigns_CampaingsExists_Correct()
        {
            var controller = new AdvertController(new SqlServerDbService(new s19205Context(), new ConfigurationBuilder().Build()));
            var result = controller.GetCampaings();
            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkObjectResult);
        }

    }
}
