using System.ComponentModel.DataAnnotations;

namespace PD411_Books.BLL.Dtos.Genre
{
    public class UpdateGenreDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
