using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Models.Casting
{
    internal class VideoDb: BaseEntity
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public bool? IsApproved { get; set; }
        public bool IsActive { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public UserDb User { get; set; }
        public ICollection<WatchedVideoDb> WatchedBy { get; set; }
        public ICollection<LikedVideoDb> LikedBy { get; set; }

        public static async Task<VideoDb> GetByNameAndUserGuidAsync(AvatarAppContext context, Guid userGuid, string fileName,
            CancellationToken cancellationToken = default)
        {
            return await context.Videos.FirstOrDefaultAsync(
                video => string.Equals(video.Name, fileName) && video.User.Guid == userGuid,
                cancellationToken);
        }
    }
}
