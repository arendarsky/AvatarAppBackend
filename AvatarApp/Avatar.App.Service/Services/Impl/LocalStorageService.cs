using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Entities;
using Avatar.App.Entities.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;

namespace Avatar.App.Service.Services.Impl
{
    public class LocalStorageService : IStorageService
    {
        private readonly AvatarAppSettings _avatarAppSettings;
        private readonly EnvironmentConfig _environmentConfig;

        public LocalStorageService(IOptions<EnvironmentConfig> environmentConfig, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _avatarAppSettings = avatarAppOptions.Value;
            _environmentConfig = environmentConfig.Value;
        }

        public async Task UploadAsync(Stream fileStream, string fileName, string storagePrefix)
        {
            var fullVideoPath = _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
            await using var videoFileStream = new FileStream(fullVideoPath, FileMode.Create);
            await fileStream.CopyToAsync(videoFileStream);
        }

        public async Task UploadWithConvertingAsync(Stream fileStream, string inputFileName, string outputFileName,
            string storagePrefix)
        {
            var fullInputVideoPath = _environmentConfig.STORAGE_PATH + _avatarAppSettings.TemporaryStoragePrefix + inputFileName;
            await using var videoFileStream = new FileStream(fullInputVideoPath, FileMode.Create);
            await fileStream.CopyToAsync(videoFileStream);

            var fullOutputFileName = _environmentConfig.STORAGE_PATH + storagePrefix + outputFileName;

            var mediaInfo = await MediaInfo.Get(fullInputVideoPath);
            var videoStream = mediaInfo.VideoStreams.First();
            var audioStream = mediaInfo.AudioStreams.First();

            videoStream.SetSize(VideoSize.Hd720);

            var conversion = Conversion.New()
                //Add video stream to output file
                .AddStream(videoStream)
                //Add audio stream to output file
                .AddStream(audioStream)
                //Set output file path
                .SetOutput(fullOutputFileName)
                //SetOverwriteOutput to overwrite files. It's useful when we already run application before
                .SetOverwriteOutput(true);

            //Add log to OnProgress
            conversion.OnProgress += async (sender, args) =>
            {
                //Show all output from FFmpeg to console
                Logger.Log.LogInformation($"[{args.Duration}/{args.TotalLength}][{args.Percent}%] {inputFileName}");
            };
            //Start conversion
            await conversion.Start();
            Logger.Log.LogInformation($"Finished conversion file [{outputFileName}]");
        }

        public async Task<Stream> GetFileStreamAsync(string fileName, string storagePrefix)
        {
            var fileStream = await Task.Run(() =>
            {
                var fullVideoPath = _environmentConfig.STORAGE_PATH + storagePrefix + fileName;
                return new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read);
            });
            return fileStream;
        }

    }
}
