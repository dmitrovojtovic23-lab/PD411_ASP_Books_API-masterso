using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PD411_Books.BLL.Dtos.Book
{
    public class CreateBookDto
    {
        [Required]
        public required string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public float Rating { get; set; } = 0f;
        public int Pages { get; set; } = 0;
        public int PublishYear { get; set; } = DateTime.UtcNow.Year;
        public int? AuthorId { get; set; }
        public List<int>? GenreIds { get; set; }
    }
}
