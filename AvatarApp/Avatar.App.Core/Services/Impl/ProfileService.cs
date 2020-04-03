using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avatar.App.Core.Entities;
using Avatar.App.Core.Exceptions;
using Avatar.App.Core.Helpers;
using Avatar.App.Core.Models;
using Avatar.App.Core.Specifications.LikedVideoSpecifications;
using Avatar.App.Core.Specifications.UserSpecifications;
using Avatar.App.SharedKernel.Interfaces;
using Avatar.App.SharedKernel.Settings;
using Microsoft.Extensions.Options;

namespace Avatar.App.Core.Services.Impl
{
    public class ProfileService : BaseServiceWithRating, IProfileService
    {
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileService(IOptions<AvatarAppSettings> avatarAppOptions, IRepository<User> userRepository, IRepository<LikedVideo> likedVideoRepository): base(likedVideoRepository, userRepository)
        {
            _avatarAppSettings = avatarAppOptions.Value;
        }

        public async Task<UserProfile> GetAsync(Guid userGuid)
        {
            var user = await GetUserAsync(new UserWithLoadedVideosSpecification(userGuid));

            var likesNumber = GetLikesNumber(user);

            return new UserProfile
            {
                LikesNumber = likesNumber,
                User = user
            };
        }

        public async Task<User> GetPublicAsync(long id)
        {
             return await GetUserAsync(new UserWithLoadedVideosSpecification(id));
        }

        public async Task<ICollection<LikedVideo>> GetNotificationsAsync(Guid userGuid, int number, int skip)
        {
            var user = await GetUserAsync(new UserWithLoadedVideosSpecification(userGuid));

            var likes = await GetNotificationsAsync(user);

            return likes.Skip(skip).Take(number).ToList();
        }

        public async Task<string> UploadPhotoAsync(Guid userGuid, Stream fileStream, string fileExtension)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            var fileName = CreateFileName(fileExtension);

            await UpdatePhotoAsync(user, fileStream, fileName);

            return fileName;
        }

        public async Task<Stream> GetPhotoStreamAsync(string fileName)
        {
            return await GetPhotoAsync(fileName);
        }

        public async Task SetDescriptionAsync(Guid userGuid, string description)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            user.Description = description;
            
            UserRepository.Update(user);
        }

        public async Task SetNameAsync(Guid userGuid, string name)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            user.Name = name;

            UserRepository.Update(user);
        }

        public async Task SetPasswordAsync(Guid userGuid, string oldPassword, string newPassword)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            if (!CheckUserPassword(user, oldPassword)) throw new InvalidPasswordException();

            user.Password = PasswordHelper.HashPassword(newPassword);

            UserRepository.Update(user);
        }

        #region Private Methods

        private static string CreateFileName(string fileExtension)
        {
            return Path.GetRandomFileName() + fileExtension;
        }

        private async Task<IEnumerable<LikedVideo>> GetNotificationsAsync(User user)
        {
            var notifications = await Task.Run(() =>
            {
                var likes = new List<LikedVideo>();
                foreach (var video in user.LoadedVideos.Where(v => v.IsApproved.HasValue && v.IsApproved == true))
                {
                    likes.AddRange(LikedVideoRepository.List(new LikedVideoWithUserSpecification(video))
                        .OrderBy(c => c.Date));
                }

                return likes;
            });

            return notifications;


        }

        private async Task UpdatePhotoAsync(User user, Stream fileStream, string fileName)
        {
            await UserRepository.InsertFileAsync(fileStream, fileName);

            user.ProfilePhoto = fileName;

            UserRepository.Update(user);
        }

        private async Task<Stream> GetPhotoAsync(string fileName)
        {
            return await UserRepository.GetFileAsync(fileName);
        }

        private static bool CheckUserPassword(User user, string password)
        {
            var hashed = PasswordHelper.HashPassword(password);
            return hashed == user.Password;
        }

        #endregion
    }
}
