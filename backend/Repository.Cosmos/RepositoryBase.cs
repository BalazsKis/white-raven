﻿using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Contract.Exceptions;
using WhiteRaven.Repository.Cosmos.Entities;

namespace WhiteRaven.Repository.Cosmos
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        // TODO: from config
        private readonly string _endpointUri = "https://localhost:8081";
        private readonly string _primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        protected readonly string DbName = "WRN";
        // TODO: from config

        protected Uri DocumentCollectionUri { get; }
        protected DocumentClient Client { get; }
        protected readonly IKeyFor<T> KeyProvider;

        private readonly string _collectionName;

        protected RepositoryBase(string collectionName, IKeyFor<T> keyProvider)
        {
            _collectionName = collectionName;
            KeyProvider = keyProvider;

            Client = new DocumentClient(new Uri(_endpointUri), _primaryKey);
            DocumentCollectionUri = UriFactory.CreateDocumentCollectionUri(DbName, _collectionName);

            Client.CreateDatabaseIfNotExistsAsync(new Database { Id = DbName }).GetAwaiter().GetResult();
            Client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DbName),
                new DocumentCollection { Id = _collectionName }).GetAwaiter().GetResult();
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
                var response = await Client.ReadDocumentAsync<StoredEntity<T>>(UriFactory.CreateDocumentUri(DbName, _collectionName, key));
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
            return Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DbName, _collectionName, key));
        }

        public Task DeleteByKeys(IEnumerable<string> keys)
        {
            return Task.WhenAll(keys.Select(DeleteByKey));
        }
    }
}
