using TeLoArreglo.Application.DamageReports;

namespace TeLoArreglo.Tests.Application.Users
{
    public class DamageReportAppServiceTests : ApplicationTestBase
    {
        private readonly IDamageReportAppService _damageReportAppService;

        public DamageReportAppServiceTests()
        {
            _damageReportAppService = Resolve<IDamageReportAppService>();
        }


    }
}
