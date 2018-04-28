using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Application.Exceptions;
using TeLoArreglo.Application.Users;
using TeLoArreglo.Logic.Entities;
using TeLoArreglo.Tests.Factories;
using Xunit;

namespace TeLoArreglo.Tests.Application.Users
{
    public class UserAppServiceTests : ApplicationTestBase
    {
        private readonly IUserAppService _userAppService;

        public UserAppServiceTests()
        {
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public void UserAppService_Logout_Successful()
        {
            Session session = SessionFactory.NewSession();

            UsingDbContext(context => context.Sessions.Add(session));

            _userAppService.Logout(session.Token);

            UsingDbContext(context =>
            {
                Assert.Equal(0, context.Sessions.Count());
            });
        }

        [Fact]
        public void UserAppService_Logout_NotLoggedIn()
        {
            Session session = SessionFactory.NewSession();

            Assert.Throws<NotLoggedInException>(() => _userAppService.Logout(session.Token));
        }

        [Fact]
        public void UserAppService_Login_Successful()
        {
            User user = UserFactory.NewUser();

            UsingDbContext(context => context.Users.Add(user));

            string token = _userAppService.Login(new UserLoginDto { Username = user.Username, Password = user.Password });

            Assert.False(token.IsNullOrEmpty());

            UsingDbContext(context =>
            {
                Assert.Equal(1, context.Sessions.Count());
            });
        }

        [Fact]
        public void UserAppService_Login_Unauthorized()
        {
            Assert.Throws<UnauthorizedAccessException>(() => _userAppService.Login(new UserLoginDto()));
        }

        [Fact]
        public void UserAppService_SignUp_Successful()
        {
            Session session = SessionFactory.NewSession();

            session.User = UserFactory.NewAdmin();

            UsingDbContext(context => context.Sessions.Add(session));

            UserSignUpDtoInput input = UserFactory.NewSignUpDtoInputForAdmin();

            UserSignUpDtoOutput output = _userAppService.SignUp(session.Token, input);

            Assert.Equal(input.Role, output.Role);

            UsingDbContext(context =>
            {
                Admin admin = context.Users.FirstOrDefault(u => u.Id == output.Id) as Admin;
                Assert.Equal(output.Username, admin?.Username);
            });
        }

        [Fact]
        public void UserAppService_SignUp_NotLoggedIn()
        {
            Session session = SessionFactory.NewSession();

            UserSignUpDtoInput input = UserFactory.NewSignUpDtoInputForAdmin();

            Assert.Throws<NotLoggedInException>(() => _userAppService.SignUp(session.Token, input));
        }

        [Fact]
        public void UserAppService_SignUp_Unauthorized()
        {
            Session session = SessionFactory.NewSession();

            session.User = UserFactory.NewUser();

            UsingDbContext(context => context.Sessions.Add(session));

            UserSignUpDtoInput input = UserFactory.NewSignUpDtoInputForAdmin();

            Assert.Throws<ForbiddenAccessException>(() => _userAppService.SignUp(session.Token, input));
        }

        [Fact]
        public void UserAppService_GetActions_Success()
        {
            Session session = SessionFactory.NewSession();

            session.User = UserFactory.NewAdmin();

            UsingDbContext(context => context.Sessions.Add(session));

            List<ActionDto> actions = _userAppService.GetActionsOf(session.Token);

            Assert.Contains(ActionDto.CreateUser, actions);
        }

        [Fact]
        public void UserAppService_GetActions_NotLoggedIn()
        {
            Session session = SessionFactory.NewSession();

            Assert.Throws<NotLoggedInException>(() => _userAppService.GetActionsOf(session.Token));
        }
    }
}
