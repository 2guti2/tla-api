using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.WebApi.Controllers;
using TeLoArreglo.Application.Dtos.DamageReport;
using TeLoArreglo.Application.Media;

namespace TeLoArreglo.WebApi.Controllers
{
    public class MediaController : AbpApiController
    {
        private readonly string _tempFolderPath = "~/";
        private readonly string _mediaFolderPath = "~/";
        private readonly IMediaAppService _mediaService;

        public MediaController(IMediaAppService mediaService)
        {
            _mediaService = mediaService;
            _tempFolderPath += _mediaService.GetTempFolderPath();
            _mediaFolderPath += _mediaService.GetMediaFolderPath();
        }

        [HttpPost, Route("api/MediaResources")]
        public async Task<MediaDto> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);

            MultipartFormDataStreamProvider provider = GetTempFolderProvider();

            MultipartFormDataStreamProvider result = await Request.Content.ReadAsMultipartAsync(provider);

            MediaDto mediaResourceDto;
            string tempFilePath = null;
            try
            {
                tempFilePath = result.FileData.First().LocalFileName;

                string fileName = GetFileName(result);

                mediaResourceDto = _mediaService.Upload(tempFilePath, fileName, HttpContext.Current.Server.MapPath(_mediaFolderPath));
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tempFilePath))
                    File.Delete(tempFilePath);
            }

            return mediaResourceDto;
        }

        private string GetFileName(MultipartFormDataStreamProvider result)
        {
            string fileName = result.FileData.First().Headers.ContentDisposition.FileName;

            return new string((from c in fileName
                where char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_'
                select c
            ).ToArray());
        }

        private MultipartFormDataStreamProvider GetTempFolderProvider()
        {
            return new MultipartFormDataStreamProvider(HttpContext.Current.Server.MapPath(_tempFolderPath));
        }
    }
}