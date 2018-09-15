using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class ContributionRepository : RepositoryBase<Contribution>, IContributionRepository
    {
        private const string CollectionName = "Contributions";

        public ContributionRepository(IKeyFor<Contribution> keyProvider) : base(CollectionName, keyProvider)
        {
        }

        public Task<Contribution> GetByEmailAndNoteId(string email, string noteId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contribution>> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contribution>> GetByNoteId(string noteId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contribution>> GetByEmailAndContributionType(string email, ContributionType contributionType)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contribution>> GetByNoteIdAndContributionType(string noteId, ContributionType contributionType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByNoteId(string noteId)
        {
            throw new NotImplementedException();
        }
    }
}