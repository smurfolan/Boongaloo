﻿using AutoMapper;
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
            CreateMap<Area, AreaDto>();
            CreateMap<User, UserDto>();
            CreateMap<Language, LanguageDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<Tag, TagDto>();
        }
    }
}