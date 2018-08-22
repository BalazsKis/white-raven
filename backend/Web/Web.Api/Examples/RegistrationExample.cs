using Swashbuckle.AspNetCore.Examples;
using System;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Web.Api.Examples
{
    public class RegistrationExample : IExamplesProvider
    {
        public object GetExamples() =>
            new Registration("John", "Smith", "joe-sm@mail.com", new DateTime(1978, 10, 24), "my-password-here");
    }
}