using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using TeLoArreglo.Logic;

namespace TeLoArreglo.Repository
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(LogicModule))]
    public class RepositoryModule : AbpModule
    {
        public override void PreInitialize()
        {
           Configuration.DefaultNameOrConnectionString = "TeLoArregloContext";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
