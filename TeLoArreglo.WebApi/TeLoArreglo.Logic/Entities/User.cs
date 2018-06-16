using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using TeLoArreglo.Logic.Validators;

namespace TeLoArreglo.Logic.Entities
{
    public class User : Validable
    {
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        public string Password { get; set; }
        public bool IsBlocked { get; set; } = false;
        public List<DamageReport> DamageReports { get; set; } = new List<DamageReport>();

        public virtual List<Action> PermittedActions => new List<Action>{ Action.ReportDamage, Action.QueryDamages };

        public static Expression<Func<User, bool>> EqualityExpression(User toCompare)
        {
            return u => u.Username.Equals(toCompare.Username) && u.Password.Equals(toCompare.Password);
        }

        public virtual Expression<Func<DamageReport, bool>> DamageReportsICanQuery()
        {
            return dr => dr.Status == DamageStatus.Accepted || dr.Status == DamageStatus.Repaired || dr.User.Id == Id;
        }

        public bool IsValid()
        {
            var validator = new UserValidator();
            _validationResult = validator.Validate(this);

            return _validationResult.IsValid;
        }
    }
}
