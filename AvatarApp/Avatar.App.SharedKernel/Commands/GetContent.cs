using System.IO;
using MediatR;

namespace Avatar.App.SharedKernel.Commands
{
    public class GetContent: IRequest<FileStream>
    {
        public GetContent(string storagePrefix, string fileName)
        {
            StoragePrefix = storagePrefix;
            FileName = fileName;
        }

        public string StoragePrefix { get; }
        public string FileName { get; }
    }
}
