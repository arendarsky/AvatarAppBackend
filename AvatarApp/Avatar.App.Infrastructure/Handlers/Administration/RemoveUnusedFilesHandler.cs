using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avatar.App.Administration.Commands;
using Avatar.App.Infrastructure.Handlers.Content;
using Avatar.App.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Avatar.App.Infrastructure.Handlers.Administration
{
    internal class RemoveUnusedFilesHandler: FileHandler, IRequestHandler<RemoveUnusedFiles>
    {
        public RemoveUnusedFilesHandler(IOptions<EnvironmentConfig> environmentConfig) : base(environmentConfig)
        {
        }

        public Task<Unit> Handle(RemoveUnusedFiles request, CancellationToken cancellationToken)
        {
            var directory = new DirectoryInfo(EnvironmentConfig.STORAGE_PATH + request.StoragePrefix);
            RemoveUnusedFiles(request.UsedFileNames.ToList(), directory);
            return Task.FromResult(Unit.Value);
        }

        private static void RemoveUnusedFiles(ICollection<string> existedFiles, DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                if (!existedFiles.Contains(file.Name)) File.Delete(file.FullName);
            }
        }
    }
}
