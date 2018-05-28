namespace TeLoArreglo.Logic.Common.Media
{
    public class AudioManager : MediaManager
    {
        private const string AudioFolder = "Audio/";

        protected override string SpecificFolder => AudioFolder;
    }
}
