using System.Collections.Generic;

namespace TeLoArreglo.Application.Dtos.User
{
    public class LoggedUserDto
    {
        public string Token { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public List<ActionDto> PermittedActions { get; set; }
    }
}
