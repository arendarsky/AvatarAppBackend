using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Logging;
using File = Google.Apis.Drive.v3.Data.File;

namespace Avatar.App.Service.Services.Impl
{
    public class YoutubeVideoService : IVideoService
    {
        private readonly DriveService _driveService;

        public YoutubeVideoService(DriveService driveService)
        {
            _driveService = driveService;
        }

        public async Task<string> Upload(Stream uploadedVideoFileStream)
        {
            Logger.Log.LogInformation("Запрос пришел");
            UserCredential credential;

            //await using (var stream =
            //    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            //{
            //    // The file token.json stores the user's access and refresh tokens, and is created
            //    // automatically when the authorization flow completes for the first time.
            //    const string credPath = "token.json";
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new string[] { DriveService.Scope.DriveReadonly},
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;
            //    Logger.Log.LogInformation("Credential file saved to: " + credPath);
            //}

            // Create Drive API service.
            Logger.Log.LogInformation("Service start");
            Logger.Log.LogInformation("Service created");
            var insertRequest = _driveService.Files.Create(
                new File()
                {
                    Name = "test"
                },
                uploadedVideoFileStream,
                "video/*");

            // Add handlers which will be notified on progress changes and upload completion.
            // Notification of progress changed will be invoked when the upload was started,
            // on each upload chunk, and on success or failure.
            insertRequest.ProgressChanged += Upload_ProgressChanged;
            insertRequest.ResponseReceived += Upload_ResponseReceived;

            var task = await insertRequest.UploadAsync();
            return "Ok";
        }
        static void Upload_ProgressChanged(IUploadProgress progress)
        {
            Logger.Log.LogInformation(progress.Status + " " + progress.BytesSent + " " + progress.Exception);
        }

        static void Upload_ResponseReceived(File file)
        {
            Logger.Log.LogInformation(file.Name + " was uploaded successfully");
        }
    }
}
