using Microsoft.AspNetCore.Mvc;
using PD411_Books.API.Extensions;
using PD411_Books.API.Settings;
using PD411_Books.BLL.Dtos.Book;
using PD411_Books.BLL.Services;

namespace PD411_Books.API.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly string _booksPath;

        public BookController(BookService bookService, IWebHostEnvironment environment)
        {
            _bookService = bookService;

            string rootPath = environment.ContentRootPath;
            _booksPath = Path.Combine(rootPath, StaticFilesSettings.StorageDir, StaticFilesSettings.BooksDir);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _bookService.GetAllAsync();
            return this.GetAction(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _bookService.GetByIdAsync(id);
            return this.GetAction(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateBookDto dto)
        {
            var response = await _bookService.CreateAsync(dto, _booksPath);
            return this.GetAction(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateBookDto dto)
        {
            var response = await _bookService.UpdateAsync(dto, _booksPath);
            return this.GetAction(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await _bookService.DeleteAsync(id, _booksPath);
            return this.GetAction(response);
        }
    }
}
