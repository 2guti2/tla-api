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

        public TokenDto Login(UserLoginDto userDto)
        {
            User user = _userRepository.FirstOrDefault(User.EqualityExpression(_objectMapper.Map<User>(userDto)));
            
            if(user == null)
                throw new UnauthorizedAccessException();

            Session session = _sessionRepository.FirstOrDefault(Session.EqualityExpressionByUser(user));

            if (session != null)
                return _objectMapper.Map<TokenDto>(session);

            session = new Session(user);
            _sessionRepository.Insert(session);

            _unitOfWorkManager.Current.SaveChanges();

            return _objectMapper.Map<TokenDto>(session);
        }

        public TokenDto Logout(string token)
        {
            Session session = GetCurrentSession(token);

            if(session == null)
                throw new NotLoggedInException();

            _sessionRepository.Delete(session);

            return _objectMapper.Map<TokenDto>(session);
        }

        public UserSignUpDtoOutput CreateUser(string token, UserSignUpDtoInput userDto)
        {
            User newUser;

            if (token != null)
            {
                VerifyCredentialsForUserCreation(token);

                newUser = GlobalFactory.User(userDto.Role) as User;
            }
            else
                newUser = GlobalFactory.User(typeof(User).Name) as User;

            _objectMapper.Map(userDto, newUser);

            _userRepository.Insert(newUser);

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<UserSignUpDtoOutput>(newUser);
        }

        private void VerifyCredentialsForUserCreation(string token)
        {
            User executingUser = GetExecutingUserIfLoggedIn(token);

            if (!_permissionManager.HasPermission(executingUser, Action.CreateUser))
                throw new ForbiddenAccessException();
        }

        public List<ActionDto> GetActionsOf(string token)
        {
            User executingUser = GetExecutingUserIfLoggedIn(token);

            return _objectMapper.Map<List<ActionDto>>(executingUser.PermittedActions);
        }

        private User GetExecutingUserIfLoggedIn(string token)
        {
            User executingUser = GetCurrentSession(token)?.User;

            if (executingUser == null)
                throw new NotLoggedInException();

            return executingUser;
        }

        private Session GetCurrentSession(string token)
        {
            return _sessionRepository.GetAllIncluding(s => s.User)
                .FirstOrDefault(Session.EqualityExpression(new Session(token)));
        }
    }
}
