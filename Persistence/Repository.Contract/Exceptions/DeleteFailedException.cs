using System;

namespace WhiteRaven.Repository.Contract.Exceptions
{
    public class DeleteFailedException : Exception
    {
        public DeleteFailedException(Type repositoryItemType)
            : base($"Error during Delete operation in {repositoryItemType.Name} repository.")
        {
        }

        public DeleteFailedException(Type repositoryItemType, string details)
            : base($"Error during Delete operation in {repositoryItemType.Name} repository. Details: {details}")
        {
        }

        public DeleteFailedException(Type repositoryItemType, Exception innerException)
            : base($"Error during Delete operation in {repositoryItemType.Name} repository.", innerException)
        {
        }

        public DeleteFailedException(Type repositoryItemType, string details, Exception innerException)
            : base($"Error during Delete operation in {repositoryItemType.Name} repository. Details: {details}", innerException)
        {
        }
    }
}