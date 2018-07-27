using System;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Shared.Basics;
using WhiteRaven.Shared.Basics.Cryptography;

namespace WhiteRaven.Domain.Operations
{
    public class UserOperations : IUserOperations
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordGuard _passwordGuard;


        public UserOperations(IRepository<User> userRepository, IPasswordGuard passwordGuard)
        {
            _userRepository = userRepository;
            _passwordGuard = passwordGuard;
        }


        public async Task<User> CreateUser(Registration registration)
        {
            if (registration.FirstName.IsBlank() ||
                registration.LastName.IsBlank() ||
                registration.Email.IsBlank() ||
                registration.Password.IsBlank())
                throw new ArgumentException("Some of the mandatory registration fields were not filled");

            var user = new User(
                UserLevel.User,
                registration.FirstName,
                registration.LastName,
                registration.Email,
                registration.BirthDate,
                DateTime.UtcNow,
                _passwordGuard.GeneratePasswordHash(registration.Password));

            await _userRepository.Insert(user);

            return user.WithoutPasswordHash();
        }

        public async Task<User> GetUser(string email)
        {
            CheckEmail(email);

            return await _userRepository.SelectByKey(email);
        }

        public async Task<User> ValidateLogin(Login login)
        {
            CheckEmail(login.Email);
            
            if(login.Password.IsBlank())
                throw new ArgumentException("The user's password cannot be blank");

            var user = await GetUser(login.Email);

            if (!_passwordGuard.IsUserPasswordValid(user.HashedPassword, login.Password))
                throw new UnauthorizedAccessException("The password was incorrect");

            return user;
        }

        public async Task<User> UpdateUserInfo(string email, InfoUpdate infoUpdate)
        {
            CheckEmail(email);

            var user = await GetUser(email);

            if (!infoUpdate.FirstName.IsBlank())
            {
                user = user.UpdateFirstName(infoUpdate.FirstName);
            }

            if (!infoUpdate.LastName.IsBlank())
            {
                user = user.UpdateLastName(infoUpdate.LastName);
            }

            if (infoUpdate.BirthDate.HasValue)
            {
                user = user.UpdateBirthDate(infoUpdate.BirthDate.Value);
            }

            await _userRepository.Update(user);

            return user.WithoutPasswordHash();
        }

        public async Task UpdateUserPassword(string email, PasswordUpdate passwordUpdate)
        {
            CheckEmail(email);

            if (passwordUpdate.OldPassword.IsBlank() || passwordUpdate.NewPassword.IsBlank())
                throw new ArgumentException("The old or the new password field was not filled");

            var user = await GetUser(email);

            if (!_passwordGuard.IsUserPasswordValid(user.HashedPassword, passwordUpdate.OldPassword))
                throw new UnauthorizedAccessException("The current password was incorrect");

            var newPasswordHash = _passwordGuard.GeneratePasswordHash(passwordUpdate.NewPassword);
            var userWithUpdatedPassword = user.UpdatePasswordHash(newPasswordHash);

            await _userRepository.Update(userWithUpdatedPassword);
        }


        private void CheckEmail(string email)
        {
            if (email.IsBlank())
                throw new ArgumentException("The user's email address (the user's unique ID) cannot be blank");
        }
    }
}