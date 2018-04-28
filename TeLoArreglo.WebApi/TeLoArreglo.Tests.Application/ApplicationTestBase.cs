using System;
using System.Data.Common;
using Abp.TestBase;
using Castle.MicroKernel.Registration;
using EntityFramework.DynamicFilters;
using TeLoArreglo.Repository;

namespace TeLoArreglo.Tests.Application
{
    public abstract class ApplicationTestBase : AbpIntegratedTestBase<ApplicationTestModule>
    {
        protected ApplicationTestBase()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

        protected override void PreInitialize()
        {
            //Fake DbConnection using Effort!
            LocalIocManager.IocContainer.Register(
            Component.For<DbConnection>()
            .UsingFactoryMethod(Effort.DbConnectionFactory.CreateTransient)
                .LifestyleSingleton()
                );

            base.PreInitialize();
        }

        public void UsingDbContext(Action<TeLoArregloContext> action)
        {
            using (var context = LocalIocManager.Resolve<TeLoArregloContext>())
            {
                context.DisableAllFilters();
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<T>(Func<TeLoArregloContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<TeLoArregloContext>())
            {
                context.DisableAllFilters();
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}
