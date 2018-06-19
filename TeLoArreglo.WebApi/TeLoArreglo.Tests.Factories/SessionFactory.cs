using System;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Tests.Factories
{
    public class SessionFactory
    {
        public static Session NewSession()
        {
            return new Session
            {
                User = UserFactory.NewUser(),
                Token = "a"
            };
        }

        public static Session NewSessionWithUser(User user)
        {
            return new Session
            {
                User = user,
                Token = Guid.NewGuid().ToString()
            };
        }
    }
}
