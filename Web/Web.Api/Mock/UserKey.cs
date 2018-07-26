using System;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Web.Api.Mock
{
    public class UserKey : IKeyFor<User>
    {
        public Func<User, string> KeyProvider =>
            user => user.Email;
    }
}