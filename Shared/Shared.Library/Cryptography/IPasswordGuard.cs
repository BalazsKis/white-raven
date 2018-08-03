namespace WhiteRaven.Shared.Library.Cryptography
{
    public interface IPasswordGuard
    {
        bool IsUserPasswordValid(string savedPasswordHash, string enteredPassword);
        string GeneratePasswordHash(string password);
    }
}