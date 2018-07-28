using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Web.Api.Examples;

namespace WhiteRaven.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributionsController : Controller
    {
        private readonly IContributionOperations _contributionOperations;


        public ContributionsController(IContributionOperations contributionOperations)
        {
            _contributionOperations = contributionOperations;
        }


        [Authorize]
        [HttpGet("all")]
        public Task<IActionResult> GetAllMyContributions() =>
            GetContributions();

        [Authorize]
        [HttpGet("mine")]
        public Task<IActionResult> GetMyOwnedContributions() =>
            GetContributions(ContributionType.Owner);

        [Authorize]
        [HttpGet("shared/view")]
        public Task<IActionResult> GetSharedReadOnlyContributions() =>
            GetContributions(ContributionType.Reader);

        [Authorize]
        [HttpGet("shared/contribute")]
        public Task<IActionResult> GetSharedWritableContributions() =>
            GetContributions(ContributionType.Contributor);

        [Authorize]
        [HttpGet("to/note/{id}")]
        public async Task<IActionResult> GetContributionsToNote(string id)
        {
            var email = GetCurrentUserEmailAddress();
            return JsonApi.OkDataObject(await _contributionOperations.GetContributionsByNote(email, id));
        }

        [Authorize]
        [HttpPost]
        [SwaggerRequestExample(typeof(Contribution), typeof(ContributionExample))]
        public async Task<IActionResult> Create([FromBody]Contribution contribution)
        {
            var email = GetCurrentUserEmailAddress();
            await _contributionOperations.CreateContribution(email, contribution);

            return NoContent();
        }

        [Authorize]
        [HttpPatch]
        [SwaggerRequestExample(typeof(Contribution), typeof(ContributionExample))]
        public async Task<IActionResult> Update([FromBody]Contribution contribution)
        {
            var email = GetCurrentUserEmailAddress();
            await _contributionOperations.UpdateContribution(email, contribution);

            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        [SwaggerRequestExample(typeof(Contribution), typeof(ContributionExample))]
        public async Task<IActionResult> Delete([FromBody] Contribution contribution)
        {
            var email = GetCurrentUserEmailAddress();
            await _contributionOperations.DeleteContribution(email, contribution);

            return NoContent();
        }


        private async Task<IActionResult> GetContributions(ContributionType? contributionType = null)
        {
            var email = GetCurrentUserEmailAddress();
            return JsonApi.OkDataObject(await _contributionOperations.GetContributionsByUser(email, contributionType));
        }
    }
}