using System;

namespace WhiteRaven.Domain.Models.Authentication
{
    public class InfoUpdate
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime BirthDate { get; }

        public InfoUpdate(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }
}