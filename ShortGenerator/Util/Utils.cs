using MediaToolkit;
using Path = System.IO.Path;

namespace ShortGenerator.Util
{
    public static class Utils
    {
        public static void MergeVideoAudio(string video, string audio, string output)
        {
            using (var engine = new Engine())
            {
                engine.CustomCommand($"-i {Path.GetFullPath(video)} -i {Path.GetFullPath(audio)} -shortest {Path.GetFullPath(output)}");
            }
        }
        
        public static void MergeAudioAudio(string audio1, string audio2, string output)
        {
            using (var engine = new Engine())
            {
                engine.CustomCommand($"-i {Path.GetFullPath(audio1)} -i {Path.GetFullPath(audio2)} -filter_complex amix=inputs=2:duration=shortest {Path.GetFullPath(output)}");
            }
        }
        
        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public static Random TimedRandom()
        {
            return new Random((int)DateTime.Now.Ticks);
        }
    }
}