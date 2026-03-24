using Microsoft.AspNetCore.Mvc;
using PD411_Books.API.Extensions;
using PD411_Books.API.Settings;
using PD411_Books.BLL.Dtos.Author;
using PD411_Books.BLL.Dtos.Common;
using PD411_Books.BLL.Services;

namespace PD411_Books.API.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;
        private readonly string _authorsPath;

        public AuthorController(AuthorService authorService, IWebHostEnvironment environment)
        {
            _authorService = authorService;

            string rootPath = environment.ContentRootPath;
            _authorsPath = Path.Combine(rootPath, StaticFilesSettings.StorageDir, StaticFilesSettings.AuthorsDir);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDto pagination)
        {
            var response = await _authorService.GetAllAsync(pagination);
            return this.GetAction(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _authorService.GetByIdAsync(id);
            return this.GetAction(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateAuthorDto dto)
        {
            var response = await _authorService.CreateAsync(dto, _authorsPath);
            return this.GetAction(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateAuthorDto dto)
        {
            var response = await _authorService.UpdateAsync(dto, _authorsPath);
            return this.GetAction(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _authorService.DeleteAsync(id, _authorsPath);
            return this.GetAction(response);
        }
    }
}
