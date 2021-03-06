﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteRaven.Repository.Contract;
using WhiteRaven.Repository.Contract.Exceptions;

namespace WhiteRaven.Repository.InMemory
{
    public class RepositoryBase<T> : IRepository<T>
    {
        private static readonly ConcurrentDictionary<string, T> Repo
            = new ConcurrentDictionary<string, T>();

        protected readonly Func<T, string> GetKey;
        private readonly Type _type;


        public RepositoryBase(IKeyFor<T> keyGenerator)
        {
            GetKey = keyGenerator?.KeyProvider ?? (item => item.GetHashCode().ToString());
            _type = typeof(T);
        }


        #region CRUD operations

        #region Create

        public Task Insert(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default(T)))
            {
                throw new CreateFailedException(_type, "Default values are not allowed in the repository");
            }

            return Task.Run(() =>
            {
                try
                {
                    var key = GetKey(item);

                    if (Repo.ContainsKey(key))
                        throw new CreateFailedException(_type,
                            "Cannot create item with key '{key}' because it already exists");

                    var added = Repo.TryAdd(GetKey(item), item);

                    if (!added)
                        throw new CreateFailedException(_type);
                }
                catch (Exception ex)
                {
                    throw new CreateFailedException(_type, ex);
                }
            });
        }

        public Task Insert(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new CreateFailedException(_type, "The item collection was null");
            }

            return DoWithMany(items, Insert);
        }

        #endregion

        #region Read

        public Task<T> GetByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ReadFailedException(_type, "The key cannot be null or empty");
            }

            return Task.Run(() =>
            {
                try
                {
                    CheckKey(key);

                    var isRetrieved = Repo.TryGetValue(key, out var item);

                    if (!isRetrieved)
                        throw new ReadFailedException(_type);

                    return item;
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
                catch (ReadFailedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ReadFailedException(_type, ex);
                }
            });
        }

        public async Task<IEnumerable<T>> GetByKeys(IEnumerable<string> keys)
        {
            var tasks = keys.Select(GetByKey).ToArray();
            await Task.WhenAll(tasks);

            return tasks.Select(t => t.Result);
        }

        protected Task<IEnumerable<T>> Get(Func<T, bool> filter)
        {
            if (filter == null)
            {
                throw new ReadFailedException(_type, "The filter cannot be null");
            }

            return Task.Run(() =>
            {
                try
                {
                    return Repo.Values.Where(filter).ToList().AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw new ReadFailedException(_type, ex);
                }
            });
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Repo.Values.ToList().AsEnumerable();
                }
                catch (Exception ex)
                {
                    throw new ReadFailedException(_type, ex);
                }
            });
        }


        public Task<int> Count()
        {
            return Task.Run(() =>
            {
                try
                {
                    return Repo.Count;
                }
                catch (Exception ex)
                {
                    throw new ReadFailedException(_type, ex);
                }
            });
        }

        public Task<bool> Contains(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default(T)))
            {
                throw new ReadFailedException(_type, "Default values are not allowed in the repository");
            }

            return ContainsKey(GetKey(item));
        }

        public Task<bool> ContainsKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ReadFailedException(_type, "The key cannot be null or empty");
            }

            return Task.Run(() =>
            {
                try
                {
                    return Repo.ContainsKey(key);
                }
                catch (Exception ex)
                {
                    throw new ReadFailedException(_type, ex);
                }
            });
        }

        #endregion

        #region Update

        public Task Update(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default(T)))
            {
                throw new UpdateFailedException(_type, "Default values are not allowed in the repository");
            }

            return Task.Run(() =>
            {
                try
                {
                    var key = GetKey(item);

                    CheckKey(key);

                    var isRetrieved = Repo.TryGetValue(key, out var old);

                    if (!isRetrieved)
                        throw new UpdateFailedException(_type);

                    var updated = Repo.TryUpdate(key, item, old);

                    if (!updated)
                        throw new UpdateFailedException(_type,
                            "Could not update entity, because its value was changed in the repository");
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
                catch (UpdateFailedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new UpdateFailedException(_type, ex);
                }
            });
        }

        public Task Update(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new UpdateFailedException(_type, "The item collection was null");
            }

            return DoWithMany(items, Update);
        }

        #endregion

        #region Delete

        public Task Delete(T item)
        {
            if (EqualityComparer<T>.Default.Equals(item, default(T)))
            {
                throw new DeleteFailedException(_type, "Default values are not allowed in the repository");
            }

            return DeleteByKey(GetKey(item));
        }

        public Task Delete(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new DeleteFailedException(_type, "The item collection was null");
            }

            return DoWithMany(items, Delete);
        }

        protected Task Delete(Func<T, bool> filter)
        {
            if (filter == null)
            {
                throw new DeleteFailedException(_type, "The filter cannot be null");
            }

            return DoWithMany(Repo.Values.Where(filter), Delete);
        }


        public Task DeleteByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DeleteFailedException(_type, "The key cannot be null or empty");
            }

            return Task.Run(() =>
            {
                try
                {
                    CheckKey(key);

                    var removed = Repo.TryRemove(key, out _);

                    if (!removed)
                        throw new DeleteFailedException(_type, "The item cannot be removed from the repository");
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
                catch (DeleteFailedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DeleteFailedException(_type, ex);
                }
            });
        }

        public Task DeleteByKeys(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new DeleteFailedException(_type, "The key collection was null");
            }

            return DoWithMany(keys, DeleteByKey);
        }

        #endregion

        #endregion


        private void CheckKey(string key)
        {
            if (!Repo.ContainsKey(key))
                throw new KeyNotFoundException($"The key '{key}' was not found in the {_type.Name} repository");
        }

        private static Task DoWithMany<TItem>(IEnumerable<TItem> collection, Func<TItem, Task> operation)
        {
            return Task.Run(() => Parallel.ForEach(collection, item => operation(item).GetAwaiter().GetResult()));
        }
    }
}