using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Domain.Operations.Interfaces
{
    public interface IUserOperations
    {
        Task<User> CreateUser(Registration registration);

        Task<User> GetUser(string email);
        Task<User> ValidateLogin(Login login);

        Task<User> UpdateUserInfo(string email, InfoUpdate infoUpdate);
        Task UpdateUserPassword(string email, PasswordUpdate passwordUpdate);
    }
}