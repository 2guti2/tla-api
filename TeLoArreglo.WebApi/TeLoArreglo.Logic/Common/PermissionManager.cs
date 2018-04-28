using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common
{
    public class PermissionManager : IPermissionManager
    {
        public bool HasPermission(User user, Action action)
        {
            return user.PermittedActions.Contains(action);
        }
    }
}
