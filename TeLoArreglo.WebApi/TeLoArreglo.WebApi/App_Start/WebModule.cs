using System.Reflection;
using System.Web.Http;
using Abp.Modules;
using Abp.WebApi;
using TeLoArreglo.Application;

namespace TeLoArreglo.WebApi
{
    [DependsOn(typeof(AbpWebApiModule), typeof(ApplicationModule))]
    public class WebModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "GeneralRoute",
                routeTemplate: "api/{Controller}/{id}",
                defaults: new { controller = "Deploy", id = RouteParameter.Optional },
                constraints: new { action = @"^.+$" }
            );

            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
        }
    }
}