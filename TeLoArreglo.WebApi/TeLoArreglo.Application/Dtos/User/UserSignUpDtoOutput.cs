using Abp.Domain.Entities;

namespace TeLoArreglo.Application.Dtos.User
{
    public class UserSignUpDtoOutput : Entity
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
