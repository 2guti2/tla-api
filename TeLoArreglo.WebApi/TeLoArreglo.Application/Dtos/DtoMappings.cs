using System.Runtime.Remoting.Channels;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Application.Dtos.Device;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Common;
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
                    .ForMember(dto => dto.Role, attribute => attribute.MapFrom(src => src.GetType().Name))
                    );

            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<Action, ActionDto>()
            );

            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<Session, LoggedUserDto>()
                .ForMember(dto => dto.PermittedActions, attribute => attribute.MapFrom(src => src.User.PermittedActions))
                .ForMember(dto => dto.Id, attribute => attribute.MapFrom(src => src.User.Id))
                .ForMember(dto => dto.Username, attribute => attribute.MapFrom(src => src.User.Username))
            );

            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<ValidationError, ValidationErrorDto>()
            );

            configuration.Configurators.Add(cfg =>
                cfg.CreateMap<Logic.Entities.Media, MediaDto>()
                .ForMember(dto => dto.MediaType, attribute => attribute.MapFrom(src => src.MediaType.ToString()))
                );

            configuration.Configurators.Add(cfg =>
                {
                    cfg.CreateMap<MediaInputDto, Logic.Entities.Media>();
                    cfg.CreateMap<Logic.Entities.Media, Entity>();
                    cfg.CreateMap<Logic.Entities.DamageReport, DamageReportOutputDto>();
                    cfg.CreateMap<DamageReportInputDto, Logic.Entities.DamageReport>();
                    cfg.CreateMap<DamageReportRepairDto, Logic.Entities.DamageReport>();
                    cfg.CreateMap<GeoCoordinateDto, GeoCoordinate>().ReverseMap();
                }
            );

            configuration.Configurators.Add(cfg =>
                {
                    cfg.CreateMap<Logic.Entities.DamageReport, DamageReportCompleteOutputDto>()
                        .ForMember(dto => dto.Username, attribute => attribute.MapFrom(src => src.User.Username))
                        .ForMember(dto => dto.UserId, attribute => attribute.MapFrom(src => src.User.Id));
                    cfg.CreateMap<DamageStatusDto, DamageStatus>();
                    cfg.CreateMap<DamageReportPriorityDto, DamagePriority>().ReverseMap();
                    cfg.CreateMap<ModifyDamageReportDto, Logic.Entities.DamageReport>();
                }
            );

            configuration.Configurators.Add(cfg =>
                {
                    cfg.CreateMap<DeviceInputDto, Logic.Entities.Device>();
                    cfg.CreateMap<Logic.Entities.Device, DeviceOutputDto>()
                        .ForMember(dto => dto.Username, attribute => attribute.MapFrom(src => src.User.Username))
                        .ForMember(dto => dto.UserId, attribute => attribute.MapFrom(src => src.User.Id));
                }
            );
        }
    }
}
