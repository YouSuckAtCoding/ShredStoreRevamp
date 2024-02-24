namespace ShredStorePresentation.Models.User.Request
{
    public class UserResetPasswordViewRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
