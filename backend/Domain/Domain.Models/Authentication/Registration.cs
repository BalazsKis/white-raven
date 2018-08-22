using System;

namespace WhiteRaven.Domain.Models.Authentication
{
    public class Registration
    {
        public string FirstName { get; }
        public string LastName { get; }

        public string Email { get; }

        public DateTime BirthDate { get; }

        public string Password { get; }

        public Registration(string firstName, string lastName, string email, DateTime birthDate, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            Password = password;
        }
    }
}