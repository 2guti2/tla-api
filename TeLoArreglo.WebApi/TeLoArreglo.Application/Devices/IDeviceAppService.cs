using TeLoArreglo.Application.Dtos.Device;

namespace TeLoArreglo.Application.Devices
{
    public interface IDeviceAppService
    {
        DeviceOutputDto RegisterDevice(string token, DeviceInputDto device);
    }
}
