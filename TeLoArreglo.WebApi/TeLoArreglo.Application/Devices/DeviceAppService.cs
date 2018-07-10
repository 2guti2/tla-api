using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using TeLoArreglo.Application.Dtos.Device;
using TeLoArreglo.Application.Users;
using TeLoArreglo.Exceptions;
using TeLoArreglo.Logic.Entities;

namespace TeLoArreglo.Application.Devices
{
    public class DeviceAppService : AppService, IDeviceAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<Device> _devicesRepository;
        private readonly IRepository<Session> _sessionRepository;

        public DeviceAppService(
            IObjectMapper objectMapper,
            IRepository<Device> devicesRepository,
            IRepository<Session> sessionRepository
            )
        {
            _objectMapper = objectMapper;
            _devicesRepository = devicesRepository;
            _sessionRepository = sessionRepository;
        }

        public DeviceOutputDto RegisterDevice(string token, DeviceInputDto device)
        {
            Device modelDevice = _objectMapper.Map<Device>(device);

            BindUser(modelDevice, token);

            if(!Exists(modelDevice))
                _devicesRepository.Insert(modelDevice);
            else
                throw new InvalidRequestException();

            CurrentUnitOfWork.SaveChanges();

            return _objectMapper.Map<DeviceOutputDto>(modelDevice);
        }

        private bool Exists(Device modelDevice)
        {
            return _devicesRepository.FirstOrDefault(d => d.DeviceToken.Equals(modelDevice.DeviceToken)) != null;
        }

        private void BindUser(Device modelDevice, string token)
        {
            modelDevice.User = UserUtillities.GetExecutingUserIfLoggedIn(token, _sessionRepository);
        }
    }
}
