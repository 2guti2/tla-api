using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Abp.Web;

namespace TeLoArreglo.WebApi
{
    public class WebApiApplication : AbpWebApplication<WebModule>
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
