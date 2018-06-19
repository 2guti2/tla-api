using System.Linq;
using TeLoArreglo.Application.DamageReports;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Logic.Entities;
using TeLoArreglo.Tests.Application.DamageReports.Helpers;
using TeLoArreglo.Tests.Factories;
using Xunit;

namespace TeLoArreglo.Tests.Application.DamageReports
{
    public class DamageReportUpdateTests : ApplicationTestBase
    {
        private readonly IDamageReportAppService _damageReportAppService;

        public DamageReportUpdateTests()
        {
            _damageReportAppService = Resolve<IDamageReportAppService>();
        }

        [Fact]
        public void ModifyDamageReportTests()
        {
            DamageReport dr = DamageReportFactory.NewDamageReport();

            string token = UsingDbContext(context => DamageReportsMockDatabaseHelper.AddDamageReport(dr, context));

            _damageReportAppService.ModifyDamageReport(token, dr.Id, new ModifyDamageReportDto
            {
               Priority = DamageReportPriorityDto.High,
               Status = DamageStatusDto.Accepted
            });

            UsingDbContext(context =>
            {
                DamageReport savedDamage = context.DamageReports.First();

                Assert.Equal(DamagePriority.High, savedDamage.Priority);
                Assert.Equal(DamageStatus.Accepted, savedDamage.Status);
            });
        }
    }
}
