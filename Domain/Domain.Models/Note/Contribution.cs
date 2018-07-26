using WhiteRaven.Domain.Models.Authentication;

namespace WhiteRaven.Domain.Models.Note
{
    public class Contribution
    {
        public string UserId { get; }

        public string NoteId { get; }

        public ContributionType ContributionType { get; }

        public Contribution(string userId, string noteId, ContributionType contributionType)
        {
            UserId = userId;
            NoteId = noteId;
            ContributionType = contributionType;
        }

        public static Contribution CreateOwnerContribution(string noteId, string userId) =>
            new Contribution(userId, noteId, ContributionType.Owner);
    }
}