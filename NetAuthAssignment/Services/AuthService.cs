using Microsoft.Extensions.Options;
using NetAuthAssignment.Options;

namespace NetAuthAssignment.Services
{
    public interface IAuthService
    {
        public bool AuthorizeUser(string username, string password);
    }
    public class AuthService: IAuthService
    {
        private readonly IOptions<AuthOptions> _options;
        public AuthService(IOptions<AuthOptions> options)
        {
            _options = options;
        }
        public bool AuthorizeUser(string username, string password)
        {
            if(_options.Value.UserName == username && _options.Value.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
