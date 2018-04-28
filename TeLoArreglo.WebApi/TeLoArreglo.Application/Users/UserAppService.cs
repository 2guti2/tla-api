using System;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Application.Exceptions;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Application.Users
{
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<User> userRepository,
            IRepository<Session> sessionRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        public string Login(UserLoginDto userDto)
        {
            User dbUser = _userRepository.FirstOrDefault(u => u.Username.Equals(userDto.Username) && u.Password.Equals(userDto.Password));
            
            if(dbUser == null)
                throw new UnauthorizedAccessException();

            Session session = _sessionRepository.GetAllIncluding(s => s.User).FirstOrDefault(s => s.User.Id == dbUser.Id);

            if (session != null)
                return session.Token;

            session = new Session(dbUser);
            _sessionRepository.Insert(session);

            _unitOfWorkManager.Current.SaveChanges();

            return session.Token;
        }

        public void Logout(string token)
        {
            Session dbSession = _sessionRepository.FirstOrDefault(s => s.Token.Equals(token));

            if(dbSession == null)
                throw new InvalidApiKeyException();

            _sessionRepository.Delete(dbSession);
        }
    }
}
