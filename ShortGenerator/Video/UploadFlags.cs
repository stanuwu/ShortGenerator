namespace ShortGenerator.Video
{
    public enum UploadFlags : byte
    {
        ForKids = 0x01,
        PaidPromotion = 0x02,
        ModifiedContent = 0x04,
        NotifySubscribers = 0x08,
    }
}