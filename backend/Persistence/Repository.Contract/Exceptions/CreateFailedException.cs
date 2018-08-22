using System;

namespace WhiteRaven.Repository.Contract.Exceptions
{
    public class CreateFailedException : Exception
    {
        public CreateFailedException(Type repositoryItemType)
            : base($"Error during Create operation in {repositoryItemType.Name} repository.")
        {
        }

        public CreateFailedException(Type repositoryItemType, string details)
            : base($"Error during Create operation in {repositoryItemType.Name} repository. Details: {details}")
        {
        }

        public CreateFailedException(Type repositoryItemType, Exception innerException)
            : base($"Error during Create operation in {repositoryItemType.Name} repository.", innerException)
        {
        }

        public CreateFailedException(Type repositoryItemType, string details, Exception innerException)
            : base($"Error during Create operation in {repositoryItemType.Name} repository. Details: {details}", innerException)
        {
        }
    }
}