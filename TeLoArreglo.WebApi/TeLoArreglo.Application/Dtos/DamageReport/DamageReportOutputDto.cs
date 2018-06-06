using System.Collections.Generic;
using Abp.Domain.Entities;

namespace TeLoArreglo.Application.Dtos.DamageReport
{
    public class DamageReportOutputDto : Entity
    {
        public string Description { get; set; }
        public GeoCoordinateDto GeoCoordinate { get; set; }
        public List<MediaDto> MediaResources { get; set; }
        public DamageStatusDto Status { get; set; }
    }
}
