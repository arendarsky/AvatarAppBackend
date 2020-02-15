using System.IO;
using System.Threading.Tasks;
using Avatar.App.Context;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

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

        public async Task UploadAsync(Stream fileStream, string fileName)
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
