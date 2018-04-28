using Abp.Domain.Services;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common
{
    public interface IPermissionManager : IDomainService
    {
        bool HasPermission(User user, Action action);
    }
}