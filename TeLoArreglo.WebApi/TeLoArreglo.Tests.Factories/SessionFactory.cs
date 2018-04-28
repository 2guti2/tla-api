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
    }
}
