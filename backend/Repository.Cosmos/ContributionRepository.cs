using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WhiteRaven.Domain.Models.Note;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Contract.Exceptions;
using WhiteRaven.Repository.Cosmos.Configurations;
using WhiteRaven.Repository.Cosmos.Entities;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class ContributionRepository : RepositoryBase<Contribution>, IContributionRepository
    {
        public ContributionRepository(DbConnectionParameters dbConnection, IKeyFor<Contribution> keyProvider)
            : base(dbConnection, "Contributions", keyProvider)
        {
        }

        public Task<Contribution> GetByEmailAndNoteId(string email, string noteId)
        {
            var key = KeyProvider.KeyProvider(new Contribution(email, noteId, ContributionType.Reader));
            return GetByKey(key);
        }

        public async Task<IEnumerable<Contribution>> GetByEmail(string email)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<Contribution>>(DocumentCollectionUri)
                    .Where(storedContribution => storedContribution.Entity.UserId == email)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<Contribution>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(Contribution), ex);
            }
        }

        public async Task<IEnumerable<Contribution>> GetByNoteId(string noteId)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<Contribution>>(DocumentCollectionUri)
                    .Where(storedContribution => storedContribution.Entity.NoteId == noteId)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<Contribution>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(Contribution), ex);
            }
        }

        public async Task<IEnumerable<Contribution>> GetByEmailAndContributionType(string email, ContributionType contributionType)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<Contribution>>(DocumentCollectionUri)
                    .Where(storedContribution => storedContribution.Entity.UserId == email &&
                                                 storedContribution.Entity.ContributionType == contributionType)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<Contribution>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (DocumentClientException de) when (de.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(Contribution), ex);
            }
        }

        public async Task<IEnumerable<Contribution>> GetByNoteIdAndContributionType(string noteId, ContributionType contributionType)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<Contribution>>(DocumentCollectionUri)
                    .Where(storedContribution => storedContribution.Entity.NoteId == noteId &&
                                                 storedContribution.Entity.ContributionType == contributionType)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<Contribution>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (DocumentClientException de) when (de.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(Contribution), ex);
            }
        }

        public async Task DeleteByNoteId(string noteId)
        {
            var toDelete = await GetByNoteId(noteId);
            await Task.WhenAll(toDelete.Select(Delete).ToArray());
        }
    }
}