﻿using System;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using System.Collections.Generic;
using ET;
using System.Threading.Tasks;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using System.IO;
using System.Configuration;

namespace DAL
{
    public class YouTubeDAL
    {
        private YouTubeService yt = new YouTubeService(new BaseClientService.Initializer() { ApiKey = ConfigurationManager.AppSettings["YouTubeApiKey"].ToString() });

        private String UploadedVideoId { get; set; }

        public List<YouTubeVideo> YoutubeLiveEvents()
        {
            //var youtubeService = AuthenticateOauth();

            var list = new List<YouTubeVideo>();

            //var ListRequest = youtubeService.Search.List("snippet");
            ////var ListRequest = yt.Videos.List("snippet");
            //ListRequest.ChannelId = ConfigurationManager.AppSettings["YouTubeOAChannel"].ToString(); //Igle Channel
            ////ListRequest.ChannelId = "UCgs9_FAGGtcforOWC91fgDw"; // test Channel
            //ListRequest.EventType = SearchResource.ListRequest.EventTypeEnum.Live;
            //ListRequest.Type = "video";
            //ListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            //var ListResponse = ListRequest.Execute();

            //foreach (var item in ListResponse.Items)
            //{
            //    var detail = new YouTubeVideo
            //    {
            //        id = item.Id.VideoId,
            //        Title = item.Snippet.Title,
            //        Description = item.Snippet.Description,
            //        //BannerLink = item.Snippet.Thumbnails.High.Url,
            //        //Tags = item.Snippet.ETag,
            //        PublishedAt = Convert.ToDateTime(item.Snippet.PublishedAt)
            //    };
            //    list.Add(detail);
            //}

            //return list;

            var ValidationRequest = yt.Search.List("snippet");
            ValidationRequest.ChannelId = ConfigurationManager.AppSettings["YouTubeOAChannel"].ToString(); //Igle Channel
            ValidationRequest.EventType = SearchResource.ListRequest.EventTypeEnum.Live;
            ValidationRequest.Type = "video";
            ValidationRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

            var ListResponse = ValidationRequest.Execute();

            foreach (var item in ListResponse.Items)
            {
                var detail = new YouTubeVideo
                {
                    id = item.Id.VideoId,
                    Title = item.Snippet.Title,
                    Description = item.Snippet.Description,
                    //BannerLink = item.Snippet.Thumbnails.High.Url,
                    //Tags = item.Snippet.ETag,
                    PublishedAt = Convert.ToDateTime(item.Snippet.PublishedAt)
                };
                list.Add(detail);
            }

            return list;

        }

        public YouTubeVideo YoutubeVideoValidation(string YouTubeID)
        {
            YouTubeVideo YTVideo = new YouTubeVideo();

            var ValidationRequest = yt.Videos.List("snippet");
            ValidationRequest.Id = YouTubeID;

            var ListResponse = ValidationRequest.Execute();


            if (ListResponse.Items.Count > 0)
            {
                if (Convert.IsDBNull(ListResponse.Items[0].Snippet.Thumbnails.Maxres))
                {
                    YTVideo.BannerLink = ListResponse.Items[0].Snippet.Thumbnails.Maxres.Url;
                }
                else
                {
                    if (Convert.IsDBNull(ListResponse.Items[0].Snippet.Thumbnails.Standard))
                    {
                        YTVideo.BannerLink = ListResponse.Items[0].Snippet.Thumbnails.Standard.Url;
                    }
                    else
                    {
                        YTVideo.BannerLink = ListResponse.Items[0].Snippet.Thumbnails.High.Url;
                    }
                }
                YTVideo.ActiveFlag = true;
            }

            return YTVideo;
        }

        public async Task<string> InsertAsync(YouTubeVideo YTVideo)
        {
            //var youtubeService = AuthenticateOauth();


            UserCredential credential;
            var CSPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/");
            string[] scopes = new string[] { YouTubeService.Scope.Youtube,
                                             YouTubeService.Scope.YoutubeForceSsl,
                                             YouTubeService.Scope.Youtubepartner,
                                             YouTubeService.Scope.YoutubepartnerChannelAudit,
                                             YouTubeService.Scope.YoutubeReadonly,
                                             YouTubeService.Scope.YoutubeUpload};

            using (var stream = new FileStream(Path.Combine(CSPath, "client_secret.json"), FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None);
            }
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "WebsiteIgleOADev"
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = YTVideo.Title;
            video.Snippet.Description = YTVideo.Description;
            video.Snippet.Tags = new string[] { YTVideo.Tags };
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "public";
            const int KB = 0x400;
            var minimumChunkSize = 50 * KB;

            var InsertRequest = youtubeService.Videos.Insert(video, "snippet,status", YTVideo.VideoData, "video/*");
            InsertRequest.ProgressChanged += insertRequest_ProgressChanged;
            InsertRequest.ResponseReceived += insertRequest_ResponseReceived;
            InsertRequest.ChunkSize = minimumChunkSize * 80;

            await InsertRequest.UploadAsync();

            return UploadedVideoId;
        }
        void insertRequest_ResponseReceived(Video video)
        {
            UploadedVideoId = video.Id;
            // video.ID gives you the ID of the Youtube video.
            // you can access the video from
            // http://www.youtube.com/watch?v=P-v6sD0L8dw}
        }
        void insertRequest_ProgressChanged(IUploadProgress progress)
        {
            // You can handle several status messages here.
            switch (progress.Status)
            {
                case UploadStatus.Failed:
                    UploadedVideoId = "FAILED";
                    break;
                case UploadStatus.Completed:
                    break;
                default:
                    break;
            }
        }

        public static YouTubeService AuthenticateOauth()
        {

            string[] scopes = new string[] { 
                                             //YouTubeService.Scope.Youtube,
                                             //,YouTubeService.Scope.YoutubeForceSsl,
                                             //,YouTubeService.Scope.Youtubepartner,
                                             //,YouTubeService.Scope.YoutubepartnerChannelAudit,
                                             YouTubeService.Scope.YoutubeReadonly
                                             //,YouTubeService.Scope.YoutubeUpload
                                            };

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                {
                    ClientId = "758172315550-fkdk2nfvpnl4ui5nnf3ded50c2ru8fs4.apps.googleusercontent.com",
                    ClientSecret = "gxLsEPUS2gmh-C3i375yt1co"
                },
                    scopes,
                    "SingleUser",
                    CancellationToken.None,
                    new FileDataStore("Daimto.YouTube.Auth.Store")).Result;

                YouTubeService service = new YouTubeService(new YouTubeService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "IgleOAWebSite"
                });

                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }
    }
}