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

        [HttpGet, Route("api/Crews")]
        public List<UserSignUpDtoOutput> GetCrewMembers()
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _userAppService.GetCrewMembers(token);
        }

        [HttpGet, Route("api/Users")]
        public List<UserSignUpDtoOutput> GetUsers()
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _userAppService.GetAllUsers(token);
        }

        [HttpPost, Route("api/Users")]
        public UserSignUpDtoOutput SignUpUser(UserSignUpDtoInput user)
        {
            string token = Utillities.GetAuthTokenOrNullIfException(Request);

            return _userAppService.CreateUser(token, user);
        }

        [HttpPost, Route("api/Users/{id}")]
        public UserSignUpDtoOutput BlockUser(int id)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _userAppService.BlockUser(token, id);
        }
    }
}