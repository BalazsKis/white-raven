using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Domain.Operations.Validation;
using WhiteRaven.Repository.Contract;

namespace WhiteRaven.Domain.Operations
{
    public class ContributionOperations : IContributionOperations
    {
        private readonly IContributionRepository _contributionRepository;
        private readonly IContributionValidator _contributionValidator;


        public ContributionOperations(
            IContributionRepository contributionRepository,
            IContributionValidator contributionValidator)
        {
            _contributionRepository = contributionRepository;
            _contributionValidator = contributionValidator;
        }


        public async Task<Contribution> CreateContribution(string editorEmail, Contribution contribution)
        {
            _contributionValidator.Validate(contribution);
            _contributionValidator.ValidateManualEdit(contribution);

            await CheckOwnerRight(editorEmail, contribution.NoteId);

            if (await _contributionRepository.Contains(contribution))
            {
                await _contributionRepository.Update(contribution);
            }
            else
            {
                await _contributionRepository.Insert(contribution);
            }

            return contribution;
        }

        public async Task<Contribution> GetContribution(string email, string noteId)
        {
            var contribution = await _contributionRepository.GetByEmailAndNoteId(email, noteId);

            if (contribution == default(Contribution))
            {
                throw new KeyNotFoundException($"A contribution for the user '{email}' and the note '{noteId}' was not found");
            }

            return contribution;
        }

        public async Task<IEnumerable<Contribution>> GetContributionsByUser(string email, ContributionType? contributionType = null)
        {
            return contributionType.HasValue
                ? await _contributionRepository.GetByEmailAndContributionType(email, contributionType.Value)
                : await _contributionRepository.GetByEmail(email);
        }

        public async Task<IEnumerable<Contribution>> GetContributionsByNote(string readerEmail, string noteId, ContributionType? contributionType = null)
        {
            await CheckReadRight(readerEmail, noteId);

            return contributionType.HasValue
                ? await _contributionRepository.GetByNoteIdAndContributionType(noteId, contributionType.Value)
                : await _contributionRepository.GetByNoteId(noteId);
        }

        public async Task UpdateContribution(string editorEmail, Contribution contribution)
        {
            _contributionValidator.Validate(contribution);
            _contributionValidator.ValidateManualEdit(contribution);

            await CheckOwnerRight(editorEmail, contribution.NoteId);
            await _contributionRepository.Update(contribution);
        }

        public async Task DeleteContribution(string editorEmail, Contribution contribution)
        {
            _contributionValidator.Validate(contribution);
            _contributionValidator.ValidateManualEdit(contribution);

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
            var contribution = Contribution.CreateOwnerContribution(noteId, creatorEmail);

            _contributionValidator.Validate(contribution);

            return _contributionRepository.Insert(contribution);
        }

        public Task NoteDeleted(string noteId)
        {
            return _contributionRepository.DeleteByNoteId(noteId);
        }


        public async Task CheckReadRight(string email, string noteId)
        {
            var c = await _contributionRepository.GetByEmailAndNoteId(email, noteId);

            if (c.ContributionType < ContributionType.Reader)
            {
                throw new UnauthorizedAccessException("The user has no contribution to this note, so cannot read it");
            }
        }

        public async Task CheckEditRight(string email, string noteId)
        {
            var c = await _contributionRepository.GetByEmailAndNoteId(email, noteId);

            if (c.ContributionType < ContributionType.Writer)
            {
                throw new UnauthorizedAccessException("The user has no right to edit this note");
            }
        }

        public async Task CheckOwnerRight(string email, string noteId)
        {
            var c = await _contributionRepository.GetByEmailAndNoteId(email, noteId);

            if (c.ContributionType < ContributionType.Owner)
            {
                throw new UnauthorizedAccessException("The user is not the owner of the note");
            }
        }
    }
}