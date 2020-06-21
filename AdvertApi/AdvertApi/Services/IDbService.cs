using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public interface IDbService
    {

        bool IsClientExists(string login);
        public RegistrationResponse AddClient(RegistrationRequest request);
        public ICollection<CampaignsResponse> GetCampaigns();
        public LoginResponse LoginClient(LoginRequest request);
        public LoginResponse RefreshToken(RefreshTokenRequest request);
        public NewCampaignResponse NewCampaign(NewCampaignRequest request);

    }
}
