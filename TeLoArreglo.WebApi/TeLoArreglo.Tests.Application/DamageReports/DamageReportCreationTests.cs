﻿using System.Linq;
using TeLoArreglo.Application.DamageReports;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Logic.Entities;
using TeLoArreglo.Tests.Application.DamageReports.Helpers;
using TeLoArreglo.Tests.Factories;
using Xunit;

namespace TeLoArreglo.Tests.Application.DamageReports
{
    public class DamageReportCreationTests : ApplicationTestBase
    {
        private readonly IDamageReportAppService _damageReportAppService;

        public DamageReportCreationTests()
        {
            _damageReportAppService = Resolve<IDamageReportAppService>();
        }

        [Fact]
        public void ReportDamageTest()
        {
            DamageReportInputDto dto = DamageReportFactory.NewDamageReportInput();

            string token = UsingDbContext(DamageReportsMockDatabaseHelper.AddAdminToContext);

            DamageReportOutputDto result = _damageReportAppService.ReportDamage(dto, token);

            UsingDbContext(context =>
            {
                Assert.Equal(dto.Description, context.DamageReports.First().Description);
            });

            Assert.Equal(result.Description, dto.Description);
        }

        [Fact]
        public void RepairDamageTest()
        {
            DamageReportInputDto dto = DamageReportFactory.NewDamageReportInput();

            string token = UsingDbContext(DamageReportsMockDatabaseHelper.AddAdminToContext);

            int id = _damageReportAppService.ReportDamage(dto, token).Id;

            string crewToken = UsingDbContext(DamageReportsMockDatabaseHelper.AddCrewToContext);

            _damageReportAppService.RepairDamage(crewToken, new DamageReportRepairDto { Id = id });

            UsingDbContext(context =>
            {
                DamageReport damage = context.DamageReports.Find(id);

                Assert.Equal(DamageStatus.Repaired, damage.Status);
            });
        }
    }
}