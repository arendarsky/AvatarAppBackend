using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Content.Commands;
using Avatar.App.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Handlers.Content
{
    internal class GetContentHandler: FileHandler, IRequestHandler<GetContent, FileStream>
    {
        public GetContentHandler(IOptions<EnvironmentConfig> environmentConfig) : base(environmentConfig)
        {
        }

        public Task<FileStream> Handle(GetContent request, CancellationToken cancellationToken)
        {
            var fullVideoPath = GetFilePath(request.FileName, request.StoragePrefix);
            var fileStream = new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                FileOptions.Asynchronous);
            return Task.FromResult(fileStream);
        }
    }
}
