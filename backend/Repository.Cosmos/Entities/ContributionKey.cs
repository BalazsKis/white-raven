using System;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.Cosmos.Entities
{
    internal class ContributionKey : IKeyFor<Contribution>
    {
        public Func<Contribution, string> KeyProvider =>
            c => $"{c.UserId}-{c.NoteId}";
    }
}