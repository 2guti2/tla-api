using System.Collections.Generic;
using System.Web.Http;
using Abp.WebApi.Controllers;
using TeLoArreglo.Application.DamageReports;
using TeLoArreglo.Application.Dtos.DamageReport;

namespace TeLoArreglo.WebApi.Controllers
{
    public class DamageReportsController : AbpApiController
    {
        private readonly IDamageReportAppService _damageAppService;

        public DamageReportsController(IDamageReportAppService damageAppService)
        {
            _damageAppService = damageAppService;
        }

        [HttpGet, Route("api/DamageReports/{id}")]
        public DamageReportCompleteOutputDto GetDamageReport(int id)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _damageAppService.Get(id, token);
        }

        [HttpGet, Route("api/DamageReports/Users/{id}")]
        public List<DamageReportOutputDto> GetDamageReportsOfUser(int id)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _damageAppService.GetAllOf(id, token);
        }

        [HttpGet, Route("api/DamageReports")]
        public List<DamageReportOutputDto> GetDamageReports()
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _damageAppService.GetAll(token);
        }

        [HttpGet, Route("api/DamageReports")]
        public List<DamageReportOutputDto> GetDamageReportsWithPriority([FromUri] DamageReportPriorityDto priority)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _damageAppService.GetWithPriority(token, priority);
        }

        [HttpPut, Route("api/DamageReports/{id}")]
        public DamageReportCompleteOutputDto ModifyDamageReport(int id, ModifyDamageReportDto modifiedDamage)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _damageAppService.ModifyDamageReport(token, id, modifiedDamage);
        }

        [HttpPost, Route("api/DamageReports")]
        public DamageReportOutputDto ReportDamage(DamageReportInputDto damage)
        {
            string token = Utillities.GetAuthTokenOrNullIfException(Request);

            return _damageAppService.ReportDamage(damage, token);
        }

        [HttpPost, Route("api/RepairedDamageReports")]
        public DamageReportCompleteOutputDto RepairDamage(DamageReportRepairDto damage)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _damageAppService.RepairDamage(token, damage);
        }
    }
}