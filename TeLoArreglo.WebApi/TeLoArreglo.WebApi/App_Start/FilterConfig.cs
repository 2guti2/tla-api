using System.Web.Http.Filters;

namespace TeLoArreglo.WebApi
{
    public class FilterConfig
    {
        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ExceptionHandlingAttribute());
        }
    }
}
