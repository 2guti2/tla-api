using System.Data.Common;
using System.Data.Entity;
using Abp.EntityFramework;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Repository
{
    public class TeLoArregloContext : AbpDbContext
    {
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Admin> Admins { get; set; }
        public virtual IDbSet<Crew> Crews { get; set; }
        public virtual IDbSet<Session> Sessions { get; set; }

        public TeLoArregloContext() : base("name=TeLoArregloContext")
        {

        }

        public TeLoArregloContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        public TeLoArregloContext(DbConnection existingConnection)
            : base(existingConnection, false)
        {

        }

        public TeLoArregloContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}
 