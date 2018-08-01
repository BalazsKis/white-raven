using System.ComponentModel.DataAnnotations;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Interfaces;

namespace WhiteRaven.Domain.Operations
{
    public class ContributionValidator : IContributionValidator
    {
        private readonly IUserValidator _userValidator;
        private readonly INoteValidator _noteValidator;

        public ContributionValidator(IUserValidator userValidator, INoteValidator noteValidator)
        {
            _userValidator = userValidator;
            _noteValidator = noteValidator;
        }

        public void Validate(Contribution item)
        {
            _noteValidator.ValidateId(item.NoteId);
            _userValidator.ValidateEmailAddress(item.UserId);
        }

        public void ValidateManualEdit(Contribution contribution)
        {
            if (contribution.ContributionType == ContributionType.Owner)
            {
                throw new ValidationException($"A contribution of type '{ContributionType.Owner}' cannot be edited directly");
            }
        }
    }
}