using System;
using System.Linq.Expressions;
using Abp.Domain.Entities;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace TeLoArreglo.Logic.Entities
{
    public class Session : Entity
    {
        public Session()
        {
            
        }

        public Session(User user)
        {
            User = user;
            Token = Guid.NewGuid().ToString();
        }

        public Session(string token)
        {
            Token = token;
        }

        public User User { get; set; }  
        public string Token { get; set; }

        public static Expression<Func<Session, bool>> EqualityExpression(Session session)
        {
            return s => s.Token.Equals(session.Token);
        }

        public static Expression<Func<Session, bool>> EqualityExpressionByUser(User user)
        {
            return s => s.User.Id == user.Id;
        }
    }
}
