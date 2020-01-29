using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Avatar.App.Service.Services.Impl
{
    public class YoutubeVideoService : IVideoService
    {
        public async Task<string> Upload(Stream uploadedVideoFileStream)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "339863436345-8dtqcd9npgsnoonjqptld4hitc9jhf5k.apps.googleusercontent.com",
                    ClientSecret = "WB-mv_-L4RmJ7x5UsK3G4rFq"
                },
                new[] {YouTubeService.Scope.Youtube},
                "xcefactor",
                CancellationToken.None);
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "AvatarApp"
            });
            var insertRequest = service.Videos.Insert(new Video(), "", uploadedVideoFileStream, "multipart/form-data");
            var task = insertRequest.UploadAsync();
            throw new NotImplementedException();
        }
    }
}
