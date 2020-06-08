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
        public bool AddClient(RegistrationRequest request);

        PasswordResponse getStudentPasswordData(string login);
        String GetRefreshTokenOwner(String refreshToken);
        void SetRefreshToken(string login, String refreshToken);
    }
}
