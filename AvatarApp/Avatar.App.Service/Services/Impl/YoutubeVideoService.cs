using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Logging;

namespace Avatar.App.Service.Services.Impl
{
    public class YoutubeVideoService : IVideoService
    {
        public async Task<string> Upload(Stream uploadedVideoFileStream)
        {
            Logger.Log.LogInformation("Запрос пришел");
            var serviceAccountEmail = "avatarapp@avatarapp.iam.gserviceaccount.com";
            var certificate = new X509Certificate2(@"avatarapp-c02672388663.p12", "notasecret", X509KeyStorageFlags.Exportable);
            //var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //    new ClientSecrets
            //    {
            //        ClientId = "339863436345-9te83vu5l00ep17m8hcnivjq6r6gcrm3.apps.googleusercontent.com",
            //        ClientSecret = "VVVyF6zgm4-fmcS9j4LCX_cv"
            //    },
            //    new[] {YouTubeService.Scope.Youtube},
            //    "xcefactor",
            //    CancellationToken.None);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = new[] {YouTubeService.Scope.Youtube}
            }.FromCertificate(certificate));
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "AvatarApp",
                ApiKey = ""
            });
            var video = new Video
            {
                Snippet = new VideoSnippet
                {
                    Title = "Default Video Title",
                    Description = "Default Video Description",
                    Tags = new string[] { "tag1", "tag2" },
                    CategoryId = "22"
                },
                Status = new VideoStatus
                {
                    PrivacyStatus = "unlisted"
                }
            };
            var insertRequest = service.Videos.Insert(video, "snippet,status", uploadedVideoFileStream, "video/*");
            insertRequest.ProgressChanged += VideosInsertRequest_ProgressChanged;
            insertRequest.ResponseReceived += VideosInsertRequest_ResponseReceived;
            Logger.Log.LogInformation("Видео готово к отправке");
            await insertRequest.UploadAsync();
            return "Ok";
        }

        private static void VideosInsertRequest_ProgressChanged(IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Logger.Log.LogInformation("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Logger.Log.LogWarning("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        private static void VideosInsertRequest_ResponseReceived(Video video)
        {
            Logger.Log.LogInformation("Video id '{0}' was successfully uploaded.", video.Id);
        }
    }
}
