using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Abp.Domain.Entities;

namespace TeLoArreglo.Logic.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; } = false;

        public virtual List<Action> PermittedActions => new List<Action>();

        public static Expression<Func<User, bool>> EqualityExpression(User toCompare)
        {
            return u => u.Username.Equals(toCompare.Username) && u.Password.Equals(toCompare.Password);
        } 
    }
}
