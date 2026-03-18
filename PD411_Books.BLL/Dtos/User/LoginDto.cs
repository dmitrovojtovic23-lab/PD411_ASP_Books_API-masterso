using System.ComponentModel.DataAnnotations;

namespace PD411_Books.BLL.Dtos.User
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
