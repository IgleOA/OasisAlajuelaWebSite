using System.Collections.Generic;
using ET;
using DAL;
using System;
using System.Threading.Tasks;

namespace BL
{
    public class YouTubeBL
    {
        private YouTubeDAL YDAL = new YouTubeDAL();

        public List<YouTubeVideo> Youtubelist(int maxresults)
        {
            return YDAL.Youtubelist(maxresults);
        }

        public async Task<string> Insert(YouTubeVideo video)
        {
            return await YDAL.InsertAsync(video);
        }
    }
}
