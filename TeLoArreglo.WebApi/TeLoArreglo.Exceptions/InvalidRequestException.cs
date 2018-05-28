using System.Net;

namespace TeLoArreglo.Exceptions
{
    public class InvalidRequestException : CommonErrorException
    {
        public InvalidRequestException() : base(HttpStatusCode.BadRequest, "Api key not found.")
        {
        }
    }
}
