using System.Collections.Generic;
using TeLoArreglo.Application.Dtos.DamageReport;

namespace TeLoArreglo.Application.DamageReports
{
    public interface IDamageReportAppService
    {
        DamageReportOutputDto ReportDamage(DamageReportInputDto damageDto, string token);
        List<DamageReportOutputDto> GetAll(string token);
        List<DamageReportOutputDto> GetAllOf(int id, string token);
        DamageReportCompleteOutputDto Get(int id, string token);
        DamageReportCompleteOutputDto ModifyDamageReport(string token, int id, ModifyDamageReportDto modifiedDamage);
        List<DamageReportOutputDto> GetWithPriority(string token, DamageReportPriorityDto priority);
        DamageReportCompleteOutputDto RepairDamage(string token, DamageReportRepairDto damage);
        List<DamageReportOutputDto> GetReportsRepairedByUser(string token, int id);
        void DeleteDamage(string token, int id);
    }
}