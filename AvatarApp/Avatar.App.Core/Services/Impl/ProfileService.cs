﻿using System;
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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Avatar.App.Core.Services.Impl
{
    public class ProfileService : BaseServiceWithRating, IProfileService
    {
        private readonly AvatarAppSettings _avatarAppSettings;

        public ProfileService(IOptions<AvatarAppSettings> avatarAppOptions, IRepository<User> userRepository,
            IRepository<LikedVideo> likedVideoRepository) : base(
            likedVideoRepository, userRepository)
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

        public async Task<ICollection<LikedVideo>> GetNotificationsAsync(Guid userGuid, int take, int skip)
        {
            var user = await GetUserAsync(new UserWithLoadedVideosSpecification(userGuid));

            var likes = await GetNotificationsAsync(user);

            var orderedLikes = TakeNotificationsOrderedByDate(likes, take, skip);

            return orderedLikes;
        }

        public async Task<string> UploadPhotoAsync(Guid userGuid, IFormFile file)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            var fileName = CreateFileName(file);

            await UpdatePhotoAsync(user, file, fileName);

            return fileName;
        }

        public Stream GetPhotoStream(string fileName)
        {
            return GetPhoto(fileName);
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

        public void RemoveAllUnusedFiles()
        {
            var existedFiles = GetImageFilesNames();

            UserRepository.RemoveFiles(existedFiles);
        }

        public async Task UpdateProfileAsync(Guid userGuid, PrivateProfileDto profile)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            user.Name = profile.Name;

            user.Description = profile.Description;

            user.InstagramLogin = profile.InstagramLogin;

            UserRepository.Update(user);
        }

        public async Task StopGeneralEmailing(Guid userGuid)
        {
            var user = await GetUserAsync(new UserSpecification(userGuid));

            user.ConsentToGeneralEmail = false;

            UserRepository.Update(user);
        }
        #region Private Methods

        private static string CreateFileName(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            return Path.GetRandomFileName() + fileExtension;
        }

        private async Task<IEnumerable<LikedVideo>> GetNotificationsAsync(User user)
        {
            var notifications = await Task.Run(() =>
            {
                var likes = new List<LikedVideo>();
                foreach (var video in user.LoadedVideos.Where(v => v.IsApproved.HasValue && v.IsApproved == true))
                {
                    likes.AddRange(LikedVideoRepository.List(new LikedVideoWithUserSpecification(video)));
                }

                return likes;
            });

            return notifications;


        }

        private static ICollection<LikedVideo> TakeNotificationsOrderedByDate(IEnumerable<LikedVideo> likedVideos, int take,
            int skip)
        {
            return likedVideos.OrderByDescending(l => l.Date).Take(take).Skip(skip).ToList();
        } 

        private async Task UpdatePhotoAsync(User user, IFormFile file, string fileName)
        {
            await UserRepository.InsertFileAsync(file, fileName);

            user.ProfilePhoto = fileName;

            UserRepository.Update(user);
        }

        private Stream GetPhoto(string fileName)
        {
            return UserRepository.GetFile(fileName);
        }

        private static bool CheckUserPassword(User user, string password)
        {
            var hashed = PasswordHelper.HashPassword(password);
            return hashed == user.Password;
        }

        private ICollection<string> GetImageFilesNames()
        {
            return UserRepository.List().Select(user => user.ProfilePhoto).ToList();
        }

        #endregion
    }
}
