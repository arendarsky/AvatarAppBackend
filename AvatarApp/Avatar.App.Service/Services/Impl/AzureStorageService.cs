using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Google.Apis.Drive.v3.Data;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using File = System.IO.File;
using User = Avatar.App.Entities.Models.User;

namespace Avatar.App.Service.Services.Impl
{
    public class AzureStorageService: IStorageService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly AvatarAppContext _context;

        public AzureStorageService(CloudStorageAccount cloudStorageAccount, AvatarAppContext context)
        {
            _cloudStorageAccount = cloudStorageAccount;
            _context = context;
        }

        public async Task UploadAsync(Stream fileStream, string fileName, string fileExtension = null)
        {
            var cloudBlobAccount = _cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobAccount.GetContainerReference("videos");
            await container.CreateIfNotExistsAsync();
            var newBlob = container.GetBlockBlobReference(fileName);
            await newBlob.UploadFromStreamAsync(fileStream);
        }

        public async Task<Stream> GetFileStreamAsync(string fileName)
        {
            var cloudBlobAccount = _cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobAccount.GetContainerReference("videos");
            if (!await container.ExistsAsync()) return null;

            var blob = container.GetBlockBlobReference(fileName);
            if (!await blob.ExistsAsync()) return null;

            return await blob.OpenReadAsync();
        }
    }
}
