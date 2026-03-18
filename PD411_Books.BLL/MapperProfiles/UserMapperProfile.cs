using AutoMapper;
using PD411_Books.BLL.Dtos.User;
using PD411_Books.DAL.Entities;

namespace PD411_Books.BLL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserEntity, UserDto>();

            CreateMap<RegisterDto, UserEntity>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmationToken, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmationSent, opt => opt.Ignore());
        }
    }
}
