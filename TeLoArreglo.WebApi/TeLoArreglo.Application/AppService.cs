using System.Security.Principal;
using System.Web;
using Abp.Application.Services;

namespace TeLoArreglo.Application
{
    public class AppService : ApplicationService
    {
        protected IIdentity CurrentIdentity => HttpContext.Current.User.Identity;
    }
}
