using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PD411_Books.BLL.Dtos.Author;
using PD411_Books.BLL.Dtos.Common;
using PD411_Books.DAL.Entities;
using PD411_Books.DAL.Repositories;

namespace PD411_Books.BLL.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _authorRepository;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public AuthorService(AuthorRepository authorRepository, ImageService imageService, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(CreateAuthorDto dto, string imagesPath)
        {
            var entity = _mapper.Map<AuthorEntity>(dto);

            if (dto.Image != null && !string.IsNullOrEmpty(imagesPath))
            {
                ServiceResponse response = await _imageService.SaveAsync(dto.Image, imagesPath);

                if(!response.Success)
                {
                    return response;
                }

                entity.Image = response.Payload!.ToString()!;
            }

            bool res = await _authorRepository.CreateAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Не вдалося додати автора"
                };
            }

            return new ServiceResponse
            {
                Message = $"Автор '{entity.Name}' успішно доданий",
                Payload = _mapper.Map<AuthorDto>(entity)
            };
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateAuthorDto dto, string imagesPath)
        {
            var entity = await _authorRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Автора з id {dto.Id} не існує"
                };
            }

            string oldName = entity.Name;
            entity = _mapper.Map(dto, entity);

            if(dto.Image != null && !string.IsNullOrEmpty(imagesPath))
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

            bool res = await _authorRepository.UpdateAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Не вдалося оновити автора"
                };
            }

            return new ServiceResponse
            {
                Message = $"Автор '{oldName}' успішно оновлений",
                Payload = _mapper.Map<AuthorDto>(entity)
            };
        }

        public async Task<ServiceResponse> DeleteAsync(int id, string imagesPath)
        {
            var entity = await _authorRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Автор з id {id} не існує"
                };
            }

            if(!string.IsNullOrEmpty(entity.Image))
            {
                string imagePath = Path.Combine(imagesPath, entity.Image);
                var response = _imageService.Delete(imagePath);

                if(!response.Success)
                {
                    return response;
                }
            }

            bool res = await _authorRepository.DeleteAsync(entity);

            if (!res)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Не вдалося видалити автора"
                };
            }

            return new ServiceResponse
            {
                Message = $"Автор '{entity.Name}' успішно видалений",
                Payload = _mapper.Map<AuthorDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _authorRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = $"Автор з id {id} не існує"
                };
            }

            return new ServiceResponse
            {
                Message = "Автор успішно отриманий",
                Payload = _mapper.Map<AuthorDto>(entity)
            };
        }

        public async Task<ServiceResponse> GetAllAsync(PaginationDto pagination)
        {
            var query = _authorRepository.Authors;

            var totalCount = await query.CountAsync();

            var entities = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<AuthorDto>>(entities);

            var paginatedResponse = new PaginatedResponseDto<AuthorDto>
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
                Message = "Автори отримано",
                Payload = paginatedResponse
            };
        }
    }
}
