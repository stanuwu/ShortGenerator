using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using ShortGenerator.Video;

namespace ShortGenerator.Youtube
{
    public class YoutubeManager
    {
        private YouTubeService ApiService { get; }
        
        public YoutubeManager(string id, string secret)
        {
            var secrets = new ClientSecrets()
            {
                ClientId = id,
                ClientSecret = secret
            };
            var scopes = new string[]
            {
                "https://www.googleapis.com/auth/youtube",
                "https://www.googleapis.com/auth/youtube.channel-memberships.creator",
                "https://www.googleapis.com/auth/youtube.force-ssl",
                "https://www.googleapis.com/auth/youtube.readonly",
                "https://www.googleapis.com/auth/youtube.upload",
                "https://www.googleapis.com/auth/youtubepartner",
                "https://www.googleapis.com/auth/youtubepartner-channel-audit",
            };
            var credentialsTask = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", CancellationToken.None, new FileDataStore("ShortGenerator.Auth"));
            credentialsTask.Wait();
            var credentials = credentialsTask.Result;
            var init2 = new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Short Generator",
            };
            ApiService = new YouTubeService(init2);
        }

        public void UploadVideo(VideoData data)
        {
            var video = new Google.Apis.YouTube.v3.Data.Video
            {
                Snippet = new VideoSnippet
                {
                    Title = data.Title,
                    Description = data.Description,
                    CategoryId = $"{(int)data.VideoCategory}",
                    Tags = data.Tags
                },
                Status = new VideoStatus
                {
                    PrivacyStatus = "unlisted"
                }
            };

            var fileSteam = File.Open(data.File, FileMode.Open);
            fileSteam.Position = 0;
            var videoInsertRequest = ApiService.Videos.Insert(video, "snippet,status", fileSteam, "video/*");
            videoInsertRequest.NotifySubscribers = false;
            videoInsertRequest.Upload();
            fileSteam.Close();
        }
    }
}