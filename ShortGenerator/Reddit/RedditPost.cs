namespace ShortGenerator.Reddit
{
    public record RedditPost(string Subreddit, string Poster, string Title, string Text, RedditAnswer[] Answers);
}