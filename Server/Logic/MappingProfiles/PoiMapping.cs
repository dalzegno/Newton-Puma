using AutoMapper;
using Logic.Enums;
using Logic.Models;
using PumaDbLibrary.Entities;

namespace Logic.MappingProfiles
{
    public class PoiMapping : Profile
    {
        public PoiMapping()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestDto, PointOfInterest>().ForAllMembers(options => options.DoNotAllowNull());


            CreateMap<Comment, CommentDto>().ForMember(dest => dest.UserDisplayName, option => option.MapFrom(src => src.User.DisplayName));
            CreateMap<CommentDto, Comment>();
            CreateMap<AddCommentDto, Comment>().ForMember(dest => dest.Id, option => option.Ignore());

            CreateMap<Position, PositionDto>();
            CreateMap<PositionDto, Position>().ForMember(dest => dest.Id, option => option.Ignore());
            
            CreateMap<Grading, GradingDto>().ForMember(dest => dest.GradeType, option => option.MapFrom(src => (GradeType)src.GradeType));
            CreateMap<GradingDto, Grading>().ForMember(dest => dest.GradeType, option => option.MapFrom(src => (int)src.GradeType))
                                            .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            CreateMap<PoiTag, PoiTagDto>();
            
            CreateMap<Tag, TagDto>();
            
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>().ForMember(dest => dest.Id, option => option.Ignore());
        }
    }
}
