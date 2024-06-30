namespace ShortGenerator.Error
{
    public class VideoLoadException : Exception
    {
        public VideoLoadException(string path) : base($"Video could not be loaded: {path}")
        {
            
        }
    }
}