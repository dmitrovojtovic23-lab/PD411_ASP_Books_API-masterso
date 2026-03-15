namespace PD411_Books.BLL.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
    }
}
