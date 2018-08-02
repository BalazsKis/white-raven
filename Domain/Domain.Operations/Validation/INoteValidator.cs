using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Domain.Operations.Validation
{
    public interface INoteValidator : IValidator<Note>
    {
        void ValidateId(string noteId);
    }
}