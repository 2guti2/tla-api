using TeLoArreglo.Logic.Entities;
using TeLoArreglo.Repository;
using TeLoArreglo.Tests.Factories;

namespace TeLoArreglo.Tests.Application.DamageReports.Helpers
{
    public class DamageReportsMockDatabaseHelper
    {
        public static (string token, int userId) AddRepairedDamageReport(DamageReport dr, TeLoArregloContext context)
        {
            dr.User = context.Users.Add(UserFactory.NewUser());
            dr.CrewMemberThatRepairedTheDamage = context.Crews.Add(UserFactory.NewCrew());

            Session session = AddSessionToContext(dr.CrewMemberThatRepairedTheDamage, context);

            context.DamageReports.Add(dr);

            context.SaveChanges();

            return (session.Token, dr.CrewMemberThatRepairedTheDamage.Id);
        }

        public static string AddDamageReport(DamageReport dr, TeLoArregloContext context)
        {
            dr.User = context.Admins.Add(UserFactory.NewAdmin());

            Session session = AddSessionToContext(dr.User, context);

            context.DamageReports.Add(dr);

            return session.Token;
        }

        public static Session AddSessionToContext(User user, TeLoArregloContext context)
        {
            Session session = SessionFactory.NewSessionWithUser(user);

            return context.Sessions.Add(session);
        }

        public static string AddAdminToContext(TeLoArregloContext context)
        {
            User user = context.Admins.Add(UserFactory.NewAdmin());

            return AddSessionToContext(user, context).Token;
        }

        public static string AddCrewToContext(TeLoArregloContext context)
        {
            User user = context.Crews.Add(UserFactory.NewCrew());

            return AddSessionToContext(user, context).Token;
        }
    }
}
