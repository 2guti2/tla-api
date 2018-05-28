using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common.Users
{
    public interface IUserManager
    {
        void ValidateUser(User user);
        void ValidateSession(Session session);
    }
}