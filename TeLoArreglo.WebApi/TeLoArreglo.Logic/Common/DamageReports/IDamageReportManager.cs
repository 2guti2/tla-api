using System.Collections.Generic;
using Abp.Domain.Services;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Logic.Common.DamageReports
{
    public interface IDamageReportManager : IDomainService
    {
        void SendDamageRepairedNotification(DamageReport damageReport, List<Device> devices);
    }
}
