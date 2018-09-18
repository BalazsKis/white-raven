using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Contract.Exceptions;
using WhiteRaven.Repository.Cosmos.Configurations;
using WhiteRaven.Repository.Cosmos.Entities;

namespace WhiteRaven.Repository.Cosmos
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        protected readonly DbConnectionParameters DbConnection;
        protected readonly string CollectionName;
        protected readonly IKeyFor<T> KeyProvider;

        protected Uri DocumentCollectionUri { get; }
        protected DocumentClient Client { get; }

        protected RepositoryBase(DbConnectionParameters dbConnection, string collectionName, IKeyFor<T> keyProvider)
        {
            KeyProvider = keyProvider;
            DbConnection = dbConnection;

            CollectionName = collectionName;

            Client = new DocumentClient(new Uri(DbConnection.EndpointUri), DbConnection.PrimaryKey);
            DocumentCollectionUri = UriFactory.CreateDocumentCollectionUri(DbConnection.DbName, CollectionName);

            Client.CreateDatabaseIfNotExistsAsync(new Database { Id = DbConnection.DbName }).GetAwaiter().GetResult();
            Client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DbConnection.DbName),
                new DocumentCollection { Id = CollectionName }).GetAwaiter().GetResult();
        }

        public Task Insert(T item)
        {
            return Update(item);
        }

        public Task Insert(IEnumerable<T> items)
        {
            return Update(items);
        }

        public async Task<T> GetByKey(string key)
        {
            try
            {
                var response = await Client.ReadDocumentAsync<StoredEntity<T>>(UriFactory.CreateDocumentUri(DbConnection.DbName, CollectionName, key));
                return response.Document.Entity;
            }
            catch (DocumentClientException de) when (de.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(T), ex);
            }
        }

        public async Task<IEnumerable<T>> GetByKeys(IEnumerable<string> keys)
        {
            var tasks = keys.Select(GetByKey).ToArray();
            await Task.WhenAll(tasks);

            return tasks.Select(t => t.Result);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                var response = await Client
                    .CreateDocumentQuery<StoredEntity<T>>(DocumentCollectionUri, "SELECT * FROM Entities")
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<T>>();

                return response.Select(e => e.Entity).ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(T), ex);
            }
        }

        public async Task<int> Count()
        {
            try
            {
                var countResponse = await Client
                    .CreateDocumentQuery<int>(DocumentCollectionUri, "SELECT VALUE COUNT(1) FROM Entities")
                    .AsDocumentQuery()
                    .ExecuteNextAsync<int>();

                return countResponse.Single();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(T), ex);
            }
        }

        public Task<bool> Contains(T item)
        {
            return ContainsKey(KeyProvider.KeyProvider(item));
        }

        public async Task<bool> ContainsKey(string key)
        {
            try
            {
                await GetByKey(key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public Task Update(T item)
        {
            var storedEntity = new StoredEntity<T>
            {
                Id = KeyProvider.KeyProvider(item),
                Entity = item
            };

            return Client.UpsertDocumentAsync(DocumentCollectionUri, storedEntity);
        }

        public Task Update(IEnumerable<T> items)
        {
            return Task.Run(() => Task.WaitAll(items.Select(Update).ToArray()));
        }

        public Task Delete(T item)
        {
            return DeleteByKey(KeyProvider.KeyProvider(item));
        }

        public Task Delete(IEnumerable<T> items)
        {
            return Task.WhenAll(items.Select(Delete));
        }

        public Task DeleteByKey(string key)
        {
            return Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DbConnection.DbName, CollectionName, key));
        }

        public Task DeleteByKeys(IEnumerable<string> keys)
        {
            return Task.WhenAll(keys.Select(DeleteByKey));
        }


        protected async Task<IEnumerable<T>> GetByFilters(params Expression<Func<StoredEntity<T>, bool>>[] filters)
        {
            try
            {
                var query = filters.Aggregate(
                    Client.CreateDocumentQuery<StoredEntity<T>>(DocumentCollectionUri).AsQueryable(),
                    (current, filter) => current.Where(filter));

                var response = await query
                    .AsDocumentQuery()
                    .ExecuteNextAsync<StoredEntity<T>>();

                return response
                    .Select(e => e.Entity)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new ReadFailedException(typeof(User), ex);
            }
        }
    }
}
