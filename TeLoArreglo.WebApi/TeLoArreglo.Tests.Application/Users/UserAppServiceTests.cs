using System;
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
        public void UserAppService_Logout_UnSuccessful()
        {
            Session session = SessionFactory.NewSession();

            Assert.Throws<InvalidApiKeyException>(() => _userAppService.Logout(session.Token));
        }

        [Fact]
        public void UserAppService_Login_Successful()
        {
            User user = UserFactory.NewUser();

            UsingDbContext(context => context.Users.Add(user));

            string token = _userAppService.Login(new UserLoginDto {Username = user.Username, Password = user.Password});
            
            Assert.False(token.IsNullOrEmpty());

            UsingDbContext(context =>
            {
                Assert.Equal(1, context.Sessions.Count());
            });
        }

        [Fact]
        public void UserAppService_Login_Unsuccessful()
        {
            Assert.Throws<UnauthorizedAccessException>(() => _userAppService.Login(new UserLoginDto()));
        }
    }
}
