using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Interfaces;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Domain.Operations
{
    public class ContributionOperations : IContributionOperations
    {
        private readonly IRepository<Contribution> _contributionRepository;


        public ContributionOperations(IRepository<Contribution> contributionRepository)
        {
            _contributionRepository = contributionRepository;
        }


        public async Task<Contribution> CreateContribution(string editorEmail, Contribution contribution)
        {
            if (contribution.ContributionType == ContributionType.Owner)
                throw new ArgumentException($"A contribution of type '{ContributionType.Owner}' cannot be inserted directly");

            await CheckOwnerRight(editorEmail, contribution.NoteId);
            await _contributionRepository.Insert(contribution);

            return contribution;
        }

        public async Task<Contribution> GetContribution(string email, string noteId)
        {
            var contributions = await _contributionRepository.Select(c => c.UserId == email && c.NoteId == noteId);
            var contribution = contributions.SingleOrDefault();

            if (contribution == default(Contribution))
                throw new KeyNotFoundException($"A contribution for the user '{email}' and the note '{noteId}' was not found");

            return contribution;
        }

        public async Task<IEnumerable<Contribution>> GetContributionsByUser(string email, ContributionType? contributionType = null)
        {
            var filter = contributionType == null
                ? new Func<Contribution, bool>(c => c.UserId == email)
                : new Func<Contribution, bool>(c => c.UserId == email && c.ContributionType == contributionType);

            return await _contributionRepository.Select(filter);
        }

        public async Task<IEnumerable<Contribution>> GetContributionsByNote(string readerEmail, string noteId, ContributionType? contributionType = null)
        {
            await CheckReadRight(readerEmail, noteId);

            var filter = contributionType == null
                ? new Func<Contribution, bool>(c => c.NoteId == noteId)
                : new Func<Contribution, bool>(c => c.NoteId == noteId && c.ContributionType == contributionType);

            return await _contributionRepository.Select(filter);
        }

        public async Task UpdateContribution(string editorEmail, Contribution contribution)
        {
            if (contribution.ContributionType == ContributionType.Owner)
                throw new ArgumentException($"A contribution cannot be upgraded to type '{ContributionType.Owner}'");

            await CheckOwnerRight(editorEmail, contribution.NoteId);
            await _contributionRepository.Update(contribution);
        }

        public async Task DeleteContribution(string editorEmail, Contribution contribution)
        {
            if (contribution.ContributionType == ContributionType.Owner)
                throw new ArgumentException($"A contribution of type '{ContributionType.Owner}' cannot be deleted directly");

            await CheckOwnerRight(editorEmail, contribution.NoteId);
            await _contributionRepository.Delete(contribution);
        }

        public async Task DeleteContribution(string editorEmail, string contributorEmail, string noteId)
        {
            var contribution = await GetContribution(contributorEmail, noteId);
            await DeleteContribution(editorEmail, contribution);
        }


        public Task NoteCreated(string creatorEmail, string noteId)
        {
            return _contributionRepository.Insert(Contribution.CreateOwnerContribution(noteId, creatorEmail));
        }

        public Task NoteDeleted(string noteId)
        {
            return _contributionRepository.Delete(c => c.NoteId == noteId);
        }


        public async Task CheckReadRight(string email, string noteId)
        {
            var contributions = await _contributionRepository.Select(c => c.NoteId == noteId && c.UserId == email);

            if (contributions.Any(c => c.ContributionType >= ContributionType.Reader))
                return;

            throw new UnauthorizedAccessException("The user has no contribution to this note, so cannot read it");
        }

        public async Task CheckEditRight(string email, string noteId)
        {
            var contributions = await _contributionRepository.Select(c => c.NoteId == noteId && c.UserId == email);

            if (contributions.Any(c => c.ContributionType >= ContributionType.Contributor))
                return;

            throw new UnauthorizedAccessException("The user has no right to edit this note");
        }

        public async Task CheckOwnerRight(string email, string noteId)
        {
            var contributions = await _contributionRepository.Select(c => c.NoteId == noteId && c.UserId == email);

            if (contributions.Any(c => c.ContributionType >= ContributionType.Owner))
                return;

            throw new UnauthorizedAccessException("The user is not the owner of the note");
        }
    }
}