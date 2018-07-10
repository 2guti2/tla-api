using System.ComponentModel.DataAnnotations.Schema;
using TeLoArreglo.Logic.Validators;

namespace TeLoArreglo.Logic.Entities
{
    public class GeoCoordinate
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
