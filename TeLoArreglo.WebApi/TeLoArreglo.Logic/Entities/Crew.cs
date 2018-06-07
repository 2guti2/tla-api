using System;
using System.Linq.Expressions;

namespace TeLoArreglo.Logic.Entities
{
    public class Crew : User
    {
        public override Expression<Func<DamageReport, bool>> DamageReportsICanQuery()
        {
            return dr =>
                dr.Status == DamageStatus.Accepted || dr.Status == DamageStatus.Waiting ||
                dr.Status == DamageStatus.Repaired;
        }
    }
}
