using Abp.Domain.Entities;
using TeLoArreglo.Logic.Common.Media;

namespace TeLoArreglo.Logic.Entities
{
    public class Media : Entity
    {
        public FilePathResolver.MediaType MediaType { get; set; }
        public string Path { get; set; }
        public string OriginalName { get; set; }
    }
}
