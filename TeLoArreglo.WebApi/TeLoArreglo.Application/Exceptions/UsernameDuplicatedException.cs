using System.Net;

namespace TeLoArreglo.Application.Exceptions
{
    public class UsernameDuplicatedException : CommonErrorException
    {
        public UsernameDuplicatedException() : base(HttpStatusCode.BadRequest, "Username already exists")
        {

        }
    }
}
