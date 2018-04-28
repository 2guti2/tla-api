using System;
using Abp.Domain.Entities;

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

        public User User { get; set; }  
        public string Token { get; set; }
    }
}
