﻿using System;

namespace Avatar.App.Infrastructure.Models.Casting
{
    internal class LikedVideoDb : BaseEntity
    {
        public long UserId { get; set; }
        public long VideoId { get; set; }
        public DateTime Date { get; set; }

        public UserDb User { get; set; }
        public VideoDb Video { get; set; }
    }
}
