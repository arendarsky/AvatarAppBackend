using System;
using System.Collections.Generic;

namespace Avatar.App.Entities.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Video> LoadedVideos { get; set; }

        public ICollection<WatchedVideo> WatchedVideos { get; set; }

        public ICollection<LikedVideo> LikedVideos { get; set; }
    }
}
