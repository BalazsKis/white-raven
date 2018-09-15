using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.InMemory
{
    public sealed class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(IKeyFor<Note> keyProvider) : base(keyProvider)
        {
        }

        public Task<IEnumerable<Note>> GetByNoteIds(IEnumerable<string> noteIds)
        {
            var ids = noteIds.ToArray();
            return Get(note => ids.Contains(note.Id));
        }
    }
}