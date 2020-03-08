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

        public RatingService(AvatarAppContext context)
        {
            _context = context;
        }

        public async Task<ICollection<RatingItem>> GetRatingAsync(int number)
        {
            var users = _context.Users.Include(u => u.LoadedVideos).ToList();
            var ratingItems = new List<RatingItem>();
            await Task.Run(() =>
                {
                    ratingItems.AddRange(from user in users
                        let likesNumber =
                            user.LoadedVideos.Sum(video => _context.LikedVideos.Count(v => v.VideoId == video.Id))
                        select new RatingItem {LikesNumber = likesNumber, User = user});
                });
            return ratingItems.OrderByDescending(r => r.LikesNumber).Take(number).ToList();
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
