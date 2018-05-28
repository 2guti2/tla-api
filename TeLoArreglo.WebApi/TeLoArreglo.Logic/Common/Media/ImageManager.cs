namespace TeLoArreglo.Logic.Common.Media
{
    public class ImageManager : MediaManager
    {
        private const string ImagesFolder = "Images/";

        protected override string SpecificFolder => ImagesFolder;
    }
}
