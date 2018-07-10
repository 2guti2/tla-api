using TeLoArreglo.Application.Dtos.DamageReport;

namespace TeLoArreglo.Application.Media
{
    public interface IMediaAppService
    {
        MediaDto Upload(string filePath, string fileName, string mediaMappedPath);
        string GetTempFolderPath();
        string GetMediaFolderPath();
    }
}