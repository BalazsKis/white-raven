namespace WhiteRaven.Shared.Basics.Cryptography
{
    public interface IPasswordGuard
    {
        bool IsUserPasswordValid(string savedPasswordHash, string enteredPassword);
        string GeneratePasswordHash(string password);
    }
}