﻿namespace Avatar.App.Entities.Models
{
    public class WatchedVideo
    {
        public long Id { get; set; }
        public virtual User User { get; set; }
        public virtual Video Video { get; set; }
    }
}