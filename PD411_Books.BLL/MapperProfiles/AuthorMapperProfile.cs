using AutoMapper;
using PD411_Books.BLL.Dtos.Author;
using PD411_Books.DAL.Entities;

namespace PD411_Books.BLL.MapperProfiles
{
    public class AuthorMapperProfile : Profile
    {
        public AuthorMapperProfile()
        {
            // AuthorEntity -> AuthorDto
            CreateMap<AuthorEntity, AuthorDto>();

            // CreateAuthorDto -> AuthorEntity
            CreateMap<CreateAuthorDto, AuthorEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // UpdateAuthorDto -> AuthorEntity
            CreateMap<UpdateAuthorDto, AuthorEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
