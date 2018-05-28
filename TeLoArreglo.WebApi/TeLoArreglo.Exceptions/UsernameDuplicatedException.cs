using System.Net;

namespace TeLoArreglo.Exceptions
{
    public class UsernameDuplicatedException : CommonErrorException
    {
        public UsernameDuplicatedException() : base(HttpStatusCode.BadRequest, "Username already exists")
        {

        }
    }
}
