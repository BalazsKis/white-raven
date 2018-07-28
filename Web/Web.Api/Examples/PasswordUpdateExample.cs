using Swashbuckle.AspNetCore.Examples;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Web.Api.Examples
{
    public class PasswordUpdateExample : IExamplesProvider
    {
        public object GetExamples() =>
            new PasswordUpdate("my-OLD-password-here", "my-NEW-password-here");
    }
}
