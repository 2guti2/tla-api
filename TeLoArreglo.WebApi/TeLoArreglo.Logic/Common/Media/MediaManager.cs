using System;
using System.IO;
using System.Linq;

namespace TeLoArreglo.Logic.Common.Media
{
    public abstract class MediaManager
    {
        public const string MediaFolder = "Media/";
        public const string TempFolder = "Temp/";

        protected abstract string SpecificFolder { get; }

        public void Upload(Entities.Media media, string mediaMappedPath)
        {
            byte[] fileBytes = File.ReadAllBytes(media.Path);

            mediaMappedPath += SpecificFolder;

            string newFileName = CreateGuidOnlyNumbersAndLetters() + Path.GetExtension(media.OriginalName);

            string targetRelativeUrl = Path.Combine(mediaMappedPath, newFileName);

            File.WriteAllBytes(targetRelativeUrl, fileBytes);

            media.Path = MediaFolder + SpecificFolder + newFileName;
        }

        public string CreateGuidOnlyNumbersAndLetters()
        {
            return new string((from c in Guid.NewGuid().ToString()
            where char.IsLetterOrDigit(c) || c == '.'
                select c
            ).ToArray());
        }
    }
}
