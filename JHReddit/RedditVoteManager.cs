using Reddit;
using Reddit.Controllers;
using Reddit.AuthTokenRetriever;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit.Inputs.Search;
using Reddit.Inputs;
using JHReddit.DTO;
using System.Diagnostics.Metrics;

namespace JHReddit
{
    public class RedditVoteManager : SecurityManager, IRedditVoteManager
    {
        private int errorCount = 0;
        private bool isRunning = false;
        public RedditVoteManager(string appID, string appSecret) : base(appID,appSecret)
        {
        }

        public RedditVoteManager(string appID, string appSecret, string refreshToken)
            : base(appID, appSecret, refreshToken)
        {
            
        }
        public bool IsRunning
        {
            get { return isRunning; }
        }
        public int ExceptionCount
        { 
            get { return errorCount; }            
        }

        public async Task<List<RedditPostDto>> GetMostUpVotesPosts(string subReddit, int limit = 10)
        {
            List<RedditPostDto> redditPosts = new();
            isRunning = true;
            try
            {
                //Get list of post for subreddit by most top votes
                List<Post> posts = await SearchSubRedditPosts(subReddit, "", limit);
                foreach (Post post in posts.OrderByDescending(t => t.UpVotes))
                {
                    redditPosts.Add(new(post));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while processing GetMostUpVotesPosts");
                Console.WriteLine(ex.Message);
                errorCount++;
            }
            finally
            {
                isRunning = false;
            }
            return redditPosts;
        }
        #region HelperMethods

        
        #endregion

    }
}
