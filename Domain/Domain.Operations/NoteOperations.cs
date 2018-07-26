using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Shared.Basics;

namespace WhiteRaven.Domain.Operations
{
    public class NoteOperations : INoteOperations
    {
        private readonly IRepository<Note> _noteRepository;
        private readonly IContributionOperations _contributionOperations;


        public NoteOperations(
            IRepository<Note> noteRepository,
            IContributionOperations contributionOperations)
        {
            _noteRepository = noteRepository;
            _contributionOperations = contributionOperations;
        }


        public async Task<Note> CreateNote(string creatorEmail, Commit commit)
        {
            var note = Note.Create(commit.Title, commit.Content);

            await _noteRepository.Insert(note);
            await _contributionOperations.NoteCreated(creatorEmail, note.Id);

            return note;
        }

        public async Task<Note> GetNoteById(string readerEmail, string noteId)
        {
            await _contributionOperations.CheckReadRight(readerEmail, noteId);
            return await _noteRepository.SelectByKey(noteId);
        }

        public async Task<IEnumerable<Note>> GetNotesByUser(string email, ContributionType? contributionType = null)
        {
            var contributions = await _contributionOperations.GetContributionsByUser(email, contributionType);

            var contributedNoteIds = contributions.Select(c => c.NoteId).ToArray();

            return await _noteRepository.Select(n => contributedNoteIds.Contains(n.Id));
        }

        public async Task<Note> UpdateNote(string editorEmail, string noteId, Commit commit)
        {
            if (commit.Title.IsBlank() && commit.Content.IsBlank())
                throw new ArgumentException("Both title and content of a note cannot be blank", nameof(commit));

            await _contributionOperations.CheckEditRight(editorEmail, noteId);

            var note = await _noteRepository.SelectByKey(noteId);

            var updatedNote = note.UpdateTitleAndContent(commit.Title, commit.Content);
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