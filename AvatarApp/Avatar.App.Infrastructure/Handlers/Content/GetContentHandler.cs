using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Content.Commands;
using Avatar.App.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Handlers.Content
{
    internal class GetContentHandler: FileHandler, IRequestHandler<GetContent, FileStream>
    {
        private readonly ILogger<GetContentHandler> _logger;

        public GetContentHandler(ILogger<GetContentHandler> logger, IOptions<EnvironmentConfig> environmentConfig) : base(environmentConfig)
        {
            _logger = logger;
        }

        public Task<FileStream> Handle(GetContent request, CancellationToken cancellationToken)
        {
            var fullVideoPath = GetFilePath(request.FileName, request.StoragePrefix);
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(fullVideoPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                    FileOptions.Asynchronous);
            }
            catch(IOException ex)
            {
                _logger.LogWarning(ex, $"Full Path={fullVideoPath}");
            }

            return Task.FromResult(fileStream);
        }
    }
}
