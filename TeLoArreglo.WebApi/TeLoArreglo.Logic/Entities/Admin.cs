using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TeLoArreglo.Logic.Entities
{
    public class Admin : User
    {
        public override List<Action> PermittedActions => 
            new List<Action>
            {
                Action.CreateUser,
                Action.ReportDamage,
                Action.QueryDamages,
                Action.ModifyDamage,
                Action.BlockUser,
                Action.DeleteDamage
            };

        public override Expression<Func<DamageReport, bool>> DamageReportsICanQuery()
        {
            return dr => dr.Status == DamageStatus.Accepted 
            || dr.Status == DamageStatus.Repaired 
            || dr.Status == DamageStatus.Repairing 
            || dr.Status == DamageStatus.Waiting;
        }
    }
}
