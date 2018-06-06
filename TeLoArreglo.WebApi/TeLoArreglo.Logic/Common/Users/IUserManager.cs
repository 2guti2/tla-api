using Abp.Domain.Services;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common.Users
{
    public interface IUserManager : IDomainService
    {
        void ValidateUser(User user);
        void ValidateSession(Session session);
    }
}