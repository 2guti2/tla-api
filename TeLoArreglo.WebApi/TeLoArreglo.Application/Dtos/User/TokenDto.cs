using System.Collections.Generic;

namespace TeLoArreglo.Application.Dtos.User
{
    public class TokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public List<ActionDto> PermittedActions { get; set; }
    }
}
