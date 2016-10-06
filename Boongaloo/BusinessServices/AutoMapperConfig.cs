using AutoMapper;
using BusinessEntities;
using DataModel;

namespace BusinessServices
{
    public static class AutoMapperConfig
    {
        public static IMapperConfigurationExpression AddAdminMapping(
        this IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateMap<Area, AreaDto>();

            return configurationExpression;
        }
    }
}
