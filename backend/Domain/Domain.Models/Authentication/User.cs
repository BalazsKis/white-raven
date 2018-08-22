using System;

namespace WhiteRaven.Domain.Models.Authentication
{
    public class User
    {
        public UserLevel Level { get; }

        public string FirstName { get; }
        public string LastName { get; }

        public string Email { get; }

        public DateTime BirthDate { get; }
        public DateTime RegistrationDate { get; }

        public string HashedPassword { get; }


        public User(
            UserLevel level,
            string firstName,
            string lastName,
            string email,
            DateTime birthDate,
            DateTime registrationDate,
            string hashedPassword)
        {
            Level = level;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            RegistrationDate = registrationDate;
            HashedPassword = hashedPassword;
        }

        public User UpdatePasswordHash(string newPasswordHash) =>
            new User(Level, FirstName, LastName, Email, BirthDate, RegistrationDate, newPasswordHash);

        public User UpdateFirstName(string newFirstName) =>
            new User(Level, newFirstName, LastName, Email, BirthDate, RegistrationDate, HashedPassword);

        public User UpdateLastName(string newLastName) =>
            new User(Level, FirstName, newLastName, Email, BirthDate, RegistrationDate, HashedPassword);

        public User UpdateBirthDate(DateTime newBirthDate) =>
            new User(Level, FirstName, LastName, Email, newBirthDate, RegistrationDate, HashedPassword);

        public User WithoutPasswordHash() =>
            new User(Level, FirstName, LastName, Email, BirthDate, RegistrationDate, string.Empty);
    }
}