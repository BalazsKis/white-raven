using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhiteRaven.Repository.Contract
{
    /// <summary>
    /// Generic contract interface for repositories
    /// </summary>
    /// <typeparam name="T">The type of the entities the repository contains</typeparam>
    public interface IRepository<T>
    {
        #region Create

        /// <summary>
        /// Inserts the given item into the repository
        /// </summary>
        /// <param name="item">The item to insert</param>
        Task Insert(T item);

        /// <summary>
        /// Inserts the given items into the repository
        /// </summary>
        /// <param name="items">The items to insert</param>
        Task Insert(IEnumerable<T> items);

        #endregion

        #region Read

        /// <summary>
        /// Returns an item from the repository by its unique key
        /// </summary>
        /// <param name="key">The key of the item to select</param>
        /// <returns>The query result</returns>
        Task<T> SelectByKey(string key);
        
        /// <summary>
        /// Returns all items from the repository
        /// </summary>
        /// <returns>All objects from the repository</returns>
        Task<IEnumerable<T>> SelectAll();

        /// <summary>
        /// Returns the number of entities currently stored in the repository
        /// </summary>
        /// <returns>The item count</returns>
        Task<int> Count();

        /// <summary>
        /// Returns whether an item is part of the collection.
        /// </summary>
        /// <param name="item">The item to search for in the repository</param>
        /// <returns>The 'contains' flag</returns>
        Task<bool> Contains(T item);

        /// <summary>
        /// Returns whether an item is part of the collection.
        /// </summary>
        /// <param name="key">The key of the item to search for in the repository</param>
        /// <returns>The 'contains' flag</returns>
        Task<bool> ContainsKey(string key);

        #endregion

        #region Update

        /// <summary>
        /// Updates the given item in the repository
        /// </summary>
        /// <param name="item">The item to update</param>
        Task Update(T item);

        /// <summary>
        /// Updates the given items in the repository
        /// </summary>
        /// <param name="items">The items to update</param>
        Task Update(IEnumerable<T> items);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the given item from the repository
        /// </summary>
        /// <param name="item">The item to delete</param>
        Task Delete(T item);

        /// <summary>
        /// Deletes the given items from the repository
        /// </summary>
        /// <param name="items">The items to delete</param>
        Task Delete(IEnumerable<T> items);

        /// <summary>
        /// Deletes an item from the repository by its unique key
        /// </summary>
        /// <param name="key">The key of the item to delete</param>
        Task DeleteByKey(string key);

        /// <summary>
        /// Deletes items from the repository by their unique key
        /// </summary>
        /// <param name="keys">The keys of the items to delete</param>
        Task DeleteByKeys(IEnumerable<string> keys);

        #endregion
    }
}