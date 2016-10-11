using AutoMapper;
using BusinessEntities;
using DataModel;

namespace BusinessServices
{
    public class BoongalooProfile : Profile
    {
        public BoongalooProfile()
        {
            CreateMap<RadiusDto, Radius>();
            CreateMap<Radius, RadiusDto>();

            CreateMap<AreaDto, Area>();
            CreateMap<Area, AreaDto>()
                .ForMember(m => m.Groups, opt => opt.Ignore());

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Language, LanguageDto>();
            CreateMap<LanguageDto, Language>();

            CreateMap<Group, GroupDto>()
                .ForMember(m => m.Tags, opt => opt.Ignore())
                .ForMember(m => m.Areas, opt => opt.Ignore())
                .ForMember(m => m.Users, opt => opt.Ignore());
            CreateMap<GroupDto, Group>();

            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
        }
    }
}
