using System;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Repository.InMemory.Keys
{
    internal class NoteKey : IKeyFor<Note>
    {
        public Func<Note, string> KeyProvider =>
            n => n.Id;
    }
}