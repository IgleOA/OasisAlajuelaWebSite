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

        public List<YouTubeVideo> Youtubelist()
        {
            return YDAL.YoutubeLiveEvents();
        }

        //public async Task<string> Insert(YouTubeVideo video)
        //{
        //    return await YDAL.InsertAsync(video);
        //}

        public YouTubeVideo YoutubeVideoValidation(string YouTubeID)
        {
            return YDAL.YoutubeVideoValidation(YouTubeID);
        }
    }
}
