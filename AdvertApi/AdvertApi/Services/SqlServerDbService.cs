using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Exceptions;
using AdvertApi.Models;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace AdvertApi.Services
{
    public class SqlServerDbService : IDbService
    {
        private readonly s19205Context dbContext;
        public IConfiguration configuration { get; set; }

        public SqlServerDbService(s19205Context dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public RegistrationResponse AddClient(RegistrationRequest request)
        {
            if (dbContext.Client.Where(c => c.Login.Equals(request.Login)).FirstOrDefault() != null) 
            {
                throw new LoginExistException("This login is already exist");
            }
            Client cl = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Login = request.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.SaltRevision.Revision2)
            };
            dbContext.Client.Add(cl);
            dbContext.SaveChanges();
            RegistrationResponse response = new RegistrationResponse
            {
                FirstName = cl.FirstName,
                LastName = cl.LastName,
                Email = cl.Email,
                Phone = cl.Phone,
                Login = cl.Login
            };
            return response;
        }

        public ICollection<CampaignsResponse> GetCampaigns()
        {
            var camp = dbContext.Campaign
                        .Join(dbContext.Client,
                            cam => cam.IdClient,
                            client => client.IdClient,
                            (cam, client) => new CampaignsResponse
                            {
                                Campaign = cam,
                                Client = client
                            })
                        .OrderByDescending(c => c.Campaign.StartDate)
                        .ToList();
            return camp;
        }

        public bool IsClientExists(string login)
        {
            return dbContext.Client.Any() ? dbContext.Client
                    .Where(client => client.Login == login)
                    .FirstOrDefault() == null : false;
        }

        public LoginResponse LoginClient(LoginRequest request)
        {
            var isUser = dbContext.Client.Where(c => c.Login.Equals(request.Login)).FirstOrDefault();
            if (isUser == null)
            {
                throw new IncorrectLoginException("Incorrect login! Try again!");
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, isUser.Password))
            {
                throw new IncorrectPasswordException("Incorrect password! Try again!");
            }

            Guid refreshToken = new Guid();
            isUser.RefreshToken = refreshToken.ToString();
            dbContext.Update(isUser);

            var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, isUser.Login),
                new Claim(ClaimTypes.Name, isUser.FirstName + " " + isUser.LastName),
                new Claim(ClaimTypes.Role, "client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "s19205",
                audience: "Client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            dbContext.SaveChanges();
            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.ToString()
            };
        }

        public LoginResponse RefreshToken(RefreshTokenRequest request)
        {
            var client = dbContext.Client.Where(c => c.RefreshToken.Equals(request.RefreshToken)).FirstOrDefault();
            if(client == null)
            {
                throw new RefreshTokenIncorrectException("Refresh token incorrect!");
            }

            Guid refreshToken = Guid.NewGuid();
            client.RefreshToken = refreshToken.ToString();
            dbContext.Update(client);

            var claims = new[]
             {
                new Claim(ClaimTypes.NameIdentifier, client.Login),
                new Claim(ClaimTypes.Name, client.FirstName + " " + client.LastName),
                new Claim(ClaimTypes.Role, "client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "s19205",
                audience: "Client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            dbContext.SaveChanges();
            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.ToString()
            };
        }

        public NewCampaignResponse NewCampaign(NewCampaignRequest request)
        {
            if (dbContext.Building.Count() < 2)
            {
                throw new BuildingsNotExistException("No buildings in the database!");
            }

            var building1 = dbContext.Building.Where(b => b.IdBuilding.Equals(request.FromIdBuilding)).FirstOrDefault();
            var building2 = dbContext.Building.Where(b => b.IdBuilding.Equals(request.ToIdBuilding)).FirstOrDefault();
            if (!building1.Street.Equals(building2.Street))
            {
                throw new BuildingsNotNearException("Buildings are not near with each other");
            }

            Campaign camp = new Campaign
            {
                IdClient = request.IdClient,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PricePerSquareMeter = request.PricePerSquareMeter,
                FromIdBuilding = request.FromIdBuilding,
                ToIdBuilding = request.ToIdBuilding
            };
            dbContext.Campaign.Add(camp);
            dbContext.SaveChanges();

            var buildings = dbContext.Building
                        .Where(b => b.StreetNumber >= building1.StreetNumber && b.StreetNumber <= building2.StreetNumber)
                        .OrderBy(a => a.StreetNumber)
                        .ToList();

            List<Banner> banners = calculateSize(buildings, camp, request.PricePerSquareMeter);
            dbContext.Banner.AddRange(banners.First(), banners.Last());
            dbContext.SaveChanges();

            return new NewCampaignResponse
            {
                Campaign = camp,
                Banner1 = banners.First(),
                Banner2 = banners.Last()
            };
        }

        public List<Banner> calculateSize(List<Building> buildings, Campaign camp, decimal price)
        {
            Banner banner1 = new Banner{};
            Banner banner2 = new Banner{};
            for (int i = 1; i < buildings.Count() - 1; i++)
            {
                decimal height1Max = 0, height2Max = 0;
                Banner firstBanner1 = new Banner { Name = 1, IdCampaign = camp.IdCampaign };
                Banner firstBanner2 = new Banner { Name = 2, IdCampaign = camp.IdCampaign };

                for (int j = 0; j < buildings.Count(); j++)
                {
                    var building = buildings.ElementAt(j);
                    if (j < i && building.Height > height1Max) 
                    {
                        height1Max = building.Height;
                    }
                    if (j == i)
                    {
                        firstBanner1.Area = height1Max * i;
                        firstBanner1.Price = firstBanner1.Area * price;
                    }
                    if (j >= i && building.Height > height2Max) 
                    {
                        height2Max = building.Height;
                    }
                    
                }
                firstBanner2.Area = height2Max * (buildings.Count() - i);
                firstBanner2.Price = firstBanner2.Area * price;

                if ((banner1.Area == 0 && banner2.Area == 0) || (firstBanner1.Area + firstBanner2.Area < banner1.Area + banner2.Area))
                {
                    banner1 = firstBanner1;
                    banner2 = firstBanner2;
                }

            }
            return new List<Banner>
            {
                banner1,
                banner2
            };

        }
   
    }
}
