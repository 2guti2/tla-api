using Abp.AutoMapper;
using TeLoArreglo.Application.Dtos.Error;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Application.Dtos
{
    public static class DtoMappings
    {
        public static void Map(IAbpAutoMapperConfiguration configuration)
        {
            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<UserLoginDto, Logic.Entities.User>()
            );

            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<UserSignUpDtoInput, Logic.Entities.User>()
            );

            configuration.Configurators.Add(cfg => 
                cfg.CreateMap<Logic.Entities.User, UserSignUpDtoOutput>()
                    .ForMember(e => e.Role, opt => opt.MapFrom(src => src.GetType().Name))
                    );

            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<Logic.Entities.Action, ActionDto>()
            );

            configuration.Configurators.Add(cfg => 
                cfg.CreateMap<Session, TokenDto>()
            );

            configuration.Configurators.Add(cfg => 
                cfg.CreateMap<ValidationError, ValidationErrorDto>()
            );
        }
    }
}
