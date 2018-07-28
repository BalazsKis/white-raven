using Swashbuckle.AspNetCore.Examples;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Web.Api.Examples
{
    public class LoginExample : IExamplesProvider
    {
        public object GetExamples() =>
            new Login("joe-sm@mail.com", "my-password-here");
    }
}