namespace ShortGenerator.Video
{
    public record VideoData(string Title, string Description, UploadFlags UploadFlags, VideoCategory VideoCategory, string File, string[] Tags);
}