using System.Collections.Generic;

namespace TeLoArreglo.Application.Dtos.DamageReport
{
    public class DamageReportInputDto
    {
        public string Description { get; set; }
        public GeoCoordinateDto GeoCoordinate { get; set; }
        public List<MediaInputDto> MediaResources { get; set; }
        public DamageReportPriorityDto Priority { get; set; }
    }
}
