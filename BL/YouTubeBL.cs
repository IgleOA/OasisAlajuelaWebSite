using System.Collections.Generic;
using ET;
using DAL;
using System;

namespace BL
{
    public class YouTubeBL
    {
        private YouTubeDAL YDAL = new YouTubeDAL();

        public List<YouTubeVideo> Youtubelist(int maxresults)
        {
            return YDAL.Youtubelist(maxresults);
        }
    }
}
