using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Domain.Operations.Validation
{
    public class UserValidator : IUserValidator
    {
        private static readonly DateTime MinBirthDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MinRegistrationDate = new DateTime(2018, 1, 1);

        public void Validate(User item)
        {
            ValidateEmailAddress(item.Email);

            if (item.BirthDate > DateTime.UtcNow || item.BirthDate < MinBirthDate)
            {
                throw new ValidationException("The user's birth date is invalid");
            }

            if (item.RegistrationDate > DateTime.UtcNow || item.RegistrationDate < MinRegistrationDate)
            {
                throw new ValidationException("The user's registration date is invalid");
            }

            if (string.IsNullOrWhiteSpace(item.FirstName))
            {
                throw new ValidationException("The user's first name is blank");
            }

            if (string.IsNullOrWhiteSpace(item.LastName))
            {
                throw new ValidationException("The user's last name is blank");
            }

            if (string.IsNullOrWhiteSpace(item.HashedPassword))
            {
                throw new ValidationException("The user's secured password is blank");
            }
        }

        public void ValidateEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ValidationException("The email address is blank");
            }

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch (FormatException ex)
            {
                throw new ValidationException("The email address is invalid", ex);
            }
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                throw new ValidationException("The password cannot be blank, and it must be at least 8 characters long");
            }
        }

        public void ValidateSearchTerm(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 3)
            {
                throw new ValidationException("The search term cannot be blank and it must be at least 3 characters long");
            }
        }
    }
}