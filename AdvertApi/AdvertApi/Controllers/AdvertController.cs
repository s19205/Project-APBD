using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdvertApi.DTOs.Requests;
using AdvertApi.DTOs.Responses;
using AdvertApi.Exceptions;
using AdvertApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdvertApi.Controllers
{
    [Route("api/adverts")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IDbService dbService;

        public AdvertController(IDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpPost("clients")]
        [AllowAnonymous]
        public IActionResult AddClient(RegistrationRequest request)
        {
            try {
                RegistrationResponse resp = dbService.AddClient(request);
                return Created("", resp);
            }
            catch (LoginExistException exc)
            {
                return BadRequest(exc);
            }
        }

        [AllowAnonymous]
        [HttpPost("clients/login")]
        public IActionResult LoginClient(LoginRequest request)
        {
            try
            {
                return Ok(dbService.LoginClient(request));
            }
            catch (IncorrectLoginException exc)
            {
                return BadRequest(exc);
            }
            catch (IncorrectPasswordException exc)
            {
                return BadRequest(exc);
            }
        }

        [HttpPost("clients/refresh")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                return Ok(dbService.RefreshToken(request));
            }
            catch (RefreshTokenIncorrectException exc)
            {
                return NotFound(exc);
            }
        }

        [HttpPost("campaigns")]
        [Authorize]
        public IActionResult GetCampaings()
        {
            return Ok(dbService.GetCampaigns());
        }

        [HttpPost("campaigns/create")]
        [Authorize]
        public IActionResult NewCampaign(NewCampaignRequest request)
        {
            try
            {
                return Created("", dbService.NewCampaign(request));
            }
            catch (BuildingsNotExistException exc)
            {
                return NotFound(exc);
            }
            catch (BuildingsNotNearException exc)
            {
                return BadRequest(exc);
            }
        }
    }
}