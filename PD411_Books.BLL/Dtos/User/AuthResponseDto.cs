namespace PD411_Books.BLL.Dtos.User
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public UserDto User { get; set; } = null!;
    }
}
