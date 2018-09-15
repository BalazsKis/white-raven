using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.InMemory
{
    public sealed class ContributionRepository : RepositoryBase<Contribution>, IContributionRepository
    {
        public ContributionRepository(IKeyFor<Contribution> keyProvider) : base(keyProvider)
        {
        }

        public Task<Contribution> GetByEmailAndNoteId(string email, string noteId)
        {
            var key = GetKey(new Contribution(email, noteId, ContributionType.Reader));
            return GetByKey(key);
        }

        public Task<IEnumerable<Contribution>> GetByEmail(string email)
        {
            return Get(storedContribution => storedContribution.UserId == email);
        }

        public Task<IEnumerable<Contribution>> GetByNoteId(string noteId)
        {
            return Get(storedContribution => storedContribution.NoteId == noteId);
        }

        public Task<IEnumerable<Contribution>> GetByEmailAndContributionType(string email, ContributionType contributionType)
        {
            return Get(storedContribution => storedContribution.UserId == email &&
                                             storedContribution.ContributionType == contributionType);
        }

        public Task<IEnumerable<Contribution>> GetByNoteIdAndContributionType(string noteId, ContributionType contributionType)
        {
            return Get(storedContribution => storedContribution.NoteId == noteId &&
                                             storedContribution.ContributionType == contributionType);
        }

        public async Task DeleteByNoteId(string noteId)
        {
            var toDelete = await GetByNoteId(noteId);
            await Task.WhenAll(toDelete.Select(Delete).ToArray());
        }
    }
}