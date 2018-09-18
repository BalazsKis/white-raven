using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Cosmos.Configurations;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class ContributionRepository : RepositoryBase<Contribution>, IContributionRepository
    {
        public ContributionRepository(DbConnectionParameters dbConnection, IKeyFor<Contribution> keyProvider)
            : base(dbConnection, "Contributions", keyProvider)
        {
        }

        public Task<Contribution> GetByEmailAndNoteId(string email, string noteId)
        {
            var key = KeyProvider.KeyProvider(new Contribution(email, noteId, ContributionType.Reader));
            return GetByKey(key);
        }

        public Task<IEnumerable<Contribution>> GetByEmail(string email)
        {
            return GetByFilters(storedContribution => storedContribution.Entity.UserId == email);
        }

        public Task<IEnumerable<Contribution>> GetByNoteId(string noteId)
        {
            return GetByFilters(storedContribution => storedContribution.Entity.NoteId == noteId);
        }

        public Task<IEnumerable<Contribution>> GetByEmailAndContributionType(string email, ContributionType contributionType)
        {
            return GetByFilters(storedContribution => storedContribution.Entity.UserId == email,
                                storedContribution => storedContribution.Entity.ContributionType == contributionType);
        }

        public Task<IEnumerable<Contribution>> GetByNoteIdAndContributionType(string noteId,
            ContributionType contributionType)
        {
            return GetByFilters(storedContribution => storedContribution.Entity.NoteId == noteId,
                                storedContribution => storedContribution.Entity.ContributionType == contributionType);
        }

        public async Task DeleteByNoteId(string noteId)
        {
            var toDelete = await GetByNoteId(noteId);
            await Task.WhenAll(toDelete.Select(Delete).ToArray());
        }
    }
}