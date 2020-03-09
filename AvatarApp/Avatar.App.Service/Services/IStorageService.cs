using System.IO;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IStorageService
    {
        Task UploadAsync(Stream fileStream, string fileName, string storagePrefix);
        Task<Stream> GetFileStreamAsync(string fileName, string storagePrefix);
    }
}
