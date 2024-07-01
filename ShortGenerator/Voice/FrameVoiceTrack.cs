using System.IO;
using System.Runtime.InteropServices;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using NAudio.Lame;
using NAudio.Wave;
using ShortGenerator.Error;

namespace ShortGenerator.Voice
{
    public class FrameVoiceTrack
    {
        private SpeechSynthesizer Synth { get; }
        private List<AudioStream> Streams { get; }
        private WaveFormat WaveFormat { get; }
        private int Fps { get; }
        public int Length { get; private set; }
        private long TotalBytes => Streams.Sum(s => s.BackingStream.Length);
        
        public FrameVoiceTrack(string voiceName, int fps = 60, int volume = 100, int speed = 2)
        {
            var synth = new SpeechSynthesizer();
            bool hasVoice = false;
            foreach (var voice in synth.GetInstalledVoices())
            {
                if (voice.VoiceInfo.Name == voiceName)
                {
                    hasVoice = true;
                    break;
                }
            }

            if (hasVoice)
            {
                synth.SelectVoice(voiceName);
            }
            else
            {
                throw new VoiceLoadException(voiceName);
            }

            synth.Volume = volume;
            synth.Rate = speed;

            Synth = synth;
            Streams = new List<AudioStream>();
            Length = 0;
            Fps = fps;

            var stream = InitNew();
            Synth.SetOutputToWaveStream(stream.BackingStream);
            Synth.Speak("test");
            stream.BackingStream.Position = 0;
            var temp = new WaveFileReader(stream.BackingStream);
            WaveFormat = temp.WaveFormat;
        }

        private int TimeToFrames(TimeSpan time)
        {
            return (int)time.TotalMilliseconds * 60 / 1000;
        }

        private AudioStream InitNew()
        {
            return new AudioStream(WaveFormat);
        }

        private int EndNew(AudioStream stream)
        {
            Streams.Add(stream);
            var len = TimeToFrames(stream.Length);
            Length += len;
            return len;
        }
        
        public int AddSpeech(string text)
        {
            var stream = InitNew();
            Synth.SetOutputToWaveStream(stream.BackingStream);
            Synth.Speak(text);
            return EndNew(stream);
        }

        public int AddBreak(int ms)
        {
            var stream = InitNew();
            var silence = new SilenceProvider(WaveFormat).ToSampleProvider().Take(TimeSpan.FromMilliseconds(ms)).ToWaveProvider();
            int sampleSize = WaveFormat.AverageBytesPerSecond * ms / 1000 / 2;
            byte[] data = new byte[sampleSize];
            silence.Read(data, 0, sampleSize);
            stream.BackingStream.Write(data, 0, sampleSize);
            return EndNew(stream);
        }

        public void Pop()
        {
            int idx = Streams.Count - 1;
            int lc = TimeToFrames(Streams[idx].Length);
            Streams.RemoveAt(idx);
            Length -= lc;
        }

        public void WriteToFile(string path)
        {
            WaveFileWriter writer = new WaveFileWriter(path + ".wav", WaveFormat);
            foreach (var audioStream in Streams)
            {
                int header = 44;
                long size = audioStream.BackingStream.Length - header;
                byte[] buffer = new byte[size];
                audioStream.BackingStream.Position = header;
                int res = audioStream.BackingStream.Read(buffer, 0, (int)size);
                if (res != size) throw new VoiceStreamSizeException();
                writer.Write(buffer);
            }
            writer.Dispose();

            var target = new WaveFormat(48000, 16, 2);
            var waveStream = new WaveFileReader(path + ".wav");
            var conversionStream = new WaveFormatConversionStream(target, waveStream);
            var mp3Writer = new LameMP3FileWriter(path, conversionStream.WaveFormat, 32);
            conversionStream.CopyTo(mp3Writer);
            
            waveStream.Dispose();
            conversionStream.Dispose();
            mp3Writer.Dispose();
            
            File.Delete(path + ".wav");
        }
    }
}