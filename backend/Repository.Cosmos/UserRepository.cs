using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private const string CollectionName = "Users";

        public UserRepository(IKeyFor<User> keyProvider) : base(CollectionName, keyProvider)
        {
        }

        public Task<IEnumerable<User>> GetByPartialEmail(string partialEmail)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetByPartialFirstName(string partialFirstName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetByPartialLastName(string partialLastName)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetByPartialFirstAndLastName(string partialFirstName, string partialLastName)
        {
            throw new System.NotImplementedException();
        }
    }
}