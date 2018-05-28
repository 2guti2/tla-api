using System.Net;

namespace TeLoArreglo.Exceptions
{
    public class NotLoggedInException : CommonErrorException
    {
        public NotLoggedInException() : base(HttpStatusCode.Unauthorized, "Not Logged In.")
        {
            
        }
    }
}
