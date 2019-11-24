using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Interfaces.Services;
using HotelShare.Interfaces.Web.Settings;
using HotelShare.Web.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Helpers;

namespace HotelShare.Web.Controllers.API
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenFactory _tokenFactory;

        public AuthController(IUserService userService,
            ITokenFactory tokenFactory)
        {
            _userService = userService;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("login")]
        public IActionResult Token([FromForm]LoginViewModel credentials)
        {
            var user = _userService.GetUserByEmail(credentials.Email);

            if (user != null && Crypto.VerifyHashedPassword(user.Password, credentials.Password))
            {
                return Ok(GenerateTokenForUser(user));
            }

            return BadRequest("Invalid credentials.");
        }

        private string GenerateTokenForUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // adding user roles
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name)));

            return _tokenFactory.Create(claims);
        }
    }
}