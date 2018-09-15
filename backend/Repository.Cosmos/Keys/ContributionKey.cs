using System;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.Cosmos.Keys
{
    internal class ContributionKey : IKeyFor<Contribution>
    {
        public Func<Contribution, string> KeyProvider =>
            c => $"{c.UserId}-{c.NoteId}";
    }
}