using System.Collections.Generic;
using System.Web.Http;
using Abp.WebApi.Controllers;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Application.Users;

namespace TeLoArreglo.WebApi.Controllers
{
    public class UsersController : AbpApiController
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        
        [HttpGet, Route("api/Actions")]
        public List<ActionDto> GetActions()
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _userAppService.GetActionsOf(token);
        }

        [HttpPost, Route("api/Users")]
        public UserSignUpDtoOutput SignUpUser(UserSignUpDtoInput user)
        {
            string token = Utillities.GetAuthTokenOrNullIfException(Request);

            return _userAppService.CreateUser(token, user);
        }
    }
}