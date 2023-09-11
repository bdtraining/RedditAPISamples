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
    public class RedditUserManager : SecurityManager, IRedditUserManager
    {
        private int errorCount = 0;
        private bool isRunning = false;
        public RedditUserManager(string appID, string appSecret) : base(appID,appSecret)
        {
        }

        public RedditUserManager(string appID, string appSecret, string refreshToken)
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

        public async Task<UserRedditPostDto?> GetUserWithMostPosts(string subReddit)
        {
            List<UserRedditPostDto> redditPosts = new();
            try
            {
                isRunning = true;
                bool isComplete = true;
                string after = "";
                //Loop through each post of subReddit POSTs
                while (isComplete)
                {                    
                    //Get list of post 
                    List<Post> posts = await SearchSubRedditPosts(subReddit, after);

                    if (posts != null && posts.Count > 0)
                    {
                        //set after to find next posts after post name
                        after = posts.Last().Fullname;
                        //Group by Author to get total post for author
                        foreach (var post in posts.GroupBy(t => t.Author).Select(t => new { name = t.Key, total = t.Count() }))
                        {
                            //If author exists in list then update the count
                            if (redditPosts.Any(t => t.Author == post.name))
                            {
                                var update = redditPosts.FirstOrDefault(t => t.Author == post.name);
                                if (update != null)
                                    update.PostCount = update.PostCount + post.total;
                            }
                            else
                                redditPosts.Add(new UserRedditPostDto() { Author = post.name, PostCount = post.total });
                        }
                    }
                    else
                    {
                        //If no post found or count ==0 then finish the loop 
                        isComplete = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                errorCount++;
                Console.WriteLine("Error occured while processing SearchUserWithMostPosts");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                isRunning = false;
            }
            //Return List of posts with author name and total post by users
            var result = redditPosts.OrderByDescending(t => t.PostCount).FirstOrDefault();
            return result;
        }

    }
}
