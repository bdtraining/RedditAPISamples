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
    public interface IRedditUserManager
    {   
        int ExceptionCount { get; }
        bool IsRunning { get; }
        /// <summary>
        /// Loop through all the post of subreddit to find most posts by users (author)
        /// </summary>
        /// <param name="subReddit">name of subreddit to search</param>
        /// <returns></returns>
        public Task<UserRedditPostDto> GetUserWithMostPosts(string subReddit);
    }
}
