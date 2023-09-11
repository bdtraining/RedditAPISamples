using JHReddit.DTO;
using Reddit;
using Reddit.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHReddit
{
    public interface IRedditVoteManager
    {
        int ExceptionCount { get; }
        bool IsRunning { get; }
        /// <summary>
        /// Get Most UpVote post from subReddit
        /// </summary>
        /// <param name="subReddit">name of subreddit to search</param>
        /// <param name="limit">no of post to search with most up votes</param>
        /// <returns></returns>
        public Task<List<RedditPostDto>> GetMostUpVotesPosts(string subReddit, int limit = 10);
    }
}
