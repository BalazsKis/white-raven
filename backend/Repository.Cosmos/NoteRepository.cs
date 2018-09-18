using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Cosmos.Configurations;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        public NoteRepository(DbConnectionParameters dbConnection, IKeyFor<Note> keyProvider)
            : base(dbConnection, "Notes", keyProvider)
        {
        }
    }
}