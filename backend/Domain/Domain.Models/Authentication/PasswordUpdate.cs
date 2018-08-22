namespace WhiteRaven.Domain.Models.Authentication
{
    public class PasswordUpdate
    {
        public string OldPassword { get; }
        public string NewPassword { get; }

        public PasswordUpdate(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}