using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Services
{
    public class SqlServerDbService : IDbService
    {
        s19205Context dbContext;

        public SqlServerDbService(s19205Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AddClient(RegistrationRequest request)
        {
            throw new NotImplementedException();
        }

        public string GetRefreshTokenOwner(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public PasswordResponse getStudentPasswordData(string login)
        {
            throw new NotImplementedException();
        }

        public bool IsClientExists(string login)
        {
            return dbContext.Client.Any() ? dbContext.Client
                    .Where(client => client.Login == login)
                    .FirstOrDefault() == null : false;
        }

        public void SetRefreshToken(string login, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
