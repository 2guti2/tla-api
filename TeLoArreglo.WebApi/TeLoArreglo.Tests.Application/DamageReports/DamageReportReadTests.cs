using System.Collections.Generic;
using System.Linq;
using TeLoArreglo.Application.DamageReports;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Logic.Entities;
using TeLoArreglo.Tests.Application.DamageReports.Helpers;
using TeLoArreglo.Tests.Factories;
using Xunit;

namespace TeLoArreglo.Tests.Application.DamageReports
{
    public class DamageReportReadTests : ApplicationTestBase
    {
        private readonly IDamageReportAppService _damageReportAppService;

        public DamageReportReadTests()
        {
            _damageReportAppService = Resolve<IDamageReportAppService>();
        }

        [Fact]
        public void GetDamageReportOfUserTest()
        {
            DamageReport dr = DamageReportFactory.NewDamageReport();

            string token = UsingDbContext(context => DamageReportsMockDatabaseHelper.AddDamageReport(dr, context));

            DamageReportCompleteOutputDto queryResult = _damageReportAppService.Get(dr.Id, token);

            Assert.Equal(dr.Description, queryResult.Description);
        }

        [Fact]
        public void GetDamageReportsRepairedByUserTest()
        {
            DamageReport dr = DamageReportFactory.NewDamageReport();

            (string token, int userId) queryParams =
                UsingDbContext(context => DamageReportsMockDatabaseHelper.AddRepairedDamageReport(dr, context));

            List<DamageReportOutputDto> queryResult =
                _damageReportAppService.GetReportsRepairedByUser(queryParams.token, queryParams.userId);

            Assert.Equal(dr.Description, queryResult.First().Description);
        }

        [Fact]
        public void GetDamageReportsWithPriorityTest()
        {
            DamageReport dr = DamageReportFactory.NewDamageReport();
            dr.Priority = DamagePriority.Medium;

            string token = UsingDbContext(context => DamageReportsMockDatabaseHelper.AddDamageReport(dr, context));

            List<DamageReportOutputDto> queryResult = _damageReportAppService.GetWithPriority(token, DamageReportPriorityDto.Medium);

            Assert.Equal(dr.Description, queryResult.First().Description);
            Assert.Equal(DamageReportPriorityDto.Medium, queryResult.First().Priority);
        }

        [Fact]
        public void GetDamageReportTest()
        {
            DamageReport dr = DamageReportFactory.NewDamageReport();

            string token = UsingDbContext(context => DamageReportsMockDatabaseHelper.AddDamageReport(dr, context));

            DamageReportCompleteOutputDto queryresult = _damageReportAppService.Get(dr.Id, token);

            Assert.Equal(queryresult.Description, dr.Description);
        }

        [Fact]
        public void GetDamageReportsTest()
        {
            var drs = new List<DamageReport>
            {
                DamageReportFactory.NewDamageReport(),
                DamageReportFactory.NewDamageReport()
            };

            string token = UsingDbContext(context =>
            {
                drs.ForEach(dr => context.DamageReports.Add(dr));
                return DamageReportsMockDatabaseHelper.AddDamageReport(DamageReportFactory.NewDamageReport(), context);
            });

            var result = _damageReportAppService.GetAll(token);

            Assert.Equal(3, result.Count);
        }
    }
}
