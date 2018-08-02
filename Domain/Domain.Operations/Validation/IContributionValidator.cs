using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Domain.Operations.Validation
{
    public interface IContributionValidator : IValidator<Contribution>
    {
        void ValidateManualEdit(Contribution contribution);
    }
}