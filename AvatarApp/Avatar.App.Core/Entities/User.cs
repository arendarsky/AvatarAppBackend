using System;
using System.Collections.Generic;
using Avatar.App.SharedKernel;

namespace Avatar.App.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string FireBaseId { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string ProfilePhoto { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }
        public string InstagramLogin { get; set; }
        public bool? ConsentToGeneralEmail {get; set;}
        public Semifinalist Semifinalist { get; set; }
        public ICollection<Video> LoadedVideos { get; set; }

        public ICollection<WatchedVideo> WatchedVideos { get; set; }

        public ICollection<LikedVideo> LikedVideos { get; set; }

        public ICollection<Message> ReceivedMessages { get; set; } 
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<BattleVote> BattleVotes { get; set; }
    }
}
