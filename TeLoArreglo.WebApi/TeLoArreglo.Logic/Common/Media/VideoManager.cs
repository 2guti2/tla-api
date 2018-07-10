namespace TeLoArreglo.Logic.Common.Media
{
    public class VideoManager : MediaManager
    {
        private const string VideosFolder = "Videos/";

        protected override string SpecificFolder => VideosFolder;
    }
}
