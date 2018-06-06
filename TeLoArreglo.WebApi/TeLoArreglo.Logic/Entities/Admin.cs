using System.Collections.Generic;

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
                Action.ModifyDamage
            };
    }
}
