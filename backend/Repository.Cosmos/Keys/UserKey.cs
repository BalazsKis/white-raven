using System;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.Cosmos.Keys
{
    internal class UserKey : IKeyFor<User>
    {
        public Func<User, string> KeyProvider =>
            u => u.Email;
    }
}