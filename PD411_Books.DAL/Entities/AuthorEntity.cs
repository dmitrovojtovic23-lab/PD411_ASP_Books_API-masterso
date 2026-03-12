namespace PD411_Books.DAL.Entities
{
    public class AuthorEntity : BaseEntity
    {
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public string? Image { get; set; }
        public string? Country { get; set; }

        public List<BookEntity> Books { get; set; } = [];
    }
}
    