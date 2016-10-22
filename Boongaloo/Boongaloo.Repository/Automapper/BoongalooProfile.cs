using AutoMapper;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Automapper
{
    public class BoongalooProfile : Profile
    {
        public BoongalooProfile()
        {
            CreateMap<Tag, TagDto>();

            CreateMap<Area, AreaDto>();

            CreateMap<User, UserDto>();

            CreateMap<User, UserResponseDto>();

            CreateMap<Language, LanguageDto>();

            CreateMap<Group, GroupDto>();

            CreateMap<NewUserRequestDto, User>();

            CreateMap<UserNotificationSettings, UserNotificationSettingsResponseDto>();

            CreateMap<Area, AreaResponseDto>();
        }
    }
}
