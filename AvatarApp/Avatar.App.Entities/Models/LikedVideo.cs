﻿namespace Avatar.App.Entities.Models
{
    public class LikedVideo
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long VideoId { get; set; }
        public User User { get; set; }
        public Video Video { get; set; }
    }
}