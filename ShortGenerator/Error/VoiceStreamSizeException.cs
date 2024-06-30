namespace ShortGenerator.Error
{
    public class VoiceStreamSizeException : Exception
    {
        public VoiceStreamSizeException() : base("Voice stream has an invalid size")
        {
            
        }
    }
}