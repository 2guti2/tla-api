using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Tests.Factories
{
    public class UserFactory
    {
        public static User NewUser()
        {
            return new User { Username = "a", Password = "a" };
        }
    }
}
