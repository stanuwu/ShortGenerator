using System.IO;
using ShortGenerator;
using ShortGenerator.Reddit;
using ShortGenerator.Video;
using ShortGenerator.Youtube;

Console.WriteLine("Short Generator by github.com/stanuwu");

Console.WriteLine("+ Grabbing Post");
var config = File.ReadAllLines("./video/config");
var reddit = new RedditManager(config[0], config[1], config[2]);
var sub = Data.RandomSubreddit();
var post = reddit.GetHotPost(sub);
var inputData = new InputData(sub, Data.RandomVideo(), Data.RandomMusic(), VideoCategory.Entertainment, false);
Console.WriteLine("+ Generating Video");
var video = VideoGenerator.GenerateReddit(inputData, post, true);
Console.WriteLine("+ Uploading Video");

Console.WriteLine();
Console.WriteLine(video.Title);
Console.WriteLine(video.Description);
Console.WriteLine();
var youtube = new YoutubeManager(config[3], config[4]);
youtube.UploadVideo(video);

Console.WriteLine("+ Completed");
