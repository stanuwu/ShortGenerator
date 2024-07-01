using Reddit;
using Reddit.Controllers;
using ShortGenerator.Error;
using ShortGenerator.Util;

namespace ShortGenerator.Reddit
{
    public class RedditManager
    {
        private RedditClient Client { get; }
        public RedditManager(string user, string secret, string refresh)
        {
            Client = new RedditClient(appId:user, appSecret:secret, refreshToken:refresh);
        }

        public RedditPost GetHotPost(string subreddit)
        {
            var sub = Client.Subreddit(subreddit);
            if (sub == null) throw new RedditSearchException(subreddit);
            var posts = sub.Posts.GetHot();
            List<Post> valid = posts.Where(post => !post.NSFW && post.UpVotes >= Data.MinUpVotes).ToList();
            if (valid.Count < 1) throw new RedditSearchException(subreddit);
            var random = Utils.TimedRandom();
            var post = valid[random.Next(0, valid.Count)];
            var comments = post.Comments.GetTop();
            List<Comment> validComments = comments.Where(comment => comment.UpVotes >= Data.MinUpVotes).ToList();
            RedditAnswer[] loadedComments = new RedditAnswer[validComments.Count];
            for (int i = 0; i < loadedComments.Length; i++)
            {
                loadedComments[i] = new RedditAnswer(validComments[i].Author, validComments[i].Body);
            }
            random.Shuffle(loadedComments);
            return new RedditPost(subreddit, post.Author, post.Title, post.Listing.SelfText, loadedComments);
        }
    }
}