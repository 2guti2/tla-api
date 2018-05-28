using System;
using System.IO;
using FluentValidation;

namespace TeLoArreglo.Logic.Common.Media
{
    public static class FilePathResolver
    {
        public enum MediaType { Image, Video, Audio }
        public enum ImageFileExtension { Jpg, Png, Jpeg, Gif }
        public enum VideoFileExtension { Mp4 }
        public enum AudioFileExtension { Mp3 }

        public static MediaType GetFileType(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName)?.Replace(".", "").ToLower();

            foreach (var imageExtension in Enum.GetNames(typeof(ImageFileExtension)))
            {
                if (string.Equals(fileExtension, imageExtension, StringComparison.OrdinalIgnoreCase))
                    return MediaType.Image;
            }

            foreach (var videoExtension in Enum.GetNames(typeof(VideoFileExtension)))
            {
                if (string.Equals(fileExtension, videoExtension, StringComparison.OrdinalIgnoreCase))
                    return MediaType.Video;
            }

            foreach (var videoExtension in Enum.GetNames(typeof(AudioFileExtension)))
            {
                if (string.Equals(fileExtension, videoExtension, StringComparison.OrdinalIgnoreCase))
                    return MediaType.Audio;
            }

            throw new ValidationException("Bad datatype");
        }

        public static string GetFileTypeName(MediaType type)
        {
            return type.ToString();
        }
    }
}
