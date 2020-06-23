using AdvertApi.DTOs.Requests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdvertApiTests.UnitTests.Campaigns
{
    [TestFixture]
    class AdvertNewCampaignUnitTests
    {
        [Test]
        public void AddCampaign_CompleteRequest_Correct()
        {
            NewCampaignRequest req = new NewCampaignRequest { 
                IdClient = 5, 
                StartDate = DateTime.Today, 
                EndDate = DateTime.Today.AddDays(15),
                PricePerSquareMeter = 45,
                FromIdBuilding = 2, 
                ToIdBuilding = 5 
            };
            var context = new ValidationContext(req, null, null);
            var results = new List<ValidationResult>();

            var isModelValid = Validator.TryValidateObject(req, context, results, true);

            Assert.IsTrue(isModelValid);
            Assert.IsTrue(results.Count == 0);
        }
    }
}
