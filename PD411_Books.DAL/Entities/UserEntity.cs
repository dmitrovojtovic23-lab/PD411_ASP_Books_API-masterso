using System.ComponentModel.DataAnnotations;

namespace PD411_Books.DAL.Entities
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationSent { get; set; }
        public List<RoleEntity> Roles { get; set; } = [];
        public List<RefreshTokenEntity> RefreshTokens { get; set; } = [];
    }
}
