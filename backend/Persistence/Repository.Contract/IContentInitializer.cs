using System;
using System.Threading.Tasks;

namespace WhiteRaven.Repository.Contract
{
    /// <summary>
    /// Contains a single method to fill a repository with data
    /// </summary>
    public interface IContentInitializer
    {
        /// <summary>
        /// Loads the content from a source (by using the given connection string) to the given repository
        /// </summary>
        /// <typeparam name="T">The type of the object in the repository</typeparam>
        /// <param name="connectionString">Connection string to the data source</param>
        /// <param name="repository">The target repository to load the objects into</param>
        Task LoadContent<T>(string connectionString, IRepository<T> repository);
    }
}