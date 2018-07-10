using System.Web.Http;
using Abp.WebApi.Controllers;
using TeLoArreglo.Application.Devices;
using TeLoArreglo.Application.Dtos.Device;

namespace TeLoArreglo.WebApi.Controllers
{
    public class DevicesController : AbpApiController
    {
        private readonly IDeviceAppService _deviceAppService;

        public DevicesController(IDeviceAppService deviceAppService)
        {
            _deviceAppService = deviceAppService;
        }

        [HttpPost, Route("api/Devices")]
        public DeviceOutputDto RegisterDevice(DeviceInputDto device)
        {
            string token = Utillities.GetTokenFromRequest(Request);

            return _deviceAppService.RegisterDevice(token, device);
        }
    }
}