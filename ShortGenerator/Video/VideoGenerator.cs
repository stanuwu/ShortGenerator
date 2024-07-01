using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using ShortGenerator.Reddit;
using ShortGenerator.Util;
using ShortGenerator.Voice;

namespace ShortGenerator.Video
{
    public static class VideoGenerator
    {
        public static VideoData GenerateReddit(InputData inputData, RedditPost post, bool debug = false)
        {
            // Time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Load Video
            FrameVideo video = new FrameVideo(inputData.Background);
            if (debug) Console.WriteLine($"Loaded Video in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            // Load Audio
            var audio = new FrameVoiceTrack(Style.VoiceName);
            if (debug) Console.WriteLine($"Loaded Audio in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();

            // Read 
            audio.AddBreak(160);
            int start = audio.Length;
            audio.AddSpeech($"R slash {post.Subreddit}");
            if (debug) Console.WriteLine($"Added Subreddit Speech in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            // Add Title
            video.OutlineText($"r/{post.Subreddit}", Style.FontFamily, FontStyle.Regular, 128, video.Width / 2, video.Height / 2, Color.Lime, 2, Color.Black, start, audio.Length);
            if (debug) Console.WriteLine($"Added Subreddit Title in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            // Add Subreddit
            video.OutlineText($"r/{post.Subreddit}", Style.FontFamily, FontStyle.Italic, 48, 5, 5, Color.White, 2, Color.Black, audio.Length, centered: false);
            if (debug) Console.WriteLine($"Added Subreddit in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            // Read Post Title
            audio.AddBreak(160);
            int postStart = audio.Length;
            audio.AddSpeech(post.Title);
            if (debug) Console.WriteLine($"Added Post Title Speech in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();

            // Seperate Title Lines
            int tLineSize = post.Title.Length > 64 ? 48 : 64;
            string[] tSegments = post.Title.Split(" ");
            List<string> tLines = new List<string>();
            string tLine = "";
            int tWidth = 800;
            foreach (var segment in tSegments)
            {
                if (video.MeasureText(tLine + $"{segment} ", Style.SmallFontFamily, FontStyle.Regular, tLineSize).Width > tWidth)
                {
                    tLines.Add(tLine);
                    tLine = "";
                }
                tLine += $"{segment} ";
            }
            if (!string.IsNullOrEmpty(tLine)) tLines.Add(tLine);
            
            // Add Post Title
            int tLineHeightP = tLineSize + 8;
            int tHeightP = tLineHeightP * tLines.Count;
            int tStartXP = tLineHeightP * tLines.Count;
            foreach (var titleText in tLines)
            {
                video.OutlineText(titleText, Style.FontFamily, FontStyle.Regular, tLineSize, video.Width / 2, video.Height / 2 - tStartXP + tHeightP / 2, Color.White, 2, Color.Black, postStart, audio.Length);
                tStartXP -= tLineHeightP;
            }
            if (debug) Console.WriteLine($"Added Post Title in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();

            Color bg = Color.FromArgb(190, 0, 0, 0);

            List<RedditAnswer> answers = new List<RedditAnswer>();
            if (!string.IsNullOrEmpty(post.Text)) answers.Add(new RedditAnswer(post.Poster, post.Text));
            answers.AddRange(post.Answers);
            int i = 0;
            foreach (var redditAnswer in answers)
            {
                bool done = false;
                i++;
                // Post Content
                audio.AddBreak(160);
                int nextPostStart = audio.Length;
                var textSentences = Regex.Split(redditAnswer.Text.Replace("\n", " "), "\\.|!|\\?\\s").Where(t => !string.IsNullOrEmpty(t) && t.Length > 1);
                string accumulator = "";
                foreach (var textSentence in textSentences)
                {
                    accumulator += textSentence + ". ";
                    
                    // Read Post Line
                    int nextContentStart = audio.Length;
                    audio.AddSpeech(textSentence);

                    if (audio.Length > video.Frames)
                    {
                        audio.Pop();
                        audio.AddBreak(160);
                        done = true;
                        break;
                    }
                    
                    audio.AddBreak(160);

                    // Seperate Post Lines
                    int lineSize = 48;
                    string[] segments = accumulator.Split(" ");
                    List<string> lines = new List<string>();
                    string line = "";
                    int width = 1000;
                    foreach (var segment in segments)
                    {
                        if (video.MeasureText(line + $"{segment} ", Style.SmallFontFamily, FontStyle.Regular, lineSize).Width > width)
                        {
                            lines.Add(line);
                            line = "";
                        }
                        line += $"{segment} ";
                    }
                    if (!string.IsNullOrEmpty(line)) lines.Add(line);

                    // Draw Post Lines
                    int lineHeight = lineSize + 8;
                    int totalHeight = lines.Count * lineHeight;
                    int startX = video.Height / 2 - totalHeight / 2 + lineHeight / 2;
                    video.RectangleRounded(bg, video.Width / 2, video.Height / 2, width, totalHeight + 16, 12, nextContentStart, audio.Length);
                    foreach (var lineText in lines)
                    {
                        video.Text(lineText, Style.SmallFontFamily, FontStyle.Regular, lineSize, video.Width / 2, startX, Color.White, nextContentStart, audio.Length);
                        startX += lineHeight;
                    }
                    
                    // Draw Title Lines
                    int tLineHeight = tLineSize + 8;
                    int tStartX = tLineHeight * tLines.Count;
                    foreach (var titleText in tLines)
                    {
                        video.OutlineText(titleText, Style.FontFamily, FontStyle.Regular, tLineSize, video.Width / 2, startX - totalHeight - tLineSize - tStartX, Color.White, 2, Color.Black, nextContentStart, audio.Length);
                        tStartX -= tLineHeight;
                    }
                }

                // Add Poster
                video.OutlineText($"u/{redditAnswer.Poster}", Style.FontFamily, FontStyle.Bold, 32, 5, video.Height - 5 - 32, Color.White, 1, Color.Black, nextPostStart, audio.Length, false);

                // Next Post
                audio.AddBreak(160);
                
                if (debug) Console.WriteLine($"Added Post {i} in {stopwatch.ElapsedMilliseconds}ms");
                stopwatch.Restart();
                
                if (done) break;
            }

            string tVid = "./video/temp/temp.mp4";
            string tAud = "./video/temp/temp.mp3";
            string tAud2 = "./video/temp/temp2.mp3";
            string outVid = "./video/out/out.mp4";
            
            video.WriteToFile(tVid);
            if (debug) Console.WriteLine($"Wrote Video in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            audio.WriteToFile(tAud);
            if (debug) Console.WriteLine($"Wrote Audio in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            Utils.MergeAudioAudio(tAud, inputData.Music, tAud2);
            if (debug) Console.WriteLine($"Merged Audio Tracks in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            Utils.MergeVideoAudio(tVid, tAud2, outVid);
            if (debug) Console.WriteLine($"Merged Files in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            File.Delete(tVid);
            File.Delete(tAud);
            File.Delete(tAud2);
            if (debug) Console.WriteLine($"Clean Up in {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Restart();
            
            if (debug) Console.WriteLine("DONE!");
            
            // Rename Video
            string videoName = "funny reddit post shorts content best reddit posts.mp4";
            string videoPath = outVid.Replace("out.mp4", videoName);
            File.Move(outVid, videoPath, true);
            
            // Generate Video Meta
            var random = Utils.TimedRandom();
            string title = $"r/{post.Subreddit} Best Posts {random.Next(0, 1000)} | Reddit Posts #fyp #reddit #shorts";
            string musicLink = inputData.Music.Split("/").Last().Split(".").First();
            List<string> creditNames = new List<string>();
            creditNames.Add($"u/{post.Poster}");
            creditNames.AddRange(post.Answers.Select(a => $"u/{a.Poster}"));
            string credits = String.Join(", ", creditNames);
            string description = $"{title}\n{post.Title}\nHigh quality reddit short content. Please like and subscribe!\nMusic: https://www.youtube.com/watch?v={musicLink}\nCredits: {credits}";
            string[] tags = new[] { post.Subreddit, "fyp", "for you page", "shorts", "short", "reddit", "reddit posts", "reddit tts", "funny reddit", "reddit drama", "reddit answers", "reddit tiktok", "tts reddit", "best reddit posts", "top reddit posts", "peak reddit", "brainrot reddit", "rizz", "skibidi", "ohio", "sigma" }; // tba
            UploadFlags flags = UploadFlags.None;
            return new VideoData(title, description, flags, inputData.Category, videoPath, tags);
        }
    }
}