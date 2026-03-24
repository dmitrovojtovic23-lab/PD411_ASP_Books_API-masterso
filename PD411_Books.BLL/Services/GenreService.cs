using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PD411_Books.BLL.Dtos.Genre;
using PD411_Books.BLL.Dtos.Common;
using PD411_Books.DAL.Entities;
using PD411_Books.DAL.Repositories;

namespace PD411_Books.BLL.Services
{
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreService(GenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }


        public async Task<ServiceResponse> CreateAsync(CreateGenreDto dto)
        {
            if(await _genreRepository.IsExistAsync(dto.Name))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр '{dto.Name}' вже існує"
                };
            }

            var entity = _mapper.Map<GenreEntity>(dto);

            var res = await _genreRepository.CreateAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося створити жанр"
                };
            }

            return new ServiceResponse
            {
                Message = $"Жанр '{dto.Name}' успішно створено",
                Payload = _mapper.Map<GenreDto>(entity)
            };
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateGenreDto dto)
        {
            var entity = await _genreRepository.GetByIdAsync(dto.Id);

            if(entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр '{dto.Name}' не існує"
                };
            }

            if (await _genreRepository.IsExistAsync(dto.Name))
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр '{dto.Name}' вже існує"
                };
            }

            string oldName = entity.Name;
            entity = _mapper.Map(dto, entity);

            var res = await _genreRepository.UpdateAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося оновити жанр"
                };
            }

            return new ServiceResponse
            {
                Message = $"Жанр '{oldName}' успішно оновлений",
                Payload = _mapper.Map<GenreDto>(entity)
            };
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр з id '{id}' не існує"
                };
            }

            var res = await _genreRepository.DeleteAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося видалити жанр"
                };
            }

            return new ServiceResponse
            {
                Message = $"Жанр '{entity.Name}' успішно видалений",
                Payload = _mapper.Map<GenreDto>(entity)
            };
        }

        public async Task<ServiceResponse> DeleteAsync(string name)
        {
            var entity = await _genreRepository.GetByNameAsync(name);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр '{name}' не існує"
                };
            }

            var res = await _genreRepository.DeleteAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося видалити жанр"
                };
            }

            return new ServiceResponse
            {
                Message = $"Жанр '{entity.Name}' успішно видалений",
                Payload = _mapper.Map<GenreDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр з id '{id}' не існує"
                };
            }

            return new ServiceResponse
            {
                Message = "Жанр успішно отриманий",
                Payload = _mapper.Map<GenreDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            var entity = await _genreRepository.GetByNameAsync(name);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Жанр '{name}' не існує"
                };
            }

            return new ServiceResponse
            {
                Message = "Жанр успішно отриманий",
                Payload = _mapper.Map<GenreDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetAllAsync(PaginationDto pagination)
        {
            var query = _genreRepository.Genres;

            var totalCount = await query.CountAsync();

            var entities = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<GenreDto>>(entities);

            var paginatedResponse = new PaginatedResponseDto<GenreDto>
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
                Message = "Жанри отримано",
                Payload = paginatedResponse
            };
        }
    }
}
