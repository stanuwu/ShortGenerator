namespace ShortGenerator.Error
{
    public class VoiceLoadException : Exception
    {
        public VoiceLoadException(string name) : base($"Voice could not be loaded: {name}")
        {
            
        }
    }
}