using System.IO;
using NAudio.Wave;

namespace ShortGenerator.Voice
{
    public class AudioStream
    {
        public AudioStream(WaveFormat format)
        {
            BackingStream = new MemoryStream();
            WaveStream = new RawSourceWaveStream(BackingStream, format);
        }
        
        public Stream BackingStream { get; }
        public WaveStream WaveStream { get; }

        public TimeSpan Length => WaveStream.TotalTime;
    }
}