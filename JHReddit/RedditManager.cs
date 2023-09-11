using JHReddit.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHReddit
{
    internal class RedditManager
    {
        //Read appsetting.json 
        RedditSettingsDto redditSettings = new();
        private readonly PeriodicTimer timer;
        private readonly CancellationTokenSource cts = new();
        private Task? timerVoteTask;
        //private Task? timerUserTask;

        public RedditManager() {
            redditSettings = GetRedditSetting();
            timer = new PeriodicTimer(TimeSpan.FromSeconds(redditSettings.TimerRefreshInSeconds));
            //timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        }

        public async void Start()
        {
            timerVoteTask = ProcessRedditWatchAysnc();
            //timerUserTask = ProcessRedditUserWatchAsync();
            Console.WriteLine("RedditManager just started");
        }

        public async Task StopAsync()
        {   
            if(timerVoteTask is null) // || timerUserTask is null) 
            {
                return;
            }
            cts.Cancel();
            await timerVoteTask;
          //  await timerUserTask;
            cts.Dispose();
            Console.WriteLine("RedditManager just stopped");
        }

        public async Task ProcessRedditWatchAysnc()        
        {
            //TODO: Fix code to use dependancy injection
            IRedditVoteManager redditManager;
            redditManager = new RedditVoteManager(redditSettings.Credentials.ClientId, redditSettings.Credentials.AppSecrete, redditSettings.Credentials.RefreshToken);
            IRedditUserManager redditUserManager;
            redditUserManager = new RedditUserManager(redditSettings.Credentials.ClientId, redditSettings.Credentials.AppSecrete, redditSettings.Credentials.RefreshToken);

            try
            {
                while (await timer.WaitForNextTickAsync(cts.Token))
                {                    
                    //Get Most up votes 
                    List<RedditPostDto> posts = new();
                    foreach (var sub in redditSettings.SubReddits)
                    {
                        if (!redditManager.IsRunning)
                        {
                            var result = await redditManager.GetMostUpVotesPosts(sub, 1);
                            posts.AddRange(result);
                            foreach (var post in result)
                            {
                                Console.WriteLine($"SubRedditName-{sub}, contains post-{post.Title}, with most upvotes {post.UpVotes}");
                            }

                            if (redditManager.ExceptionCount > redditSettings.MaxTryOnError)
                            {
                                Console.WriteLine("Maximum error count occured while retriving data, exit.");
                                cts.Cancel();
                                cts.Dispose();
                                timer.Dispose();
                                return;
                            }

                        }
                        if (!redditUserManager.IsRunning)
                        {
                            var userResult = await redditUserManager.GetUserWithMostPosts(sub);
                            if (userResult != null)
                            {
                                Console.WriteLine($"SubRedditName-{sub},Author-{userResult.Author}, with most posts {userResult.PostCount}");
                            }
                            else
                                Console.WriteLine($"No result found for SubRedditName-{sub}");


                            if (redditUserManager.ExceptionCount > redditSettings.MaxTryOnError)
                            {
                                Console.WriteLine("Maximum error count occured while retriving data, exit.");
                                cts.Cancel();
                                cts.Dispose();
                                timer.Dispose();
                                return;
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation intruptible while process RedditVoteWatch.");
                cts.Cancel();
            }
        }


        public RedditSettingsDto GetRedditSetting()
        {
            RedditSettingsDto? redditSettings = null;
            //Update: Create common setting manager class to handle configuration and read/write settings
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);
                if (builder != null)
                {
                    IConfiguration config = builder.Build();
                    var section = config.GetSection("RedditSettings");
                    if (section != null && section.Exists())
                    {
                        redditSettings = section.Get<RedditSettingsDto>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Either Not able to read appsettings.json file or appsettings.json not exists.");
                Console.WriteLine(ex.ToString());
            }

            if (redditSettings == null)
            {
                redditSettings = new RedditSettingsDto();
                redditSettings.SubReddits = new List<string>() { "redditdev" };
                redditSettings.MaxTryOnError = 3;
                redditSettings.TimerRefreshInSeconds = 30; //30 seconds
                redditSettings.Credentials = new CredentialsDto()
                {
                    UserName = "MBDevApi",
                    RefreshToken = "53474382130240-wWZMo76LcL1uOZSMU9AgndismKcexA",
                    ClientId = "javeuLxwmCE3ym0UDm2ZmQ",
                    AppSecrete = "XQm19S6LWW0MdVL3vFfqSD8mOW0d1w"
                };
            }
            return redditSettings;
        }
    }


}
