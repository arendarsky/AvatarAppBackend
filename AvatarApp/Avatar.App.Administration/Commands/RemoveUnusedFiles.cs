using System.Collections.Generic;
using MediatR;

namespace Avatar.App.Administration.Commands
{
    public class RemoveUnusedFiles: IRequest
    {
        public RemoveUnusedFiles(IEnumerable<string> usedFileNames, string storagePrefix)
        {
            UsedFileNames = usedFileNames;
            StoragePrefix = storagePrefix;
        }

        public IEnumerable<string> UsedFileNames { get; }
        public string StoragePrefix { get; }
    }
}
