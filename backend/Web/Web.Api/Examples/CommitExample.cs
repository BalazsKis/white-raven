using Swashbuckle.AspNetCore.Examples;
using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Web.Api.Examples
{
    public class CommitExample : IExamplesProvider
    {
        public object GetExamples() =>
            new Commit("Note title", "Note content: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed consectetur nisi. Donec vitae dolor eget.");
    }
}