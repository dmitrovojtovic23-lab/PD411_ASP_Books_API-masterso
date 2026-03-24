using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PD411_Books.BLL.Dtos.Book;
using PD411_Books.BLL.Dtos.Common;
using PD411_Books.DAL.Entities;
using PD411_Books.DAL.Repositories;

namespace PD411_Books.BLL.Services
{
    public class BookService
    {
        private readonly BookRepository _bookRepository;
        private readonly GenreRepository _genreRepository;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public BookService(BookRepository bookRepository, GenreRepository genreRepository, ImageService imageService, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _genreRepository = genreRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(CreateBookDto dto, string imagesPath)
        {
            var entity = _mapper.Map<BookEntity>(dto);

            if (dto.Image != null && !string.IsNullOrEmpty(imagesPath))
            {
                ServiceResponse response = await _imageService.SaveAsync(dto.Image, imagesPath);

                if (!response.Success)
                {
                    return response;
                }

                entity.Image = response.Payload!.ToString()!;
            }

            if (dto.GenreIds != null && dto.GenreIds.Any())
            {
                var genres = await _genreRepository.GetAll()
                    .Where(g => dto.GenreIds.Contains(g.Id))
                    .ToListAsync();
                entity.Genres = _mapper.Map<List<GenreEntity>>(genres);
            }

            bool res = await _bookRepository.CreateAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося додати книгу"
                };
            }

            return new ServiceResponse
            {
                Message = $"Книга '{entity.Title}' успішно додана",
                Payload = _mapper.Map<BookDto>(entity)
            };
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateBookDto dto, string imagesPath)
        {
            var entity = await _bookRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Книгу з id {dto.Id} не існує"
                };
            }

            string oldTitle = entity.Title;
            entity = _mapper.Map(dto, entity);

            if (dto.Image != null && !string.IsNullOrEmpty(imagesPath))
            {
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    string imagePath = Path.Combine(imagesPath, entity.Image);
                    var deleteResponse = _imageService.Delete(imagePath);

                    if (!deleteResponse.Success)
                    {
                        return deleteResponse;
                    }
                }

                var saveResponse = await _imageService.SaveAsync(dto.Image, imagesPath);

                if (!saveResponse.Success)
                {
                    return saveResponse;
                }

                entity.Image = saveResponse.Payload!.ToString()!;
            }
            if (dto.GenreIds != null)
            {
                var genres = await _genreRepository.GetAll()
                    .Where(g => dto.GenreIds.Contains(g.Id))
                    .ToListAsync();
                entity.Genres = _mapper.Map<List<GenreEntity>>(genres);
            }

            bool res = await _bookRepository.UpdateAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Не вдалося оновити книгу"
                };
            }

            return new ServiceResponse
            {
                Message = $"Книга '{oldTitle}' успішно оновлена",
                Payload = _mapper.Map<BookDto>(entity)
            };
        }

        public async Task<ServiceResponse> DeleteAsync(int id, string imagesPath)
        {
            var entity = await _bookRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Книгу з id {id} не існує"
                };
            }

            if (!string.IsNullOrEmpty(entity.Image))
            {
                string imagePath = Path.Combine(imagesPath, entity.Image);
                var response = _imageService.Delete(imagePath);

                if (!response.Success)
                {
                    return response;
                }
            }

            bool res = await _bookRepository.DeleteAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Не вдалося видалити книгу"
                };
            }

            return new ServiceResponse
            {
                Message = $"Книга '{entity.Title}' успішно видалена",
                Payload = _mapper.Map<BookDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _bookRepository.GetAll()
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Книгу з id {id} не існує"
                };
            }

            return new ServiceResponse
            {
                Message = "Книга успішно отримана",
                Payload = _mapper.Map<BookDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetAllAsync(PaginationDto pagination)
        {
            var query = _bookRepository.GetAll()
                .Include(b => b.Author)
                .Include(b => b.Genres);

            var totalCount = await query.CountAsync();

            var entities = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<BookDto>>(entities);

            var paginatedResponse = new PaginatedResponseDto<BookDto>
            {
                Data = dtos,
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize),
                HasNextPage = pagination.Page * pagination.PageSize < totalCount,
                HasPreviousPage = pagination.Page > 1
            };

            return new ServiceResponse
            {
                Message = "Книги отримано",
                Payload = paginatedResponse
            };
        }
    }
}
