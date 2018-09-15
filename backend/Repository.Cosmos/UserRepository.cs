using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using WhiteRaven.Domain.Models.Authentication;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Contract.Exceptions;
using WhiteRaven.Repository.Cosmos.Entities;

namespace WhiteRaven.Repository.Cosmos
{
    public sealed class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private const string CollectionName = "Users";

        public UserRepository(IKeyFor<User> keyProvider) : base(CollectionName, keyProvider)
        {
        }

        public async Task<IEnumerable<User>> GetByPartialEmail(string partialEmail)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<User>>(DocumentCollectionUri)
                    .Where(storedUser => storedUser.Id.Contains(partialEmail))
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<User>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(User), ex);
            }
        }

        public async Task<IEnumerable<User>> GetByPartialFirstName(string partialFirstName)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<User>>(DocumentCollectionUri)
                    .Where(storedUser => storedUser.Entity.FirstName.Contains(partialFirstName))
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<User>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(User), ex);
            }
        }

        public async Task<IEnumerable<User>> GetByPartialLastName(string partialLastName)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<User>>(DocumentCollectionUri)
                    .Where(storedUser => storedUser.Entity.LastName.Contains(partialLastName))
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<User>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(User), ex);
            }
        }

        public async Task<IEnumerable<User>> GetByPartialFirstAndLastName(string partialFirstName, string partialLastName)
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<User>>(DocumentCollectionUri)
                    .Where(storedUser => storedUser.Entity.FirstName.Contains(partialFirstName) &&
                                         storedUser.Entity.LastName.Contains(partialLastName))
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<User>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(User), ex);
            }
        }
    }
}