using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetAuthAssignment.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NetAuthAssignment.Auth
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly IOptions<JwtOptions> _options;
        public AuthMiddleware( IOptions<JwtOptions> options)
        {
            _options = options;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string token = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    var claimsPrincipal = ValidateToken(token);
                    context.User = claimsPrincipal;
                }
                catch
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }
            await next(context);
        }
        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false,
                ValidateAudience = false, 
                ValidateIssuer = false,  
                ValidIssuer = _options.Value.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key)) 
            };
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
