using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Application.Users;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Common;
using TeLoArreglo.Logic.Common.DamageReports;
using TeLoArreglo.Logic.Entities;
using Action = TeLoArreglo.Logic.Entities.Action;
using DamageReport = TeLoArreglo.Logic.Entities.DamageReport;
using Session = TeLoArreglo.Logic.Entities.Session;

namespace TeLoArreglo.Application.DamageReports
{
    public class DamageReportAppService : AppService, IDamageReportAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IPermissionManager _permissionManager;
        private readonly IDamageReportManager _damageReportManager;
        private readonly IRepository<DamageReport> _damageReportsRepository;
        private readonly IRepository<Logic.Entities.Media> _mediaRepository;
        private readonly IRepository<Session> _sessionsRepository;
        private readonly IRepository<Device> _deviceRepository;

        public DamageReportAppService(
            IObjectMapper objectMapper,
            IPermissionManager permissionManager,
            IDamageReportManager damageReportManager,
            IRepository<DamageReport> damageReportsRepository,
            IRepository<Logic.Entities.Media> mediaRepository,
            IRepository<Session> sessionsRepository,
            IRepository<Device> deviceRepository)
        {
            _objectMapper = objectMapper;
            _permissionManager = permissionManager;
            _damageReportManager = damageReportManager;
            _damageReportsRepository = damageReportsRepository;
            _mediaRepository = mediaRepository;
            _sessionsRepository = sessionsRepository;
            _deviceRepository = deviceRepository;
        }

        public DamageReportOutputDto ReportDamage(DamageReportInputDto damageDto, string token)
        {
            VerifyCredentialsForDamageReporting(token);

            DamageReport damage = _objectMapper.Map<DamageReport>(damageDto);

            if (!damage.IsValid())
                throw new ModelValidationException(_objectMapper.Map<List<ValidationErrorDto>>(damage.GetValidationErrors()));

            BindMediaResources(damage);

            damage.User = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            _damageReportsRepository.Insert(damage);

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<DamageReportOutputDto>(damage);
        }

        public List<DamageReportOutputDto> GetAll(string token)
        {
            VerifyCredentialsForQueryingDamageReports(token);

            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            List<DamageReport> damageReports =
                _damageReportsRepository.GetAllIncluding(dr => dr.MediaResources).Where(user.DamageReportsICanQuery()).ToList();

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

            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            DamageReport damageReport = _damageReportsRepository
                .GetAllIncluding(dr => dr.MediaResources)
                .Where(user.DamageReportsICanQuery())
                .FirstOrDefault(dr => dr.Id == id);

            return _objectMapper.Map<DamageReportCompleteOutputDto>(damageReport);
        }

        public DamageReportCompleteOutputDto ModifyDamageReport(string token, int id, ModifyDamageReportDto modifiedDamage)
        {
            VerifyCredentialsForModifyingDamageReports(token);

            DamageReport damageReport = _damageReportsRepository.Get(id);

            _objectMapper.Map(modifiedDamage, damageReport);

            CheckIfDamageIsModifiable(damageReport);

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<DamageReportCompleteOutputDto>(damageReport);
        }

        private static void CheckIfDamageIsModifiable(DamageReport damageReport)
        {
            if (damageReport.Status == DamageStatus.Repaired)
                throw new ModelValidationException(
                    new List<ValidationErrorDto>
                    {
                        new ValidationErrorDto
                        {
                            Field = "Status", Message = "Cannot repair damage in this endpoint."
                        }
                    });
        }

        public List<DamageReportOutputDto> GetWithPriority(string token, DamageReportPriorityDto priority)
        {
            VerifyCredentialsForQueryingDamageReports(token);

            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            DamagePriority domainPriority = _objectMapper.Map<DamagePriority>(priority);

            List<DamageReport> damageReports =
                _damageReportsRepository.GetAllIncluding(dr => dr.MediaResources)
                .Where(user.DamageReportsICanQuery()).Where(dr => dr.Priority == domainPriority).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public DamageReportCompleteOutputDto RepairDamage(string token, DamageReportRepairDto damage)
        {
            VerifyCredentialsForModifyingDamageReports(token);

            DamageReport dbDamageReport = _damageReportsRepository
                .GetAllIncluding(d => d.MediaResources).FirstOrDefault(d => d.Id == damage.Id);

            _objectMapper.Map(damage, dbDamageReport);

            BindRepairedMediaResources(dbDamageReport);

            dbDamageReport.Status = DamageStatus.Repaired;

            CurrentUnitOfWork.SaveChanges();

            _damageReportManager.SendDamageRepairedNotification(dbDamageReport, GetDevicesOf(token));

            return _objectMapper.Map<DamageReportCompleteOutputDto>(dbDamageReport);
        }

        private List<Device> GetDevicesOf(string token)
        {
            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            return _deviceRepository.GetAllList().Where(d => d.User.Id == user.Id).ToList();
        }

        private void BindRepairedMediaResources(DamageReport damage)
        {
            int[] mediaResourceIds = damage.RepairedMediaResources.Select(mr => mr.Id).ToArray();

            foreach (int mediaResourceId in mediaResourceIds)
            {
                Logic.Entities.Media dbMedia = _mediaRepository.Get(mediaResourceId);

                if (dbMedia == null)
                    throw new InvalidRequestException();

                damage.RepairedMediaResources.RemoveAll(mr => mr.Id == mediaResourceId);

                damage.RepairedMediaResources.Add(dbMedia);
            }
        }

        private void BindMediaResources(DamageReport damage)
        {
            int[] mediaResourceIds = damage.MediaResources.Select(mr => mr.Id).ToArray();

            foreach (int mediaResourceId in mediaResourceIds)
            {
                Logic.Entities.Media dbMedia = _mediaRepository.Get(mediaResourceId);

                if (dbMedia == null)
                    throw new InvalidRequestException();

                damage.MediaResources.RemoveAll(mr => mr.Id == mediaResourceId);

                damage.MediaResources.Add(dbMedia);
            }
        }

        private void VerifyCredentialsForModifyingDamageReports(string token)
        {
            User executingUser = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            if (!_permissionManager.HasPermission(executingUser, Action.ModifyDamage))
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

            if (!_permissionManager.HasPermission(executingUser, Action.QueryDamages))
                throw new ForbiddenAccessException();
        }
    }
}
