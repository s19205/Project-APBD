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

namespace AdvertApiTests.IntegrationTests.Campaigns
{
    [TestFixture]
    class AdvertNewCampaignIntegrationTests
    {
        [Test]
        public void AddCampaign_CompleteRequest_Correct()
        {
            NewCampaignRequest request = new NewCampaignRequest
            {
                IdClient = 5,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(15),
                PricePerSquareMeter = 45,
                FromIdBuilding = 2,
                ToIdBuilding = 5
            };
            var controller = new AdvertController(new SqlServerDbService(new s19205Context(), new ConfigurationBuilder().Build()));
            var result = controller.NewCampaign(request);

            Assert.IsNotNull(result);
            Assert.IsTrue(result is CreatedResult);
        }

    }
}
