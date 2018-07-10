using System.Reflection;
using Abp.Modules;

namespace TeLoArreglo.Logic
{
    public class LogicModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
