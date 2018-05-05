using System;
using System.Net;

namespace TeLoArreglo.Application.Exceptions
{
    public class InvalidRequestException : CommonErrorException
    {
        public InvalidRequestException() : base(HttpStatusCode.BadRequest, "Api key not found.")
        {
        }
    }
}
