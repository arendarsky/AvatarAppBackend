using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Models;

namespace Avatar.App.Core.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetAsync(Guid userGuid);
        Task<string> UploadPhotoAsync(Guid userGuid, Stream fileStream, string fileExtension);
        Task<Stream> GetPhotoStreamAsync(string fileName);
        Task SetDescriptionAsync(Guid userGuid, string description);
        Task SetNameAsync(Guid userGuid, string name);
        Task SetPasswordAsync(Guid userGuid, string oldPassword, string newPassword);
        Task<ICollection<LikedVideo>> GetNotificationsAsync(Guid userGuid, int number, int skip);
    }
}
