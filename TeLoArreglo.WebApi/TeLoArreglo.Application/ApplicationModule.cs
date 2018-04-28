using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using TeLoArreglo.Application.Dtos;
using TeLoArreglo.Logic;
using TeLoArreglo.Repository;

namespace TeLoArreglo.Application
{
    [DependsOn(typeof(AbpAutoMapperModule), typeof(LogicModule), typeof(RepositoryModule))]
    public class ApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            DtoMappings.Map(Configuration.Modules.AbpAutoMapper());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
