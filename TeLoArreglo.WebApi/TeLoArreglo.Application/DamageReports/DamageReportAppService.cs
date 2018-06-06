﻿using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Application.Users;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Common;
using TeLoArreglo.Logic.Entities;
using Action = TeLoArreglo.Logic.Entities.Action;

namespace TeLoArreglo.Application.DamageReports
{
    public class DamageReportAppService : AppService, IDamageReportAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IPermissionManager _permissionManager;
        private readonly IRepository<DamageReport> _damageReportsRepository;
        private readonly IRepository<Logic.Entities.Media> _mediaRepository;
        private readonly IRepository<Session> _sessionsRepository;

        public DamageReportAppService(
            IObjectMapper objectMapper,
            IPermissionManager permissionManager,
            IRepository<DamageReport> damageReportsRepository,
            IRepository<Logic.Entities.Media> mediaRepository,
            IRepository<Session> sessionsRepository)
        {
            _objectMapper = objectMapper;
            _permissionManager = permissionManager;
            _damageReportsRepository = damageReportsRepository;
            _mediaRepository = mediaRepository;
            _sessionsRepository = sessionsRepository;
        }

        public DamageReportOutputDto ReportDamage(DamageReportInputDto damageDto, string token)
        {
            VerifyCredentialsForDamageReporting(token);

            DamageReport damage = _objectMapper.Map<DamageReport>(damageDto);

            if(!damage.IsValid())
                throw new InvalidRequestException();

            BindMediaResources(damage);

            damage.User = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            _damageReportsRepository.Insert(damage);

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<DamageReportOutputDto>(damage);
        }

        public List<DamageReportOutputDto> GetAll(string token)
        {
            VerifyCredentialsForQueryingDamageReports(token);

            List<DamageReport> damageReports =
                _damageReportsRepository.GetAllIncluding(dr => dr.MediaResources).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public List<DamageReportOutputDto> GetAllOf(int id, string token)
        {
            VerifyCredentialsForQueryingDamageReports(token);

            List<DamageReport> damageReports = _damageReportsRepository
                .GetAllIncluding(dr => dr.MediaResources, dr => dr.User).Where(dr => dr.User.Id == id).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public DamageReportCompleteOutputDto Get(int id, string token)
        {
            VerifyCredentialsForQueryingDamageReports(token);

            DamageReport damageReport = _damageReportsRepository.GetAllIncluding(dr => dr.MediaResources).FirstOrDefault(dr => dr.Id == id);

            return _objectMapper.Map<DamageReportCompleteOutputDto>(damageReport);
        }
        
        public DamageReportCompleteOutputDto ModifyDamageReport(string token, int id, ModifyDamageReportDto modifiedDamage)
        {
            VerifyCredentialsForModifyingDamageReports(token);

            DamageReport damageReport = _damageReportsRepository.Get(id);

            _objectMapper.Map(modifiedDamage, damageReport);
            
            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<DamageReportCompleteOutputDto>(damageReport);
        }
        
        private void BindMediaResources(DamageReport damage)
        {
            int[] mediaResourceIds = damage.MediaResources.Select(mr => mr.Id).ToArray();

            foreach (int mediaResourceId in mediaResourceIds)
            {
                var dbMedia = _mediaRepository.Get(mediaResourceId);

                if(dbMedia == null)
                    throw new InvalidRequestException();

                damage.MediaResources.RemoveAll(mr => mr.Id == mediaResourceId);

                damage.MediaResources.Add(dbMedia);
            }
        }

        private void VerifyCredentialsForModifyingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if(!_permissionManager.HasPermission(executingUser, Action.ModifyDamage))
                throw new ForbiddenAccessException();
        }
        
        private void VerifyCredentialsForDamageReporting(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.ReportDamage))
                throw new ForbiddenAccessException();
        }

        private void VerifyCredentialsForQueryingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if(!_permissionManager.HasPermission(executingUser, Action.QueryDamages))
                throw new ForbiddenAccessException();
        }
    }
}