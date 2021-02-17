using System.IO;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Avatar.App.Content.Commands
{
    public class UploadContent: IRequest<string>
    {
        public IFormFile File { get; }
        public string StoragePrefix { get; }
        public string FileName { get; }

        public UploadContent(IFormFile file, string storagePrefix)
        {
            File = file;
            StoragePrefix = storagePrefix;
            var fileExtension = Path.GetExtension(file.FileName);
            FileName = Path.GetRandomFileName() + fileExtension;
        }
    }
}
