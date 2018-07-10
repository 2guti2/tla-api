using System.Net;

namespace TeLoArreglo.Exceptions
{
    public class ForbiddenAccessException : CommonErrorException
    {
        public ForbiddenAccessException() : base(HttpStatusCode.Forbidden, "You don't have the sufficient privileges to perform this action.")
        {
            
        }
    }
}
