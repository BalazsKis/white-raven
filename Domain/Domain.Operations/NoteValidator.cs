using System;
using System.ComponentModel.DataAnnotations;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Interfaces;

namespace WhiteRaven.Domain.Operations
{
    public class NoteValidator : INoteValidator
    {
        private static readonly DateTime MinDateTime = new DateTime(2018, 1, 1);

        public void Validate(Note item)
        {
            ValidateId(item.Id);

            if (string.IsNullOrWhiteSpace(item.Title) &&
                string.IsNullOrWhiteSpace(item.Content))
            {
                throw new ValidationException("The note is blank (both title and content)");
            }

            if (item.CreatedUtc > DateTime.UtcNow || item.CreatedUtc < MinDateTime)
            {
                throw new ValidationException("Creation time is invalid");
            }

            if (item.LastModifiedUtc > DateTime.UtcNow || item.LastModifiedUtc < item.CreatedUtc)
            {
                throw new ValidationException("Last modification time is invalid");
            }
        }

        public void ValidateId(string noteId)
        {
            try
            {
                var guid = new Guid(noteId);
            }
            catch (Exception ex)
            {
                throw new ValidationException("The provided ID is not a valid note ID", ex);
            }
        }
    }
}