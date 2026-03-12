using AutoMapper;
using PD411_Books.BLL.Dtos.Genre;
using PD411_Books.DAL.Entities;

namespace PD411_Books.BLL.MapperProfiles
{
    public class GenreMapperProfile : Profile
    {
        public GenreMapperProfile()
        {
            // GenreEntity -> GenreDto
            CreateMap<GenreEntity, GenreDto>();

            // CreateGenreDto -> GenreEntity
            CreateMap<CreateGenreDto, GenreEntity>();

            // UpdateGenreDto -> GenreEntity
            CreateMap<UpdateGenreDto, GenreEntity>();
        }
    }
}
