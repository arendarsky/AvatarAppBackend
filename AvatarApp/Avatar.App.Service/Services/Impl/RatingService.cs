using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Context;
using Avatar.App.Entities.Models;
using Avatar.App.Service.Exceptions;
using Avatar.App.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Service.Services.Impl
{
    public class RatingService : IRatingService
    {
        private readonly AvatarAppContext _context;
        private readonly IProfileService _profileService;

        public RatingService(AvatarAppContext context, IProfileService profileService)
        {
            _context = context;
            _profileService = profileService;
        }


        public async Task<ICollection<UserProfile>> GetRatingAsync(int number)
        {
            var users = _context.Users.Include(u => u.LoadedVideos).ToList();
            var userProfiles = new List<UserProfile>();
            await Task.Run(() =>
            {
                userProfiles.AddRange(from user in users
                    select new UserProfile { LikesNumber = _profileService.GetLikesNumber(user), User = user});
            });
            return userProfiles.OrderByDescending(r => r.LikesNumber).Take(number).ToList();
        }

        public async Task<ICollection<LikedVideo>> GetLikesAsync(Guid userGuid, int number, int skip)
        {
            var user = await GetUserAsync(userGuid);
            await _context.Entry(user).Collection(u => u.LoadedVideos).LoadAsync();
            var likes = new List<LikedVideo>();
            foreach (var video in user.LoadedVideos)
            {
                likes.AddRange(_context.LikedVideos.Include(l => l.User).ThenInclude(u => u.LoadedVideos).Where(c => c.VideoId == video.Id)
                    .OrderByDescending(c => c.Date));
            }

            return likes.Skip(skip).Take(number).ToList();
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
