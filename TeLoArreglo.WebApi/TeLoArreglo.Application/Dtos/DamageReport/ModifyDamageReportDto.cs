using TeLoArreglo.Application.Dtos.User;

namespace TeLoArreglo.Application.Dtos.DamageReport
{
    public class ModifyDamageReportDto
    {
        public DamageStatusDto Status { get; set; }
        public CrewDto Crew { get; set; }
        public DamageReportPriorityDto Priority { get; set; }
    }
}
