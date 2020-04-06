using System;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System.Collections.Generic;
using ET;
using System.Threading.Tasks;
using System.IO;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;

namespace DAL
{
    public class YouTubeDAL
    {
        private YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = "AIzaSyBqdmvtcDCB56cTfs3O2kbYLTZANZrm9ts" });
        private String UploadedVideoId { get; set; }

        public List<YouTubeVideo> Youtubelist(int MaxResults)
        {
            List<YouTubeVideo> list = new List<YouTubeVideo>();

            var ListRequest = yt.Search.List("snippet");
            //var ListRequest = yt.Videos.List("snippet");
            ListRequest.ChannelId = "UCsWIb3EobSzS-pNrPXvq7_A"; //Igle Channel
            //ListRequest.ChannelId = "UCgs9_FAGGtcforOWC91fgDw"; // test Channel
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
                detail.Tags = item.Snippet.ETag;
                detail.PublishedAt = Convert.ToDateTime(item.Snippet.PublishedAt);
                
                list.Add(detail);
            }
            return list;
        }

        public string Insert(YouTubeVideo YTVideo)
        {
            //string rpta = null;

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = YTVideo.Title;
            video.Snippet.Description = YTVideo.Description;
            video.Snippet.Tags = new string[] { YTVideo.Tags };
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "public";

            var InsertRequest = yt.Videos.Insert(video, "snippet,status", YTVideo.VideoData, "video/*");
            InsertRequest.ResponseReceived += insertRequest_ResponseReceived;

            InsertRequest.Upload();

            return UploadedVideoId;
        }
        void insertRequest_ResponseReceived(Video video)
        {
            UploadedVideoId = video.Id;
            // video.ID gives you the ID of the Youtube video.
            // you can access the video from
            // http://www.youtube.com/watch?v={video.ID}
        }

    }
}
