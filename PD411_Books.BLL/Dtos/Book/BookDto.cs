using PD411_Books.BLL.Dtos.Author;
using PD411_Books.BLL.Dtos.Genre;

namespace PD411_Books.BLL.Dtos.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public float Rating { get; set; } = 0f;
        public int Pages { get; set; } = 0;
        public int PublishYear { get; set; } = DateTime.UtcNow.Year;
        public int? AuthorId { get; set; }
        public AuthorDto? Author { get; set; }
        public List<GenreDto> Genres { get; set; } = [];
    }
}
