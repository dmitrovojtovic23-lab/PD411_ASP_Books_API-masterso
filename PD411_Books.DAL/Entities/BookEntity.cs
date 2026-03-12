namespace PD411_Books.DAL.Entities
{
    public class BookEntity : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float Rating { get; set; } = 0f;
        public int Pages { get; set; } = 0;
        public int PublishYear { get; set; } = DateTime.UtcNow.Year;

        public int? AuthorId { get; set; }
        public AuthorEntity? Author { get; set; }

        public List<GenreEntity> Genres { get; set; } = [];
    }
}
