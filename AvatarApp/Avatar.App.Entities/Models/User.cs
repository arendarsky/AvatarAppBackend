using System;
using System.Collections.Generic;
using System.Text;

namespace Avatar.App.Entities.Models
{
    public class User
    {
        public long Id { get; set; }

        public Guid Guid { get; set; }
        public string Email { get; set; }
        public ICollection<Video> LoadedVideos { get; set; }

        public ICollection<WatchedVideo> WatchedVideos { get; set; }
    }
}
