using System;

namespace WhiteRaven.Repository.Contract.Exceptions
{
    public class ReadFailedException : Exception
    {
        public ReadFailedException(Type repositoryItemType)
            : base($"Error during Read operation in {repositoryItemType.Name} repository.")
        {
        }

        public ReadFailedException(Type repositoryItemType, string details)
            : base($"Error during Read operation in {repositoryItemType.Name} repository. Details: {details}")
        {
        }

        public ReadFailedException(Type repositoryItemType, Exception innerException)
            : base($"Error during Read operation in {repositoryItemType.Name} repository.", innerException)
        {
        }

        public ReadFailedException(Type repositoryItemType, string details, Exception innerException)
            : base($"Error during Read operation in {repositoryItemType.Name} repository. Details: {details}", innerException)
        {
        }
    }
}