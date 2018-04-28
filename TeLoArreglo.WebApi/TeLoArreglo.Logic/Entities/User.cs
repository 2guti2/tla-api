using System.Collections.Generic;
using Abp.Domain.Entities;

namespace TeLoArreglo.Logic.Entities
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; } = false;

        public virtual List<Action> PermittedActions => new List<Action>();
    }
}
