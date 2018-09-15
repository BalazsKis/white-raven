using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Validation;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Domain.Operations
{
    public class NoteOperations : INoteOperations
    {
        private readonly INoteRepository _noteRepository;
        private readonly IContributionOperations _contributionOperations;
        private readonly INoteValidator _noteValidator;


        public NoteOperations(
            INoteRepository noteRepository,
            IContributionOperations contributionOperations,
            INoteValidator noteValidator)
        {
            _noteRepository = noteRepository;
            _contributionOperations = contributionOperations;
            _noteValidator = noteValidator;
        }


        public async Task<Note> CreateNote(string creatorEmail, Commit commit)
        {
            var note = Note.Create(commit.Title, commit.Content);

            _noteValidator.Validate(note);

            await _noteRepository.Insert(note);

            await _contributionOperations.NoteCreated(creatorEmail, note.Id);

            return note;
        }

        public async Task<Note> GetNoteById(string readerEmail, string noteId)
        {
            await _contributionOperations.CheckReadRight(readerEmail, noteId);

            return await _noteRepository.GetByKey(noteId);
        }

        public async Task<IEnumerable<Note>> GetNotesByUser(string email, ContributionType? contributionType = null)
        {
            var contributions = await _contributionOperations.GetContributionsByUser(email, contributionType);

            var contributedNoteIds = contributions.Select(c => c.NoteId).ToArray();

            return await _noteRepository.GetByNoteIds(contributedNoteIds);
        }

        public async Task<Note> UpdateNote(string editorEmail, string noteId, Commit commit)
        {
            await _contributionOperations.CheckEditRight(editorEmail, noteId);

            var note = await _noteRepository.GetByKey(noteId);

            var updatedNote = note.UpdateTitleAndContent(commit.Title, commit.Content);

            _noteValidator.Validate(updatedNote);

            await _noteRepository.Update(updatedNote);

            return updatedNote;
        }

        public async Task DeleteNote(string editorEmail, string noteId)
        {
            await _contributionOperations.CheckOwnerRight(editorEmail, noteId);

            await _noteRepository.DeleteByKey(noteId);
            await _contributionOperations.NoteDeleted(noteId);
        }
    }
}