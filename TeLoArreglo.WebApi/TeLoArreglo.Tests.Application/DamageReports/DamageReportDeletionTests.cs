using TeLoArreglo.Application.DamageReports;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Tests.Application.DamageReports.Helpers;
using TeLoArreglo.Tests.Factories;
using Xunit;

namespace TeLoArreglo.Tests.Application.DamageReports
{
    public class DamageReportDeletionTests : ApplicationTestBase
    {
        private readonly IDamageReportAppService _damageReportAppService;

        public DamageReportDeletionTests()
        {
            _damageReportAppService = Resolve<IDamageReportAppService>();
        }

        [Fact]
        public void DeleteDamageReport()
        {
            DamageReportInputDto dto = DamageReportFactory.NewDamageReportInput();

            string token = UsingDbContext(DamageReportsMockDatabaseHelper.AddAdminToContext);

            DamageReportOutputDto result = _damageReportAppService.ReportDamage(dto, token);

            _damageReportAppService.DeleteDamage(token, result.Id);

            UsingDbContext(context => Assert.Empty(context.DamageReports));
        }

        [Fact]
        public void DeleteDamageReport_InsufficientPrivileges()
        {
            DamageReportInputDto dto = DamageReportFactory.NewDamageReportInput();

            string token = UsingDbContext(context => 
            DamageReportsMockDatabaseHelper.AddSessionToContext(UserFactory.NewUser(), context).Token);

            DamageReportOutputDto result = _damageReportAppService.ReportDamage(dto, token);

            Assert.Throws<ForbiddenAccessException>(() => _damageReportAppService.DeleteDamage(token, result.Id));
        }
    }
}
