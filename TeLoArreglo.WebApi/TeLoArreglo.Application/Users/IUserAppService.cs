using TeLoArreglo.Application.Dtos.User;

namespace TeLoArreglo.Application.Users
{
    public interface IUserAppService
    {
        string Login(UserLoginDto userDto);
        void Logout(string token);
    }
}
