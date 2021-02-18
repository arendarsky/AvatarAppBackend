using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avatar.App.Infrastructure.Models.Semifinal;
using Microsoft.EntityFrameworkCore;

namespace Avatar.App.Infrastructure.Models.Casting
{
    internal class UserDb : BaseEntity
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string FireBaseId { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ProfilePhoto { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string InstagramLogin { get; set; }
        public bool? ConsentToGeneralEmail {get; set;}
        public string ConfirmationCode { get; set; }

        public SemifinalistDb Semifinalist { get; set; }
        public ICollection<VideoDb> LoadedVideos { get; set; }
        public ICollection<WatchedVideoDb> WatchedVideos { get; set; }

        public static async Task<UserDb> GetByGuidAsync(AvatarAppContext context, Guid userGuid)
        {
            return await context.Users.FirstOrDefaultAsync(user => user.Guid == userGuid);
        }
    }
}
