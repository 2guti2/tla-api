using Abp.Domain.Services;

namespace TeLoArreglo.Logic.Common.Media
{
    public interface IMediaManagerDomainService : IDomainService
    {
        Entities.Media CreateMediaEntity(string filePath, string fileName, string mediaMappedPath);

        void UploadMediaEntityToFileSystem(Entities.Media media);
    }
}