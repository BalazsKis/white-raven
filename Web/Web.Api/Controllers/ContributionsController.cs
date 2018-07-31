using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Web.Api.Examples;

namespace WhiteRaven.Web.Api.Controllers
{
    /// <summary>
    /// API controller for Contribution operations
    /// </summary>
    [ApiController]
    [Route("api/contributions")]
    public class ContributionsController : Controller
    {
        private readonly IContributionOperations _contributionOperations;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContributionsController"/> class
        /// </summary>
        /// <param name="contributionOperations">The contribution operations</param>
        public ContributionsController(IContributionOperations contributionOperations)
        {
            _contributionOperations = contributionOperations;
        }


        /// <summary>
        /// Returns all of the user's contributions
        /// </summary>
        /// <returns>All of the user's contributions</returns>
        [Authorize]
        [HttpGet("all")]
        public Task<IActionResult> GetAllMyContributions() =>
            GetContributions();

        /// <summary>
        /// Returns all of the user's owner type contributions
        /// </summary>
        /// <returns>All of the user's owner type contributions</returns>
        [Authorize]
        [HttpGet("mine")]
        public Task<IActionResult> GetMyOwnedContributions() =>
            GetContributions(ContributionType.Owner);

        /// <summary>
        /// Returns all of the user's read only contributions
        /// </summary>
        /// <returns>All of the user's read only contributions</returns>
        [Authorize]
        [HttpGet("shared/read")]
        public Task<IActionResult> GetSharedReadOnlyContributions() =>
            GetContributions(ContributionType.Reader);

        /// <summary>
        /// Returns all of the user's writable contributions
        /// </summary>
        /// <returns>All of the user's writable contributions</returns>
        [Authorize]
        [HttpGet("shared/write")]
        public Task<IActionResult> GetSharedWritableContributions() =>
            GetContributions(ContributionType.Writer);

        /// <summary>
        /// Returns all contributions belonging to a note
        /// </summary>
        /// <param name="id">The note's ID</param>
        /// <returns>All contributions belonging to the note</returns>
        [Authorize]
        [HttpGet("to/note/{id}")]
        public async Task<IActionResult> GetContributionsToNote([FromRoute]string id)
        {
            var email = GetCurrentUserEmailAddress();
            return JsonApi.OkDataObject(await _contributionOperations.GetContributionsByNote(email, id));
        }

        /// <summary>
        /// Creates the specified contribution
        /// </summary>
        /// <param name="contribution">The contribution to create</param>
        /// <returns>No content</returns>
        [Authorize]
        [HttpPost]
        [SwaggerRequestExample(typeof(Contribution), typeof(ContributionExample))]
        public async Task<IActionResult> Create([Required, FromBody]Contribution contribution)
        {
            var email = GetCurrentUserEmailAddress();
            await _contributionOperations.CreateContribution(email, contribution);

            return NoContent();
        }

        /// <summary>
        /// Updates the specified contribution
        /// </summary>
        /// <param name="contribution">The contribution to update</param>
        /// <returns>No content</returns>
        [Authorize]
        [HttpPatch]
        [SwaggerRequestExample(typeof(Contribution), typeof(ContributionExample))]
        public async Task<IActionResult> Update([Required, FromBody]Contribution contribution)
        {
            var email = GetCurrentUserEmailAddress();
            await _contributionOperations.UpdateContribution(email, contribution);

            return NoContent();
        }

        /// <summary>
        /// Deletes the specified contribution
        /// </summary>
        /// <param name="contribution">The contribution to delete</param>
        /// <returns>No content</returns>
        [Authorize]
        [HttpDelete]
        [SwaggerRequestExample(typeof(Contribution), typeof(ContributionExample))]
        public async Task<IActionResult> Delete([Required, FromBody]Contribution contribution)
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