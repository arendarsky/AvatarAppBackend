using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Models;

namespace Avatar.App.Service.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetAsync(Guid userGuid);
        Task<string> UploadPhotoAsync(Guid userGuid, Stream fileStream, string fileExtension);
        Task<Stream> GetPhotoStreamAsync(string fileName);
        Task SetDescriptionAsync(Guid userGuid, string description);
        Task SetNameAsync(Guid userGuid, string name);
        Task SetPasswordAsync(Guid userGuid, string oldPassword, string newPassword);
        Task<int> GetLikesNumberAsync(Guid userGuid);
        int GetLikesNumber(User user);
    }
}
