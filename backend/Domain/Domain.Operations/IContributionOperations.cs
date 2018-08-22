using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;

namespace WhiteRaven.Domain.Operations
{
    public interface IContributionOperations
    {
        Task<Contribution> CreateContribution(string editorEmail, Contribution contribution);

        Task<Contribution> GetContribution(string email, string noteId);
        Task<IEnumerable<Contribution>> GetContributionsByUser(string email, ContributionType? contributionType = null);
        Task<IEnumerable<Contribution>> GetContributionsByNote(string readerEmail, string noteId, ContributionType? contributionType = null);

        Task UpdateContribution(string editorEmail, Contribution contribution);

        Task DeleteContribution(string editorEmail, Contribution contribution);
        Task DeleteContribution(string editorEmail, string contributorEmail, string noteId);

        Task NoteCreated(string creatorEmail, string noteId);
        Task NoteDeleted(string noteId);

        Task CheckReadRight(string email, string noteId);
        Task CheckEditRight(string email, string noteId);
        Task CheckOwnerRight(string email, string noteId);
    }
}