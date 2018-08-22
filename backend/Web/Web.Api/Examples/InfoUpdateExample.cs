using Swashbuckle.AspNetCore.Examples;
using System;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Web.Api.Examples
{
    public class InfoUpdateExample : IExamplesProvider
    {
        public object GetExamples() =>
            new InfoUpdate("John", "Smith", new DateTime(1978, 10, 24));
    }
}