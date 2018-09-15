using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.InMemory
{
    public sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IKeyFor<User> keyProvider) : base(keyProvider)
        {
        }

        public Task<IEnumerable<User>> GetByPartialEmail(string partialEmail)
        {
            return Get(storedUser => storedUser.Email.Contains(partialEmail));
        }

        public Task<IEnumerable<User>> GetByPartialFirstName(string partialFirstName)
        {
            return Get(storedUser => storedUser.FirstName.Contains(partialFirstName));
        }

        public Task<IEnumerable<User>> GetByPartialLastName(string partialLastName)
        {
            return Get(storedUser => storedUser.LastName.Contains(partialLastName));
        }

        public Task<IEnumerable<User>> GetByPartialFirstAndLastName(string partialFirstName, string partialLastName)
        {
            return Get(storedUser => storedUser.FirstName.Contains(partialFirstName) &&
                                     storedUser.LastName.Contains(partialLastName));
        }
    }
}