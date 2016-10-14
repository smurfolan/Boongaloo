using AutoMapper;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;

namespace Boongaloo.API.Automapper
{
    public class BoongalooProfile : Profile
    {
        public BoongalooProfile()
        {
            //CreateMap<RadiusDto, Radius>();
            //CreateMap<Radius, RadiusDto>();

            CreateMap<AreaDto, Area>();
            CreateMap<Area, AreaDto>();

            //CreateMap<User, UserDto>()
            //    .ForMember(m => m.Groups, opt => opt.Ignore())
            //    .ForMember(m => m.Languages, opt => opt.Ignore());

            //CreateMap<UserDto, User>();

            //CreateMap<Language, LanguageDto>();
            //CreateMap<LanguageDto, Language>();

            //CreateMap<Group, GroupDto>()
            //    .ForMember(m => m.Tags, opt => opt.Ignore())
            //    .ForMember(m => m.Areas, opt => opt.Ignore())
            //    .ForMember(m => m.Users, opt => opt.Ignore());
            //CreateMap<GroupDto, Group>();

            //CreateMap<Tag, TagDto>();
            //CreateMap<TagDto, Tag>();
        }
    }
}