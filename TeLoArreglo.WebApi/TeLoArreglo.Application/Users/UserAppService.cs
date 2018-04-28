using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Application.Exceptions;
using TeLoArreglo.Logic.Common;
using TeLoArreglo.Logic.Entities;
using Action = TeLoArreglo.Logic.Entities.Action;

namespace TeLoArreglo.Application.Users
{
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Session> _sessionRepository;

        public UserAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IObjectMapper objectMapper,
            IPermissionManager permissionManager,
            IRepository<User> userRepository,
            IRepository<Session> sessionRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _objectMapper = objectMapper;
            _permissionManager = permissionManager;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        public string Login(UserLoginDto userDto)
        {
            User user = _userRepository.FirstOrDefault(u => u.Username.Equals(userDto.Username) && u.Password.Equals(userDto.Password));
            
            if(user == null)
                throw new UnauthorizedAccessException();

            Session session = _sessionRepository.FirstOrDefault(s => s.User.Id == user.Id);

            if (session != null)
                return session.Token;

            session = new Session(user);
            _sessionRepository.Insert(session);

            _unitOfWorkManager.Current.SaveChanges();

            return session.Token;
        }

        public void Logout(string token)
        {
            Session session = _sessionRepository.FirstOrDefault(s => s.Token.Equals(token));

            if(session == null)
                throw new NotLoggedInException();

            _sessionRepository.Delete(session);
        }

        public UserSignUpDtoOutput SignUp(string token, UserSignUpDtoInput userDto)
        {
            User executingUser = GetExecutingUser(token);

            if (executingUser == null)
                throw new NotLoggedInException();

            if(!_permissionManager.HasPermission(executingUser, Action.CreateUser))
                throw new ForbiddenAccessException();

            User newUser = InstanceCreator.User(userDto.Role) as User;

            _objectMapper.Map(userDto, newUser);

            _userRepository.Insert(newUser);

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<UserSignUpDtoOutput>(newUser);
        }

        public List<ActionDto> GetActionsOf(string token)
        {
            User executingUser = GetExecutingUser(token);

            if(executingUser == null)
                throw new NotLoggedInException();

            return _objectMapper.Map<List<ActionDto>>(executingUser.PermittedActions);
        }

        private User GetExecutingUser(string token)
        {
            return _sessionRepository.GetAllIncluding(s => s.User)
                .FirstOrDefault(s => s.Token.Equals(token))?.User;
        }
    }
}
