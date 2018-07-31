using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Examples;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Web.Api.Examples;

namespace WhiteRaven.Web.Api.Controllers
{
    /// <summary>
    /// API controller for login and token generation
    /// </summary>
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserOperations _userOperations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenController"/> class.
        /// </summary>
        /// <param name="config">The global configuration</param>
        /// <param name="userOperations">The user operations</param>
        public TokenController(
            IConfiguration config,
            IUserOperations userOperations)
        {
            _config = config;
            _userOperations = userOperations;
        }

        /// <summary>
        /// Generates a personal access token
        /// </summary>
        /// <param name="login">The user's ID (email address) and password</param>
        /// <returns>The personal access token</returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerRequestExample(typeof(Login), typeof(LoginExample))]
        public async Task<IActionResult> CreateToken([Required, FromBody]Login login)
        {
            var user = await _userOperations.ValidateLogin(login);
            return Ok(new { token = BuildToken(user) });
        }


        private string BuildToken(User user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString("yyyy-MM-dd")),
                new Claim(ClaimTypes.Role, ((int)user.Level).ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                issuer: _config["Jwt:Issuer"],
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}