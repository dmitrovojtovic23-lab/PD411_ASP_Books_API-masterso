using System.ComponentModel.DataAnnotations;

namespace PD411_Books.BLL.Dtos.Genre
{
    public class CreateGenreDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
