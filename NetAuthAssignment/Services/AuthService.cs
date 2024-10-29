namespace NetAuthAssignment.Services
{
    public interface IAuthSeervice
    {
        public bool AuthorizeUser(string username, string password);
    }
    public class AuthService: IAuthSeervice
    {
        public bool AuthorizeUser(string username, string password)
        {
            if(username == "testUser" && password == "Password1")
            {
                return true;
            }
            return false;
        }
    }
}
