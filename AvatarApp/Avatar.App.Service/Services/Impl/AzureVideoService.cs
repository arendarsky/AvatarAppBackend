using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Avatar.App.Service.Services.Impl
{
    public class AzureVideoService: IVideoService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly AvatarAppContext _context;

        public AzureVideoService(CloudStorageAccount cloudStorageAccount, AvatarAppContext context)
        {
            _cloudStorageAccount = cloudStorageAccount;
            _context = context;
        }

        public async Task UploadAsync(Stream fileStream, Guid userGuid, string fileExtension = null)
        {
            var user = _context.Users.FirstOrDefault(u => u.Guid == userGuid);
            if (user == null) throw new NullReferenceException();

            var cloudBlobAccount = _cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobAccount.GetContainerReference("videos");
            await container.CreateIfNotExistsAsync();

            var newFilename = Path.GetRandomFileName();
            var video = new Video
            {
                User = user,
                Name = newFilename,
                Extension = fileExtension
            };
            await _context.Videos.AddAsync(video);
            var newBlob = container.GetBlockBlobReference(newFilename);
            await newBlob.UploadFromStreamAsync(fileStream);
            await _context.SaveChangesAsync();
        }

        public Task<VideoStream> GetModeratedVideoAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<VideoStream> GetUncheckedVideoAsync()
        {
            var video = _context.Videos.FirstOrDefault(v => !v.IsApproved.HasValue);
            if (video == null) return null;

            var cloudBlobAccount = _cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobAccount.GetContainerReference("videos");
            if (!await container.ExistsAsync()) return null;

            var blob = container.GetBlockBlobReference(video.Name);
            if (!await blob.ExistsAsync()) return null;

            var stream = await blob.OpenReadAsync();

            return new VideoStream
            {
                Name = video.Name,
                Stream = stream
            };
        }
    }
}
