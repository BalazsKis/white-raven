using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Cosmos.Configurations;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbConnectionParameters dbConnection, IKeyFor<User> keyProvider)
            : base(dbConnection, "Users", keyProvider)
        {
        }

        public Task<IEnumerable<User>> GetByPartialEmail(string partialEmail)
        {
            return GetByFilters(storedUser => storedUser.Id.Contains(partialEmail));
        }

        public Task<IEnumerable<User>> GetByPartialFirstName(string partialFirstName)
        {
            return GetByFilters(storedUser => storedUser.Entity.FirstName.Contains(partialFirstName));
        }

        public Task<IEnumerable<User>> GetByPartialLastName(string partialLastName)
        {
            return GetByFilters(storedUser => storedUser.Entity.LastName.Contains(partialLastName));
        }

        public Task<IEnumerable<User>> GetByPartialFirstAndLastName(string partialFirstName, string partialLastName)
        {
            return GetByFilters(storedUser => storedUser.Entity.FirstName.Contains(partialFirstName),
                                storedUser => storedUser.Entity.LastName.Contains(partialLastName));
        }
    }
}