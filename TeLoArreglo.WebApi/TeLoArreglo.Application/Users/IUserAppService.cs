using System.Collections.Generic;
using TeLoArreglo.Application.Dtos.User;

namespace TeLoArreglo.Application.Users
{
    public interface IUserAppService
    {
        TokenDto Login(UserLoginDto userDto);
        TokenDto Logout(string token);
        UserSignUpDtoOutput CreateUser(string token, UserSignUpDtoInput user);
        List<ActionDto> GetActionsOf(string token);
    }
}
