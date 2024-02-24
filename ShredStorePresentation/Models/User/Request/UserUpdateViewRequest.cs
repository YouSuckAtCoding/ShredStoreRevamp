namespace ShredStorePresentation.Models.User.Request
{
    public class UserUpdateViewRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
