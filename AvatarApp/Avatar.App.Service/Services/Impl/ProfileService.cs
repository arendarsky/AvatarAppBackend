using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Avatar.App.Entities.Settings;
using Avatar.App.Service.Exceptions;
using Avatar.App.Service.Helpers;
using Avatar.App.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Avatar.App.Service.Services.Impl
{
    public class ProfileService : IProfileService
    {
        private readonly AvatarAppContext _context;
        private readonly IStorageService _storageService;
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileService(AvatarAppContext context, IStorageService storageService, IOptions<AvatarAppSettings> avatarAppOptions)
        {
            _context = context;
            _storageService = storageService;
            _avatarAppSettings = avatarAppOptions.Value;
        }

        public async Task<UserProfile> GetAsync(Guid userGuid)
        {
            var user = await GetUserAsync(userGuid);
            await _context.Entry(user).Collection(u => u.LoadedVideos).LoadAsync();
            var likesNumber = GetLikesNumber(user);

            return new UserProfile
            {
                LikesNumber = likesNumber,
                User = user
            };
        }

        public async Task<int> GetLikesNumberAsync(Guid userGuid)
        {
            var user = await GetUserAsync(userGuid);
            await _context.Entry(user).Collection(u => u.LoadedVideos).Query()
                .Where(v => v.IsApproved.HasValue && v.IsApproved == true).LoadAsync();

            return GetLikesNumber(user);
        }

        public int GetLikesNumber(User user)
        {
            return user.LoadedVideos.Where(v => v.IsApproved.HasValue && v.IsApproved == true).Sum(v => _context.LikedVideos.Count(c => c.VideoId == v.Id));
        }

        public async Task<string> UploadPhotoAsync(Guid userGuid, Stream fileStream, string fileExtension)
        {
            var user = await GetUserAsync(userGuid);

            var newFilename = Path.GetRandomFileName() + fileExtension;

            await _storageService.UploadAsync(fileStream, newFilename, _avatarAppSettings.ImageStoragePrefix);

            user.ProfilePhoto = newFilename;

            await _context.SaveChangesAsync();

            return newFilename;
        }

        public async Task<Stream> GetPhotoStreamAsync(string fileName)
        {
            return await _storageService.GetFileStreamAsync(fileName, _avatarAppSettings.ImageStoragePrefix);
        }

        public async Task SetDescriptionAsync(Guid userGuid, string description)
        {
            var user = await GetUserAsync(userGuid);
            user.Description = description;
            

            await _context.SaveChangesAsync();
        }

        public async Task SetNameAsync(Guid userGuid, string name)
        {
            var user = await GetUserAsync(userGuid);
            user.Name = name;

            await _context.SaveChangesAsync();
        }

        public async Task SetPasswordAsync(Guid userGuid, string oldPassword, string newPassword)
        {
            var user = await GetUserAsync(userGuid);

            if (PasswordHelper.HashPassword(oldPassword) != user.Password) throw new InvalidPasswordException();

            user.Password = PasswordHelper.HashPassword(newPassword);

            await _context.SaveChangesAsync();
        }

        #region Private Methods

        private async Task<User> GetUserAsync(Guid userGuid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Guid == userGuid);
            if (user == null) throw new UserNotFoundException();
            return user;
        }

        #endregion
    }
}
