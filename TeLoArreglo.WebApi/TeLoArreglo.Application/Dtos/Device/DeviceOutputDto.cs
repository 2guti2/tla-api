using Abp.Domain.Entities;

namespace TeLoArreglo.Application.Dtos.Device
{
    public class DeviceOutputDto : Entity
    {
        public string DeviceToken { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}
