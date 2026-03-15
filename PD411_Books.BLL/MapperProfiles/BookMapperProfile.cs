using AutoMapper;
using PD411_Books.BLL.Dtos.Book;
using PD411_Books.DAL.Entities;

namespace PD411_Books.BLL.MapperProfiles
{
    public class BookMapperProfile : Profile
    {
        public BookMapperProfile()
        {
            CreateMap<BookEntity, BookDto>();

            CreateMap<CreateBookDto, BookEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore());

            CreateMap<UpdateBookDto, BookEntity>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}
