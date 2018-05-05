using System;
using System.Net;

namespace TeLoArreglo.Application.Exceptions
{
    public class NotLoggedInException : CommonErrorException
    {
        public NotLoggedInException() : base(HttpStatusCode.Unauthorized, "Not Logged In.")
        {
            
        }
    }
}
