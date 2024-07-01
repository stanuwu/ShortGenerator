namespace ShortGenerator.Error
{
    public class RedditSearchException : Exception
    {
        public RedditSearchException(string query) : base($"Query could not be found: {query}")
        {
            
        }
    }
}