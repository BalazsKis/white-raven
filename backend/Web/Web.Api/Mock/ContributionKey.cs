using System;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Web.Api.Mock
{
    public class ContributionKey : IKeyFor<Contribution>
    {
        public Func<Contribution, string> KeyProvider =>
            contribution => $"{contribution.UserId}|{contribution.NoteId}";
    }
}