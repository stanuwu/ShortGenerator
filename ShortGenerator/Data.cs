using ShortGenerator.Util;

namespace ShortGenerator
{
    public static class Data
    {
        public static readonly int MinUpVotes = 1000;

        public static readonly string[] Subreddits = { "AskReddit", "AmITheAsshole" };
        public static readonly string[] Videos = { "./video/bg/destiny2.mp4" };
        public static readonly string[] Music = { "./video/bg/520ASjtTbKM.mp3" };

        public static string RandomSubreddit()
        {
            var random = Utils.TimedRandom();
            return Subreddits[random.Next(0, Subreddits.Length)];
        }
        
        public static string RandomVideo()
        {
            var random = Utils.TimedRandom();
            return Videos[random.Next(0, Videos.Length)];
        }
        
        public static string RandomMusic()
        {
            var random = Utils.TimedRandom();
            return Music[random.Next(0, Music.Length)];
        }
    }
}