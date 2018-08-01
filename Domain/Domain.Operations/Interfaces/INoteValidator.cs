using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Domain.Operations.Interfaces
{
    public interface INoteValidator : IValidator<Note>
    {
        void ValidateId(string noteId);
    }
}