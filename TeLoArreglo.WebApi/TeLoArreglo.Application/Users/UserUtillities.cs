using System.Linq;
using Abp.Domain.Repositories;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Application.Users
{
    public class UserUtillities
    {
        public static User GetExecutingUserIfLoggedIn(string token, IRepository<Session> sessionRepository)
        {
            User executingUser = GetCurrentSession(token, sessionRepository)?.User;

            if (executingUser == null)
                throw new NotLoggedInException();

            return executingUser;
        }

        public static Session GetCurrentSession(string token, IRepository<Session> sessionRepository)
        {
            return sessionRepository.GetAllIncluding(s => s.User)
                .FirstOrDefault(Session.EqualityExpression(new Session(token)));
        }
    }
}
