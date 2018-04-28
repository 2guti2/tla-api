using Abp.AutoMapper;
using TeLoArreglo.Application.Dtos.User;

namespace TeLoArreglo.Application.Dtos
{
    public static class DtoMappings
    {
        public static void Map(IAbpAutoMapperConfiguration configuration)
        {
            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<UserLoginDto, Logic.Entities.User>()
            );
        }
    }
}
