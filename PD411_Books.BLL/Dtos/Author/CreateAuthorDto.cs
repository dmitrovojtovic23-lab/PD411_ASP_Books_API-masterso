using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PD411_Books.BLL.Dtos.Author
{
    public class CreateAuthorDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public IFormFile? Image { get; set; }
        public string? Country { get; set; }
    }
}
