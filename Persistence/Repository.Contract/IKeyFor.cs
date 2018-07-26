using System;

namespace WhiteRaven.Repository.Contract
{
    /// <summary>
    /// Generic wrapper for a key provider function
    /// </summary>
    /// <typeparam name="T">The type of the entity for which the enclosed function provides key</typeparam>
    public interface IKeyFor<in T>
    {
        /// <summary>
        /// The function that provides key for a specific entity type
        /// </summary>
        Func<T, string> KeyProvider { get; }
    }
}