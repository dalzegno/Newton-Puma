using AutoMapper;
using Logic.Models;
using PumaDbLibrary.Entities;

namespace Logic.MappingProfiles
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            // Vi slipper Translators med detta! Ställ krav med ForMember
            // dest = destinationsobjekt (UserDto)
            // src = ursprungsobjekt (User)
            CreateMap<User, UserDto>().ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email.ToLower()));

            // Skippar att sätta Id
            CreateMap<UserDto, User>().ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email.ToLower()))
                                      .ForMember(dest => dest.Id, option => option.Ignore())
                                      .ForAllMembers(options => options.DoNotAllowNull());

            CreateMap<AddUserDto, User>().ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email.ToLower()));
        }
    }
}
