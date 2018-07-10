using System;
using Abp.Domain.Repositories;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common.Users
{
    public class UserManager : IUserManager
    {
        public void ValidateUser(User user)
        {
            if (user == null || user.IsBlocked)
                throw new UnauthorizedAccessException();
        }

        public void ValidateSession(Session session)
        {
            if (session == null)
                throw new NotLoggedInException();
        }

        public void BlockUser(User user)
        {
            user.IsBlocked = true;
        }
    }
}
