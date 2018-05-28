using System;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common.Users
{
    public class UserManager : IUserManager
    {
        public void ValidateUser(User user)
        {
            if (user == null)
                throw new UnauthorizedAccessException();
        }

        public void ValidateSession(Session session)
        {
            if (session == null)
                throw new NotLoggedInException();
        }
    }
}
