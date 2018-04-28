using System;
using Abp.Modules;
using Abp.TestBase;
using TeLoArreglo.Application;
using TeLoArreglo.Logic;
using TeLoArreglo.Repository;

namespace TeLoArreglo.Tests.Application
{
    [DependsOn(
        typeof(AbpTestBaseModule), 
        typeof(ApplicationModule),
        typeof(RepositoryModule),
        typeof(LogicModule))]
    public class ApplicationTestModule : AbpModule
    {
        
    }
}
