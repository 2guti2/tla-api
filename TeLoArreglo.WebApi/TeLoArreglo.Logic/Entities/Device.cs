using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace TeLoArreglo.Logic.Entities
{
    public class Device : Entity
    {
        [StringLength(999)]
        [Index(IsUnique = true)]
        public string DeviceToken { get; set; }
        public User User { get; set; }
    }
}
