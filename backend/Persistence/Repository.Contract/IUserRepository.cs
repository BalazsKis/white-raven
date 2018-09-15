using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Repository.Contract
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetByPartialEmail(string partialEmail);
        Task<IEnumerable<User>> GetByPartialFirstName(string partialFirstName);
        Task<IEnumerable<User>> GetByPartialLastName(string partialLastName);
        Task<IEnumerable<User>> GetByPartialFirstAndLastName(string partialFirstName, string partialLastName);
    }
}
