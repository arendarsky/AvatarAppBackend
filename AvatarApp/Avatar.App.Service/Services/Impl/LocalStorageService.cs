using System;
using System.Collections.Generic;
using System.IO;
using Avatar.App.Service.Helpers;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services.Impl
{
    public class LocalStorageService : IStorageService
    {
        private readonly AvatarAppContext _avatarContext;
        private readonly string _videoStoreDirection = @"C:\VideoDB";
        public LocalStorageService(AvatarAppContext videoContext)
        {
            _avatarContext = videoContext;
        }
        public async Task UploadAsync(Stream uploadedVideoFileStream, string fileName, string fileExtension)
        {
            var fullPathToVideo = _videoStoreDirection + "\\" + fileName + fileExtension;
            await using var videoFileStream = new FileStream(@fullPathToVideo, FileMode.Create);
            await uploadedVideoFileStream.CopyToAsync(videoFileStream);
        }

        public Task<Stream> GetFileStreamAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
