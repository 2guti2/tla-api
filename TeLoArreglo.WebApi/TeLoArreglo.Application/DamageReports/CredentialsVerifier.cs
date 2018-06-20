using Abp.Domain.Repositories;
using TeLoArreglo.Application.Users;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Common;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Application.DamageReports
{
    public class CredentialsVerifier
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<Session> _sessionsRepository;
        
        public CredentialsVerifier(
            IPermissionManager permissionManager,
            IRepository<Session> sessionsRepository)
        {
            _permissionManager = permissionManager;
            _sessionsRepository = sessionsRepository;
        }

        public void VerifyCredentialsForModifyingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.ModifyDamage))
                throw new ForbiddenAccessException();
        }

        public void VerifyCredentialsForDamageReporting(string token)
        {
            UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            //we don't check privileges because everyone can report damages
        }

        public void VerifyCredentialsForQueryingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.QueryDamages))
                throw new ForbiddenAccessException();
        }

        public void VerifyCredentialsForRepairingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.RepairDamage))
                throw new ForbiddenAccessException();
        }

        public void VerifyCredentialsForDeletingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if(!_permissionManager.HasPermission(executingUser, Action.DeleteDamage))
                throw new ForbiddenAccessException();
        }
    }
}
