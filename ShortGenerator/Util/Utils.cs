using System.Diagnostics;
using System.IO;
using System.Windows.Shapes;
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
        
        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}