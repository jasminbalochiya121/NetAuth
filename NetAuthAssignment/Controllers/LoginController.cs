using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NetAuthAssignment.Models;
using NetAuthAssignment.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NetAuthAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IAuthSeervice _authSeService;
        public LoginController(IConfiguration config, IAuthSeervice authSeervice)
        {
            _config = config;
            _authSeService = authSeervice;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest loginRequest)
        {
            bool isAuthorized = _authSeService.AuthorizeUser(loginRequest.username, loginRequest.password);
            if (!isAuthorized)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(token);
        }
    }
}
