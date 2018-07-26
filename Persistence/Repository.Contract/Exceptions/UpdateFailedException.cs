using System;

namespace WhiteRaven.Repository.Contract.Exceptions
{
    public class UpdateFailedException : Exception
    {
        public UpdateFailedException(Type repositoryItemType)
            : base($"Error during Update operation in {repositoryItemType.Name} repository.")
        {
        }

        public UpdateFailedException(Type repositoryItemType, string details)
            : base($"Error during Update operation in {repositoryItemType.Name} repository. Details: {details}")
        {
        }

        public UpdateFailedException(Type repositoryItemType, Exception innerException)
            : base($"Error during Update operation in {repositoryItemType.Name} repository.", innerException)
        {
        }

        public UpdateFailedException(Type repositoryItemType, string details, Exception innerException)
            : base($"Error during Update operation in {repositoryItemType.Name} repository. Details: {details}", innerException)
        {
        }
    }
}