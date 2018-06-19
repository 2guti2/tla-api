using System.Collections.Generic;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Tests.Factories
{
    public class DamageReportFactory
    {
        public static DamageReport NewDamageReport()
        {
            return new DamageReport
            {
                Description = "Description",
                GeoCoordinate = new GeoCoordinate
                {
                    Latitude = 90,
                    Longitude = 90
                }
            };
        }

        public static DamageReportInputDto NewDamageReportInput()
        {
            return new DamageReportInputDto
            {
                Description = "Description",
                GeoCoordinate = new GeoCoordinateDto
                {
                    Latitude = 90,
                    Longitude = 90
                },
                MediaResources = new List<MediaInputDto>(),
                Priority = DamageReportPriorityDto.Medium
            };
        }
    }
}
