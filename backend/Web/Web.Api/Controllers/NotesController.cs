using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Examples;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations;
using WhiteRaven.Web.Api.Examples;

namespace WhiteRaven.Web.Api.Controllers
{
    /// <summary>
    /// API controller for Note operations
    /// </summary>
    [ApiController]
    [Route("api/notes")]
    public class NotesController : Controller
    {
        private readonly INoteOperations _noteOperations;


        /// <summary>
        /// Initializes a new instance of the <see cref="NotesController"/> class
        /// </summary>
        /// <param name="noteOperations">The note operations</param>
        public NotesController(INoteOperations noteOperations)
        {
            _noteOperations = noteOperations;
        }


        /// <summary>
        /// Returns all of the user's notes
        /// </summary>
        /// <returns>All of the user's notes</returns>
        [Authorize]
        [HttpGet("all")]
        public Task<IActionResult> GetAllNotes()
        {
            return GetNotes();
        }

        /// <summary>
        /// Returns all notes owned by the user
        /// </summary>
        /// <returns>All notes owned by the user</returns>
        [Authorize]
        [HttpGet("mine")]
        public Task<IActionResult> GetMyNotes()
        {
            return GetNotes(ContributionType.Owner);
        }

        /// <summary>
        /// Returns all notes shared with the user to read
        /// </summary>
        /// <returns>All notes shared with the user to read</returns>
        [Authorize]
        [HttpGet("shared/read")]
        public Task<IActionResult> GetSharedReadOnlyNotes()
        {
            return GetNotes(ContributionType.Reader);
        }

        /// <summary>
        /// Returns all notes shared with the user to read and write
        /// </summary>
        /// <returns>All notes shared with the user to read and write</returns>
        [Authorize]
        [HttpGet("shared/write")]
        public Task<IActionResult> GetSharedWritableNotes()
        {
            return GetNotes(ContributionType.Writer);
        }

        /// <summary>
        /// Returns a note by its unique ID
        /// </summary>
        /// <param name="id">The note's ID</param>
        /// <returns>The note with the given ID</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]string id)
        {
            var email = GetCurrentUserEmailAddress();
            return JsonApi.OkDataObject(await _noteOperations.GetNoteById(email, id));
        }

        /// <summary>
        /// Creates the specified note
        /// </summary>
        /// <param name="commit">The note to create</param>
        /// <returns>The created note</returns>
        [Authorize]
        [HttpPost]
        [SwaggerRequestExample(typeof(Commit), typeof(CommitExample))]
        public async Task<IActionResult> Create([Required, FromBody]Commit commit)
        {
            var email = GetCurrentUserEmailAddress();
            var note = await _noteOperations.CreateNote(email, commit);

            return CreatedAtAction(nameof(GetById), new { id = note.Id }, JsonApi.DataObject(note));
        }

        /// <summary>
        /// Updates the specified note
        /// </summary>
        /// <param name="id">The note's ID</param>
        /// <param name="commit">The content to update the note with</param>
        /// <returns>The updated note</returns>
        [Authorize]
        [HttpPatch("{id}")]
        [SwaggerRequestExample(typeof(Commit), typeof(CommitExample))]
        public async Task<IActionResult> Update([FromRoute]string id, [Required, FromBody]Commit commit)
        {
            var email = GetCurrentUserEmailAddress();
            return JsonApi.OkDataObject(await _noteOperations.UpdateNote(email, id, commit));
        }

        /// <summary>
        /// Deletes the specified note
        /// </summary>
        /// <param name="id">The note's ID</param>
        /// <returns>No content</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var email = GetCurrentUserEmailAddress();
            await _noteOperations.DeleteNote(email, id);

            return NoContent();
        }


        private async Task<IActionResult> GetNotes(ContributionType? contributionType = null)
        {
            var email = GetCurrentUserEmailAddress();
            return JsonApi.OkDataObject(await _noteOperations.GetNotesByUser(email, contributionType));
        }
    }
}