using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.App.Context;
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

        public async Task<ICollection<RatingItem>> GetAsync(int number)
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
    }
}
