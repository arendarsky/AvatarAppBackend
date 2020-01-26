using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Avatar.App.Service.Services
{
    public interface IVideoService
    {
        Task<string> Upload(Stream uploadedVideoFileStream);
    }
}
