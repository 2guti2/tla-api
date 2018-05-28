using Abp.Domain.Entities;

namespace TeLoArreglo.Application.Dtos.DamageReport
{
    public class MediaDto : Entity
    {
        public string MediaType { get; set; }
        public string Path { get; set; }
        public string OriginalName { get; set; }
    }
}
