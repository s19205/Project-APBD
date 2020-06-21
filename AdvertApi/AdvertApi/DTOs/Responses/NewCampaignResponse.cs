using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.DTOs.Responses
{
    public class NewCampaignResponse
    {
        public Campaign Campaign { get; set; }
        public Banner Banner1 { get; set; }
        public Banner Banner2 { get; set; }
    }
}
