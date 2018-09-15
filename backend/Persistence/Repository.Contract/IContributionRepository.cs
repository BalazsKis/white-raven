using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Repository.Contract
{
    public interface IContributionRepository : IRepository<Contribution>
    {
        Task<Contribution> GetByEmailAndNoteId(string email, string noteId);

        Task<IEnumerable<Contribution>> GetByEmail(string email);
        Task<IEnumerable<Contribution>> GetByNoteId(string noteId);

        Task<IEnumerable<Contribution>> GetByEmailAndContributionType(string email, ContributionType contributionType);
        Task<IEnumerable<Contribution>> GetByNoteIdAndContributionType(string noteId, ContributionType contributionType);

        Task DeleteByNoteId(string noteId);
    }
}