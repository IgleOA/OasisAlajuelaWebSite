using System;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System.Collections.Generic;
using ET;
using System.Threading.Tasks;

namespace DAL
{
    public class YouTubeDAL
    {
        YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyBqdmvtcDCB56cTfs3O2kbYLTZANZrm9ts" });

        public List<YouTubeVideo> Youtubelist(int MaxResults)
        {
            List<YouTubeVideo> list = new List<YouTubeVideo>();

            var ListRequest = yt.Search.List("snippet");
            //var ListRequest = yt.Videos.List("snippet");
            ListRequest.ChannelId = "UCsWIb3EobSzS-pNrPXvq7_A";
            ListRequest.Type = "video";
            ListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            ListRequest.MaxResults = MaxResults;
            var ListResponse = ListRequest.Execute();

            foreach(var item in ListResponse.Items)
            {
                var detail = new YouTubeVideo();
                detail.id = item.Id.VideoId;
                detail.Title = item.Snippet.Title;
                detail.Description = item.Snippet.Description;
                detail.BannerLink = item.Snippet.Thumbnails.High.Url;
                detail.tags = item.Snippet.ETag;
                detail.PublishedAt = Convert.ToDateTime(item.Snippet.PublishedAt);
                
                list.Add(detail);
            }
            return list;
        }
    }
}
