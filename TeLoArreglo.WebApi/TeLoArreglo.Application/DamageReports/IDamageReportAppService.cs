﻿using System.Collections.Generic;
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
    }
}