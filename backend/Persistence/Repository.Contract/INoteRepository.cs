using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Repository.Contract
{
    public interface INoteRepository : IRepository<Note>
    {
        Task<IEnumerable<Note>> GetByNoteIds(IEnumerable<string> noteIds);
    }
}