using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Helpers;
using HotelShare.Domain.Models.SqlModels.AccountModels;
using HotelShare.Interfaces.Services;
using HotelShare.Interfaces.Web.Settings;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("login/{email}/{password}")]
        public IActionResult Token(string email, string password)
        {
            var user = _userService.GetUserByEmail(email);

            if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
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