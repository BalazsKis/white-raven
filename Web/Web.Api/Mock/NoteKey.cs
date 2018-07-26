using System;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Web.Api.Mock
{
    public class NoteKey : IKeyFor<Note>
    {
        public Func<Note, string> KeyProvider =>
            note => note.Id;
    }
}