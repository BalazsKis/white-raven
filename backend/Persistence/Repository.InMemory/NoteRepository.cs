using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.InMemory
{
    public sealed class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(IKeyFor<Note> keyProvider) : base(keyProvider)
        {
        }
    }
}