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
using DamageReport = TeLoArreglo.Logic.Entities.DamageReport;
using Session = TeLoArreglo.Logic.Entities.Session;

namespace TeLoArreglo.Application.DamageReports
{
    public class DamageReportAppService : AppService, IDamageReportAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IDamageReportManager _damageReportManager;
        private readonly IRepository<DamageReport> _damageReportsRepository;
        private readonly IRepository<Logic.Entities.Media> _mediaRepository;
        private readonly IRepository<Session> _sessionsRepository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly CredentialsVerifier _credentialsVerifier;

        public DamageReportAppService(
            IObjectMapper objectMapper,
            IPermissionManager permissionManager,
            IDamageReportManager damageReportManager,
            IRepository<DamageReport> damageReportsRepository,
            IRepository<Logic.Entities.Media> mediaRepository,
            IRepository<Session> sessionsRepository,
            IRepository<Device> deviceRepository,
            IRepository<User> userRepository)
        {
            _objectMapper = objectMapper;
            _damageReportManager = damageReportManager;
            _damageReportsRepository = damageReportsRepository;
            _mediaRepository = mediaRepository;
            _sessionsRepository = sessionsRepository;
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
            _credentialsVerifier = new CredentialsVerifier(permissionManager, _sessionsRepository);
        }

        public DamageReportOutputDto ReportDamage(DamageReportInputDto damageDto, string token)
        {
            _credentialsVerifier.VerifyCredentialsForDamageReporting(token);

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
            _credentialsVerifier.VerifyCredentialsForQueryingDamageReports(token);

            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            List<DamageReport> damageReports =
                _damageReportsRepository.GetAllIncluding(dr => dr.MediaResources).Where(user.DamageReportsICanQuery()).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public List<DamageReportOutputDto> GetAllOf(int id, string token)
        {
            _credentialsVerifier.VerifyCredentialsForQueryingDamageReports(token);

            List<DamageReport> damageReports = _damageReportsRepository
                .GetAllIncluding(dr => dr.MediaResources, dr => dr.User).Where(dr => dr.User.Id == id).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public DamageReportCompleteOutputDto Get(int id, string token)
        {
            _credentialsVerifier.VerifyCredentialsForQueryingDamageReports(token);

            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            DamageReport damageReport = _damageReportsRepository
                .GetAllIncluding(dr => dr.MediaResources)
                .Where(user.DamageReportsICanQuery())
                .FirstOrDefault(dr => dr.Id == id);

            return _objectMapper.Map<DamageReportCompleteOutputDto>(damageReport);
        }

        public DamageReportCompleteOutputDto ModifyDamageReport(string token, int id, ModifyDamageReportDto modifiedDamage)
        {
            _credentialsVerifier.VerifyCredentialsForModifyingDamageReports(token);

            DamageReport damageReport = _damageReportsRepository.Get(id);

            DamageStatus oldStatus = damageReport.Status;

            _objectMapper.Map(modifiedDamage, damageReport);

            DamageStatus newStatus = damageReport.Status;

            CheckIfDamageIsModifiable(damageReport);

            if(modifiedDamage.Status == DamageStatusDto.Repairing)
                damageReport.CrewMemberThatRepairedTheDamage = _userRepository.Get(modifiedDamage.Crew.Id) as Crew;

            CurrentUnitOfWork.SaveChanges();

            NotifyStatusChange(oldStatus, newStatus);

            return _objectMapper.Map<DamageReportCompleteOutputDto>(damageReport);
        }

        private void NotifyStatusChange(DamageStatus oldStatus, DamageStatus newStatus)
        {
            if (HasDamageBeenAccepted(oldStatus, newStatus))
                _damageReportManager.SendDamageAcceptedNotification(GetCrewDevices());
        }

        private List<Device> GetCrewDevices()
        {
            return _deviceRepository.GetAll().Where(device => device.User is Crew).ToList();
        }

        private bool HasDamageBeenAccepted(DamageStatus oldStatus, DamageStatus newStatus)
        {
            return oldStatus != DamageStatus.Accepted && newStatus == DamageStatus.Accepted;
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
            _credentialsVerifier.VerifyCredentialsForQueryingDamageReports(token);

            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            DamagePriority domainPriority = _objectMapper.Map<DamagePriority>(priority);

            List<DamageReport> damageReports =
                _damageReportsRepository.GetAllIncluding(dr => dr.MediaResources)
                .Where(user.DamageReportsICanQuery()).Where(dr => dr.Priority == domainPriority).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public DamageReportCompleteOutputDto RepairDamage(string token, DamageReportRepairDto damage)
        {
            _credentialsVerifier.VerifyCredentialsForRepairingDamageReports(token);

            DamageReport dbDamageReport = _damageReportsRepository
                .GetAllIncluding(d => d.MediaResources).FirstOrDefault(d => d.Id == damage.Id);

            _objectMapper.Map(damage, dbDamageReport);

            BindRepairedMediaResources(dbDamageReport);

            dbDamageReport.Status = DamageStatus.Repaired;

            CurrentUnitOfWork.SaveChanges();

            _damageReportManager.SendDamageRepairedNotification(GetDevicesOf(token));

            return _objectMapper.Map<DamageReportCompleteOutputDto>(dbDamageReport);
        }

        public List<DamageReportOutputDto> GetReportsRepairedByUser(string token, int id)
        {
            _credentialsVerifier.VerifyCredentialsForQueryingDamageReports(token);

            List<DamageReport> damageReports = _damageReportsRepository
                .GetAllIncluding(d => d.MediaResources, d => d.RepairedMediaResources)
                .Where(d => d.CrewMemberThatRepairedTheDamage.Id == id).ToList();

            return _objectMapper.Map<List<DamageReportOutputDto>>(damageReports);
        }

        public void DeleteDamage(string token, int id)
        {
            _credentialsVerifier.VerifyCredentialsForDeletingDamageReports(token);

            _damageReportsRepository.Delete(id);
        }

        private List<Device> GetDevicesOf(string token)
        {
            User user = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionsRepository);

            return _deviceRepository.GetAllIncluding(d => d.User).Where(d => d.User.Id == user.Id).ToList();
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
    }
}
