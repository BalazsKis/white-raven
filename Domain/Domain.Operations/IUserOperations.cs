using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Domain.Operations
{
    public interface IUserOperations
    {
        Task<User> CreateUser(Registration registration);

        Task<User> GetUser(string email);
        Task<User> ValidateLogin(Login login);
        Task<IEnumerable<User>> SearchUserByEmail(string partialEmail);
        Task<IEnumerable<User>> SearchUserByFirstName(string partialFirstName);
        Task<IEnumerable<User>> SearchUserByLastName(string partialLastName);
        Task<IEnumerable<User>> SearchUserByFullName(string partialFirstName, string partialLastName);

        Task<User> UpdateUserInfo(string email, InfoUpdate infoUpdate);
        Task UpdateUserPassword(string email, PasswordUpdate passwordUpdate);
    }
}