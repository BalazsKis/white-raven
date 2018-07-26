using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Domain.Operations.Interfaces
{
    public interface INoteOperations
    {
        Task<Note> CreateNote(string creatorEmail, Commit commit);

        Task<Note> GetNoteById(string readerEmail, string noteId);
        Task<IEnumerable<Note>> GetNotesByUser(string readerEmail, ContributionType? contributionType = null);

        Task<Note> UpdateNote(string editorEmail, string noteId, Commit commit);
        
        Task DeleteNote(string editorEmail, string noteId);
    }
}