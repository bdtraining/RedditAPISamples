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
    public interface ISecurityManager
    {
        /// <summary>
        /// Authorize user using Reddit.Net to get refresh token as suggested in 
        /// https://github.com/sirkris/Reddit.NET/blob/master/docs/examples/cs/Authorize%20New%20User.md
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public Task<RedditClient> AuthorizeUser(string appId, string appSecret, int port = 8080);
    }
}
