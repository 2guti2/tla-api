﻿using System.Collections.Generic;
using TeLoArreglo.Application.Dtos.User;

namespace TeLoArreglo.Application.Users
{
    public interface IUserAppService
    {
        LoggedUserDto Login(UserLoginDto userDto);
        LoggedUserDto Logout(string token);
        UserSignUpDtoOutput CreateUser(string token, UserSignUpDtoInput user);
        List<ActionDto> GetActionsOf(string token);
        UserSignUpDtoOutput BlockUser(string token, int id);
        List<UserSignUpDtoOutput> GetCrewMembers(string token);
        List<UserSignUpDtoOutput> GetAllUsers(string token);
    }
}
