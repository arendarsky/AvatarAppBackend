using System;
using System.Collections.Generic;
using Avatar.App.Infrastructure.Models.Semifinal;

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

        public SemifinalistDb Semifinalist { get; set; }
        public ICollection<VideoDb> LoadedVideos { get; set; }
        public ICollection<WatchedVideoDb> WatchedVideos { get; set; }
        public ICollection<LikedVideoDb> LikedVideos { get; set; }
        public ICollection<BattleVoteDb> BattleVotes { get; set; }
    }
}
