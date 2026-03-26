namespace PD411_Books.DAL.Entities
{
    public class RefreshTokenEntity : BaseEntity
    {
        public required string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}
