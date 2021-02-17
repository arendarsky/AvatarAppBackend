using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Content.Commands;
using Avatar.App.Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Handlers.Content
{
    internal class UploadFileHandler: FileHandler, IRequestHandler<UploadContent, string>
    {
        public UploadFileHandler(IOptions<EnvironmentConfig> environmentConfig) : base(environmentConfig)
        {
        }

        public async Task<string> Handle(UploadContent request, CancellationToken cancellationToken)
        {
            var fullPath = GetFilePath(request.FileName, request.StoragePrefix);
            await SaveFileAsync(fullPath, request.File);
            return request.FileName;
        }

        private static async Task SaveFileAsync(string fullPath, IFormFile file)
        {
            await using var outputFileStream = File.Create(fullPath);
            await file.CopyToAsync(outputFileStream);
        }

    }
}
