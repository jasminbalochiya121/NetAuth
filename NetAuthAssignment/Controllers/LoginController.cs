using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetAuthAssignment.Models;
using NetAuthAssignment.Options;
using NetAuthAssignment.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace NetAuthAssignment.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IOptions<JwtOptions> _options;
        private readonly IAuthService _authSeService;
        public LoginController(IOptions<JwtOptions> options, IAuthService authSeervice)
        {
            _options = options;
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var Sectoken = new JwtSecurityToken(_options.Value.Issuer,
              _options.Value.Issuer,
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return Ok(token);
        }
    }
}
