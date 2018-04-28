using System.Web.Http;
using Abp.WebApi.Controllers;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Application.Users;

namespace TeLoArreglo.WebApi.Controllers
{
    public class AuthController : AbpApiController
    {
        private readonly IUserAppService _userAppService;

        public AuthController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost, Route("api/Login")]
        public string Login([FromBody] UserLoginDto loginUser)
        {
            return _userAppService.Login(loginUser);
        }

        [HttpPost, Route("api/Logout")]
        public string Logout()
        {
            string token = Utillities.GetTokenFromRequest(Request);

            _userAppService.Logout(token);

            return token;
        }
    }
}