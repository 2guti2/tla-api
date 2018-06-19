using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using TeLoArreglo.Application.Dtos.User;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Common;
using TeLoArreglo.Logic.Common.Users;
using TeLoArreglo.Logic.Entities;
using Action = TeLoArreglo.Logic.Entities.Action;

namespace TeLoArreglo.Application.Users
{
    public class UserAppService : AppService, IUserAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IPermissionManager _permissionManager;
        private readonly IUserManager _userManager;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Session> _sessionRepository;

        public UserAppService(IObjectMapper objectMapper,
            IPermissionManager permissionManager,
            IUserManager userManager,
            IRepository<User> userRepository,
            IRepository<Session> sessionRepository)
        {
            _objectMapper = objectMapper;
            _permissionManager = permissionManager;
            _userManager = userManager;
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        public LoggedUserDto Login(UserLoginDto userDto)
        {
            User user = _userRepository.FirstOrDefault(User.EqualityExpression(_objectMapper.Map<User>(userDto)));

            _userManager.ValidateUser(user);

            Session session = _sessionRepository.FirstOrDefault(Session.EqualityExpressionByUser(user));

            if (session != null)
                return _objectMapper.Map<LoggedUserDto>(session);

            session = new Session(user);
            _sessionRepository.Insert(session);
            
            SaveChanges();

            return _objectMapper.Map<LoggedUserDto>(session);
        }

        public LoggedUserDto Logout(string token)
        {
            Session session = UserUtillities.GetCurrentSession(token, _sessionRepository);

            _userManager.ValidateSession(session);

            _sessionRepository.Delete(session);

            return _objectMapper.Map<LoggedUserDto>(session);
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
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

            if(!newUser.IsValid())
                throw new ModelValidationException(_objectMapper.Map<List<ValidationErrorDto>>(newUser.GetValidationErrors()));

            _userRepository.Insert(newUser);

            SaveChanges();

            return _objectMapper.Map<UserSignUpDtoOutput>(newUser);
        }

        private void SaveChanges()
        {
            try
            {
                CurrentUnitOfWork.SaveChanges();
            }
            catch (DataException)
            {
                throw new UsernameDuplicatedException();
            }
        }

        private void VerifyCredentialsForUserCreation(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.CreateUser))
                throw new ForbiddenAccessException();
        }

        public List<ActionDto> GetActionsOf(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionRepository);

            return _objectMapper.Map<List<ActionDto>>(executingUser.PermittedActions);
        }

        public UserSignUpDtoOutput BlockUser(string token, int id)
        {
            VerifyCredentialsForUserBlock(token);

            User user = _userRepository.Get(id);

            _userManager.BlockUser(user);

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<UserSignUpDtoOutput>(user);
        }

        private void VerifyCredentialsForUserBlock(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.BlockUser))
                throw new ForbiddenAccessException();
        }
    }
}
