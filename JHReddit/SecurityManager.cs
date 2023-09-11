using Reddit.AuthTokenRetriever;
using Reddit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Reddit.Controllers;

namespace JHReddit
{
    public class SecurityManager : ISecurityManager
    {
        protected readonly RedditClient _redditClient;
        public SecurityManager(string appID, string appSecret)
        {
            _redditClient = AuthorizeUser(appID, appSecret).Result;
        }

        public SecurityManager(string appID, string appSecret, string refreshToken)
        {
            _redditClient = new(appID, refreshToken, appSecret);
        }

        public async Task<RedditClient> AuthorizeUser(string appId, string appSecret, int port = 8080)
        {
            AuthTokenRetrieverLib authTokenRetrieverLib = new(appId, port, "localhost",
                appSecret: appSecret);

            // Start the callback listener.  --Kris
            // Note - Ignore the logging exception message if you see it.  You can use Console.Clear() after this call to get rid of it if you're running a console app.
            authTokenRetrieverLib.AwaitCallback();

            await OpenBrowser(authTokenRetrieverLib.AuthURL());
            while (string.IsNullOrWhiteSpace(authTokenRetrieverLib.RefreshToken))
            {

            }
            authTokenRetrieverLib.StopListening();
            return new(appId, authTokenRetrieverLib.RefreshToken, appSecret,
                authTokenRetrieverLib.AccessToken);
        }

        #region HelperMethods

        private async Task OpenBrowser(string authUrl, string browserPath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe")
        {
            try
            {

                Process.Start(new ProcessStartInfo(authUrl) { UseShellExecute = true });
                //ProcessStartInfo processStartInfo = new ProcessStartInfo(authUrl);
                //Process.Start(processStartInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // This typically occurs if the runtime doesn't know where your browser is.  Use BrowserPath for when this happens.  --Kris
                ProcessStartInfo processStartInfo = new ProcessStartInfo(browserPath)
                {
                    Arguments = authUrl
                };
                Process.Start(processStartInfo);
            }
        }

        protected Task<List<Post>> SearchSubRedditPosts(string subReddit, string after = "", int limit = 10)
        {
            //Get Top most post of subreddit 
            //page through after and get next 10 records
            List<Post> posts = _redditClient.Subreddit(subReddit).Posts.GetTop(after: after, limit: limit);
            return Task.FromResult(posts);
        }
        #endregion
    }
}
