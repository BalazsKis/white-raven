namespace WhiteRaven.Domain.Models.Authentication
{
    public class Login
    {
        public string Email { get; }
        public string Password { get; }

        public Login(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}