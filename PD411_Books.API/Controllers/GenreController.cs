using Microsoft.AspNetCore.Mvc;
using PD411_Books.API.Extensions;
using PD411_Books.BLL.Dtos.Genre;
using PD411_Books.BLL.Services;
using System.Xml.Linq;

namespace PD411_Books.API.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _genreService.GetAllAsync();
            return this.GetAction(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _genreService.GetByIdAsync(id);
            return this.GetAction(response);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetAsync(string name)
        {
            var response = await _genreService.GetByNameAsync(name);
            return this.GetAction(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateGenreDto dto)
        {
            var response = await _genreService.CreateAsync(dto);
            return this.GetAction(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGenreDto dto)
        {
            var response = await _genreService.UpdateAsync(dto);
            return this.GetAction(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _genreService.DeleteAsync(id);
            return this.GetAction(response);
        }

        [HttpDelete("by-name/{name}")]
        public async Task<IActionResult> DeleteAsync(string name)
        {
            var response = await _genreService.DeleteAsync(name);
            return this.GetAction(response);
        }
    }
}
