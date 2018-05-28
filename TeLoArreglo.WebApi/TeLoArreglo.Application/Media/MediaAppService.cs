using System;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Logic.Common.Media;

namespace TeLoArreglo.Application.Media
{
    public class MediaAppService : AppService, IMediaAppService
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IMediaManagerDomainService _mediaManager;
        private readonly IRepository<Logic.Entities.Media> _mediaRepository;

        public MediaAppService(
            IObjectMapper objectMapper,
            IMediaManagerDomainService mediaManager,
            IRepository<Logic.Entities.Media> mediaRepository)
        {
            _mediaRepository = mediaRepository;
            _mediaManager = mediaManager;
            _objectMapper = objectMapper;
        }

        public MediaDto Upload(string filePath, string fileName, string mediaMappedPath)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception();
            
            Logic.Entities.Media newMedia = _mediaManager.CreateMediaEntity(filePath, fileName, mediaMappedPath);

            _mediaManager.UploadMediaEntityToFileSystem(newMedia);

            _mediaRepository.Insert(newMedia);

            CurrentUnitOfWork.SaveChanges();
            
            return _objectMapper.Map<MediaDto>(newMedia);
        }

        public string GetTempFolderPath()
        {
            return MediaManager.TempFolder;
        }

        public string GetMediaFolderPath()
        {
            return MediaManager.MediaFolder;
        }
    }
}
