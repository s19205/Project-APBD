using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdvertApi.DTOs.Requests;
using AdvertApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdvertApi.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IDbService dbService;
        IPasswordService passwordService;
        IConfiguration configuration;

        public AdvertController(IDbService dbService, IPasswordService passwordService, IConfiguration configuration)
        {
            this.dbService = dbService;
            this.passwordService = passwordService;
            this.configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult AddClient(RegistrationRequest request)
        {
            if (dbService.AddClient(request))
                return StatusCode(201);
            else
                return BadRequest("WRONG DATA");
        }


        [AllowAnonymous]
        [HttpPost("refresh-token/{refreshToken}")]
        public IActionResult refreshToken(String refreshToken)
        {
            string login = dbService.GetRefreshTokenOwner(refreshToken);
            if (login == null)
            {
                return BadRequest("Wrong refresh token");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,login),
                new Claim(ClaimTypes.Role,"Client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "Gakko",
                audience: "Client",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!dbService.IsClientExists(request.Login))
            {
                return BadRequest("WRONG PASSWORD OR LOGIN");
            }

            var requestedPasswordsData = dbService.getStudentPasswordData(request.Login);
            if (!passwordService.ValidatePassword(requestedPasswordsData.Password, request.Password, requestedPasswordsData.Value))
            {
                return BadRequest("Wrong password or login");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.Login),
                new Claim(ClaimTypes.Role,"Client")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "Gakko",
                audience: "Student",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );
            var TmpRefreshToken = Guid.NewGuid();
            dbService.SetRefreshToken(request.Login, TmpRefreshToken.ToString());
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refershToken = TmpRefreshToken
            });
        }

    }
}