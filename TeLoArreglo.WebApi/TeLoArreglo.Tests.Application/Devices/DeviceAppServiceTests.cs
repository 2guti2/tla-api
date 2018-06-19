using System;
using System.Linq;
using TeLoArreglo.Application.Devices;
using TeLoArreglo.Application.Dtos.Device;
using TeLoArreglo.Tests.Application.DamageReports;
using TeLoArreglo.Tests.Application.DamageReports.Helpers;
using Xunit;

namespace TeLoArreglo.Tests.Application.Devices
{
    public class DeviceAppServiceTests : ApplicationTestBase
    {
        private readonly IDeviceAppService _deviceAppService;

        public DeviceAppServiceTests()
        {
            _deviceAppService = Resolve<IDeviceAppService>();
        }

        [Fact]
        public void RegisterDeviceTest()
        {
            var dto = new DeviceInputDto
            {
                DeviceToken = Guid.NewGuid().ToString()
            };

            string token = UsingDbContext(DamageReportsMockDatabaseHelper.AddAdminToContext);

            _deviceAppService.RegisterDevice(token, dto);

            UsingDbContext(context => Assert.Equal(context.Devices.First().DeviceToken, dto.DeviceToken));
        }
    }
}
