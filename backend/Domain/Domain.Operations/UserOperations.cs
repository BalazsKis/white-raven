using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Domain.Operations.Validation;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Shared.Library.Cryptography;

namespace WhiteRaven.Domain.Operations
{
    public class UserOperations : IUserOperations
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordGuard _passwordGuard;
        private readonly IUserValidator _userValidator;


        public UserOperations(IUserRepository userRepository, IPasswordGuard passwordGuard, IUserValidator userValidator)
        {
            _userRepository = userRepository;
            _passwordGuard = passwordGuard;
            _userValidator = userValidator;
        }


        public async Task<User> CreateUser(Registration registration)
        {
            _userValidator.ValidatePassword(registration.Password);

            var passwordHash = _passwordGuard.GeneratePasswordHash(registration.Password);

            var user = new User(
                UserLevel.User,
                registration.FirstName,
                registration.LastName,
                registration.Email.ToLower(),
                registration.BirthDate,
                DateTime.UtcNow,
                passwordHash);

            _userValidator.Validate(user);

            await _userRepository.Insert(user);

            return user.WithoutPasswordHash();
        }

        public async Task<User> GetUser(string email)
        {
            return (await _userRepository.GetByKey(email)).WithoutPasswordHash();
        }

        public async Task<IEnumerable<User>> SearchUserByEmail(string partialEmail)
        {
            _userValidator.ValidateSearchTerm(partialEmail);

            var emailTerm = partialEmail.ToLower();
            var result = await _userRepository.GetByPartialEmail(emailTerm);

            return result.Select(u => u.WithoutPasswordHash());
        }

        public async Task<IEnumerable<User>> SearchUserByFirstName(string partialFirstName)
        {
            _userValidator.ValidateSearchTerm(partialFirstName);

            var result = await _userRepository.GetByPartialFirstName(partialFirstName);

            return result.Select(u => u.WithoutPasswordHash());
        }

        public async Task<IEnumerable<User>> SearchUserByLastName(string partialLastName)
        {
            _userValidator.ValidateSearchTerm(partialLastName);

            var result = await _userRepository.GetByPartialLastName(partialLastName);

            return result.Select(u => u.WithoutPasswordHash());
        }

        public async Task<IEnumerable<User>> SearchUserByFullName(string partialFirstName, string partialLastName)
        {
            _userValidator.ValidateSearchTerm(partialFirstName);
            _userValidator.ValidateSearchTerm(partialLastName);

            var result = await _userRepository.GetByPartialFirstAndLastName(partialFirstName, partialLastName);

            return result.Select(u => u.WithoutPasswordHash());
        }

        public async Task<User> ValidateLogin(Login login)
        {
            var user = await _userRepository.GetByKey(login.Email);

            if (!_passwordGuard.IsUserPasswordValid(user.HashedPassword, login.Password))
            {
                throw new UnauthorizedAccessException("The password was incorrect");
            }

            return user;
        }

        public async Task<User> UpdateUserInfo(string email, InfoUpdate infoUpdate)
        {
            var user = await GetUser(email);

            user = user.UpdateFirstName(infoUpdate.FirstName);
            user = user.UpdateLastName(infoUpdate.LastName);
            user = user.UpdateBirthDate(infoUpdate.BirthDate);

            _userValidator.Validate(user);

            await _userRepository.Update(user);

            return user.WithoutPasswordHash();
        }

        public async Task UpdateUserPassword(string email, PasswordUpdate passwordUpdate)
        {
            _userValidator.ValidatePassword(passwordUpdate.OldPassword);
            _userValidator.ValidatePassword(passwordUpdate.NewPassword);

            var user = await _userRepository.GetByKey(email);

            if (!_passwordGuard.IsUserPasswordValid(user.HashedPassword, passwordUpdate.OldPassword))
            {
                throw new UnauthorizedAccessException("The current password was incorrect");
            }

            var newPasswordHash = _passwordGuard.GeneratePasswordHash(passwordUpdate.NewPassword);
            var userWithUpdatedPassword = user.UpdatePasswordHash(newPasswordHash);

            _userValidator.Validate(userWithUpdatedPassword);

            await _userRepository.Update(userWithUpdatedPassword);
        }
    }
}