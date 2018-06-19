using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TeLoArreglo.Logic.Entities
{
    public class Crew : User
    {
        public override List<Action> PermittedActions =>
            new List<Action>
            {
                Action.ModifyDamage,
                Action.QueryDamages,
                Action.RepairDamage,
                Action.ReportDamage,
            };

        public override Expression<Func<DamageReport, bool>> DamageReportsICanQuery()
        {
            return dr =>
                dr.Status == DamageStatus.Accepted ||
                dr.Status == DamageStatus.Repaired || 
                (dr.Status == DamageStatus.Repairing 
                    && dr.CrewMemberThatRepairedTheDamage != null 
                    && dr.CrewMemberThatRepairedTheDamage .Id == Id) ||
                dr.User.Id == Id;
        }
    }
}
