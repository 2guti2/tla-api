using System.Collections.Generic;
using Abp.Domain.Entities;

namespace TeLoArreglo.Application.Dtos.DamageReport
{
    public class DamageReportRepairDto : Entity
    {
        public List<MediaInputDto> RepairedMediaResources { get; set; }
    }
}
