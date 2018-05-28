namespace TeLoArreglo.Logic.Common.Media
{
    public class MediaManagerDomainService : IMediaManagerDomainService
    {
        private MediaManager _mediaManager;
        private string _mediaMappedPath;

        public Entities.Media CreateMediaEntity(string filePath, string fileName, string mediaMappedPath)
        {
            _mediaMappedPath = mediaMappedPath;

            FilePathResolver.MediaType fileMediaType = FilePathResolver.GetFileType(fileName);

            string fileTypeName = FilePathResolver.GetFileTypeName(fileMediaType);

            _mediaManager = (MediaManager)GlobalFactory.MediaManager(fileTypeName);

            return new Entities.Media { MediaType = fileMediaType, Path = filePath, OriginalName = fileName };
        }

        public void UploadMediaEntityToFileSystem(Entities.Media media)
        {
            _mediaManager.Upload(media, _mediaMappedPath);
        }
    }
}
