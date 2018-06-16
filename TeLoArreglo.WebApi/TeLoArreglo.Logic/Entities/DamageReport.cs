using System;
using System.Collections.Generic;
using TeLoArreglo.Logic.Validators;

namespace TeLoArreglo.Logic.Entities
{
    public class DamageReport : Validable
    {
        public string Description { get; set; } 
        public DateTime Date { get; set; }
        public List<Media> MediaResources { get; set; }
        public GeoCoordinate GeoCoordinate { get; set; }
        public User User { get; set; }
        public DamageStatus Status { get; set; } = DamageStatus.Waiting;
        public DamagePriority Priority { get; set; } = DamagePriority.Low;
        
        public DamageReport()
        {
            Date = DateTime.UtcNow;
        }

        public bool IsValid()
        {
            var validator = new DamageReportValidator();
            _validationResult = validator.Validate(this);

            return _validationResult.IsValid;
        }
    }
}
