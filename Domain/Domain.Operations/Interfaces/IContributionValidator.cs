using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Domain.Operations.Interfaces
{
    public interface IContributionValidator : IValidator<Contribution>
    {
        void ValidateManualEdit(Contribution contribution);
    }
}