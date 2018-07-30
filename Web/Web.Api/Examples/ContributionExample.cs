using Swashbuckle.AspNetCore.Examples;
using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Web.Api.Examples
{
    public class ContributionExample : IExamplesProvider
    {
        public object GetExamples() =>
            new Contribution("joe-sm@mail.com", "256562596b8e43239ca5311aaf40cfcc", ContributionType.Writer);
    }
}