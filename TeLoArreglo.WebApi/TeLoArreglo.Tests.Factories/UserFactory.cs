using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Tests.Factories
{
    public class UserFactory
    {
        public static User NewUser()
        {
            return new User { Username = "a", Password = "a" };
        }

        public static Admin NewAdmin()
        {
            return new Admin {Username = "admin", Password = "admin"};
        }

        public static UserSignUpDtoInput NewSignUpDtoInputForAdmin()
        {
            return new UserSignUpDtoInput
            {
                Password = "admin",
                Role = "Admin",
                Username = "admin"
            };
        }
    }
}
