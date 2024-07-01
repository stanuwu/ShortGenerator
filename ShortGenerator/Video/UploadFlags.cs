namespace ShortGenerator.Video
{
    public enum UploadFlags : byte
    {
        None = 0x00,
        ForKids = 0x01,
        PaidPromotion = 0x02,
        ModifiedContent = 0x04,
        NotifySubscribers = 0x08,
    }
}